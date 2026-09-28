using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 60f;

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A)) horizontal -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontal += 1f;
        if (Input.GetKey(KeyCode.S)) vertical -= 1f;
        if (Input.GetKey(KeyCode.W)) vertical += 1f;

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        if (movement.sqrMagnitude > 1f) movement.Normalize();

        transform.position += movement * movementSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        float yaw = 0f;
        float pitch = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) yaw -= 1f;
        if (Input.GetKey(KeyCode.RightArrow)) yaw += 1f;
        if (Input.GetKey(KeyCode.UpArrow)) pitch -= 1f;
        if (Input.GetKey(KeyCode.DownArrow))  pitch += 1f;

        transform.Rotate(pitch * rotationSpeed * Time.deltaTime, yaw * rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
