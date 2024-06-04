using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;
    [SerializeField] private float playerSize = .7f;
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField]float playerRadius = .7f;
    

    void Update()
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

    public bool IsWalking()
    {
        return isWalking;
    }
}