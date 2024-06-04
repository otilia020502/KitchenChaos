using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    [SerializeField] private float playerSize = .7f;
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField]float playerRadius = .7f;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 lastInteractionDir;
    private ClearCounter selectedCounter;
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteraction;
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
        

    }
    void Update()
    {   
        HandleMovement();
        HandleInteractions();
        
        
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                //has clear counter
                //clearCounter.Interact();
                if (clearCounter != selectedCounter)
                {
                    selectedCounter = clearCounter;
                }
            }
            else
            {
                selectedCounter = null;
            }
        }
        else
        {
            selectedCounter = null;
        }
       Debug.Log(selectedCounter);
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float playerHeight = 2f;
        
        var moveDistance = Time.deltaTime* moveSpeed;
        var step = moveDir * moveDistance;
        bool canMove= Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, playerRadius,moveDir, moveDistance)==false;
        if (!canMove)
        {//try to move on x axis
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            bool canMoveX= Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, playerRadius,moveDirX, moveDistance)==false;
            if (canMoveX)
            {
                canMove = true;
                step = moveDirX * moveDistance;
            }

            if (canMove == false)
            {//try to move on z axis
                var moveDirZ = new Vector3(0, 0, moveDir.z);
                bool canMoveZ= Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, playerRadius,moveDirZ, moveDistance)==false;
                if (canMoveZ)
                {
                    canMove = true;
                    step = moveDirZ * moveDistance;
                }
            }
        }
        if (canMove)
        {//if canmove any directon, move
            transform.position += step;
        }
        //Debug.Log(canMove);
        isWalking = moveDir != Vector3.zero;
        transform.forward=Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed) ;
    }
}