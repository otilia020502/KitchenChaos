using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    void Update()
    {
        Vector2 inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1f;
        }

        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // Multiply moveSpeed with Time.deltaTime first for better performance
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
        Debug.Log(Time.deltaTime);
    }
}