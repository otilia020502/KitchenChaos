using UnityEngine;
using System;
using System.Collections.Generic;
using Counters;
using Unity.Netcode;
public class Player : NetworkBehaviour,IKitchenObjectParent
{

    public static event EventHandler OnAnyPlayerSpawned;
    public static event EventHandler OnAnyPickedSomething;
    public static Player LocalInstance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged ;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }
    [SerializeField] private float moveSpeed = 5f;
    
    private bool _isWalking; 
    [SerializeField] private float playerSize = .7f;//
    [SerializeField]float rotateSpeed = 10f;
    [SerializeField]float playerRadius = .7f;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private LayerMask collisionsLayerMask;
    [SerializeField] private List<Vector3> spawnPositionList;
    private Vector3 _lastInteractionDir;
    private BaseCounter _selectedCounter;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private KitchenObject kitchenObject;
    
    public override void OnNetworkSpawn()
    {
        if(IsOwner){
            
                LocalInstance = this;
            
        }

        transform.position = spawnPositionList[(int)OwnerClientId];
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
            
        
        
    }

    private void Start()//iau de la altii
    {
        GameInput.Instance.OnInteractAction -= GameInput_OnInteraction;
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
    public static void ResetStaticData()
    {
        OnAnyPlayerSpawned = null;
    }
    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.GameIsPlaying() == false)
        {
            return;
        }
        if (_selectedCounter != null)
        {
            Debug.Log("interact called");
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
            OnAnyPickedSomething?.Invoke(this, EventArgs.Empty);
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
        bool canMove= Physics.BoxCast(transform.position, Vector3.one * playerRadius,
            moveDir,Quaternion.identity, moveDistance, collisionsLayerMask)==false;
        
        if (!canMove)
        {//try to move on x axis
            var moveDirX = new Vector3(moveDir.x, 0, 0);
            bool canMoveX= (moveDir.x <-.5f || moveDir.x>.5f) && Physics.BoxCast(transform.position, Vector3.one * playerRadius,
                moveDirX,Quaternion.identity, moveDistance, collisionsLayerMask)==false;
            if (canMoveX)
            {
                canMove = true;
                step = moveDirX * moveDistance;
            }

            if (canMove == false)
            {//try to move on z axis
                var moveDirZ = new Vector3(0, 0, moveDir.z);
                bool canMoveZ= (moveDir.z <-.5f || moveDir.z>.5f) &&Physics.BoxCast(transform.position, Vector3.one * playerRadius,
                    moveDirZ,Quaternion.identity, moveDistance, collisionsLayerMask)==false;
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }
}