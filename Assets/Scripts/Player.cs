using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput _gameInput;
    private bool isWalking;

    void Update()
    {
        Vector2 inputVector = _gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = moveDir != Vector3.zero;
        // Multiply moveSpeed with Time.deltaTime first for better performance
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        float rotateSpeed = 10f;
        
        transform.forward=Vector3.Slerp(transform.forward, moveDir, Time.deltaTime*rotateSpeed) ;
        
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}