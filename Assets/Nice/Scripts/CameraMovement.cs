using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float rotationSpeed = 60f;

    private float yaw;
    private float pitch;

    private void Awake()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void OnEnable()
    {
        GameManager.GetInstance().Input.Camera.Enable();
    }

    private void OnDisable()
    {
        GameManager.GetInstance().Input.Camera.Disable();
    }

    private void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector2 move = GameManager.GetInstance().Input.Camera.Move.ReadValue<Vector2>();

        Vector3 movement = transform.right * move.x + transform.forward * move.y;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        transform.position += movement * movementSpeed * Time.deltaTime;
    }

    private void Rotate()
    {
        Vector2 rotate = GameManager.GetInstance().Input.Camera.Rotate.ReadValue<Vector2>();

        yaw += rotate.x * rotationSpeed * Time.deltaTime;
        pitch -= rotate.y * rotationSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -89f, 89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}