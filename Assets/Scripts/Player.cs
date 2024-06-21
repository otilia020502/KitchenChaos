using UnityEngine;
using System;
using Unity.Netcode;
public class Player : NetworkBehaviour,IKitchenObjectParent
{
    
    //public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged ;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }
    [SerializeField] private float moveSpeed = 5f;
    
    private bool _isWalking; [SerializeField] private float playerSize = .7f;//
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField]float playerRadius = .7f;
    [SerializeField] private LayerMask countersLayerMask;
    private Vector3 _lastInteractionDir;
    private BaseCounter _selectedCounter;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;

    private void Awake()//happens before start, setez eu
    {
        
        //    Instance = this;
            
      
        
        
    }
    private void Start()//iau de la altii
    {
        GameInput.Instance.OnInteractAction += GameInput_OnInteraction;
        GameInput.Instance.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }
    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.GameIsPlaying() == false)
        {
            return;
        }
        if (_selectedCounter != null)
        {
            _selectedCounter.InteractAlternate(this);
        }
        

    }
    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.GameIsPlaying() == false)
        {
            return;
        }
        if (_selectedCounter != null)
        {
            _selectedCounter.Interact(this);
        }
        

    }
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
        HandleInteractions();
        
        
    }
    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }


    public bool IsWalking()
    {
        return _isWalking;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        if (moveDir != Vector3.zero)
        {
            _lastInteractionDir = moveDir;
        }
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractionDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //has clear counter
                //clearCounter.Interact();
                if (baseCounter != _selectedCounter)
                {
                    _selectedCounter = baseCounter;
                    SetSelectedCounter(baseCounter);
                }
               
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
       
    }
    
    private void HandleMovement()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float playerHeight = 2f;
        
        var moveDistance = Time.deltaTime* moveSpeed;
        var step = moveDir * moveDistance;
        bool canMove= Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, 
            playerRadius,moveDir, moveDistance, countersLayerMask)==false;
        
        if (!canMove)
        {//try to move on x axis
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            bool canMoveX= (moveDir.x <-.5f || moveDir.x>.5f) && Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, playerRadius,moveDirX, moveDistance)==false;
            if (canMoveX)
            {
                canMove = true;
                step = moveDirX * moveDistance;
            }

            if (canMove == false)
            {//try to move on z axis
                var moveDirZ = new Vector3(0, 0, moveDir.z);
                bool canMoveZ= (moveDir.z <-.5f || moveDir.z>.5f) &&Physics.CapsuleCast(transform.position, transform.position+Vector3.up * playerHeight, playerRadius,moveDirZ, moveDistance)==false;
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
        _isWalking = moveDir != Vector3.zero;
        transform.forward=Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed) ;
    }

    private void SetSelectedCounter(BaseCounter newSelectedCounter)
    {
        _selectedCounter = newSelectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            SelectedCounter = newSelectedCounter
        });
    }

    public bool TryGetPlateObject(out PlatekitchenObject platekitchenObject)
    {
        if (kitchenObject.TryGetComponent(out PlatekitchenObject plate))
        {
            platekitchenObject = plate;
            return true;
        }

        platekitchenObject = null;
        return false;
    }
}