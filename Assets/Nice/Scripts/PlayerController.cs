using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourInstance<PlayerController>
{
    private Rigidbody2D rB2D;
    public float speed;
    private Vector2 movedDirection;

    public InputActionReference move;
    public InputActionReference interact;

    public bool deslizandose;

    public Animator playerAnimatorController;
    public void Start()
    {
    
    }
    public Tile GetCurrentTile()
    {
        GameMapGrid grid = GameMapGrid.GetInstance();
        if(grid)
        {
            Vector2Int gridPosition = grid.WorldToGrid(transform.position);
            return grid.GetTile(gridPosition.x, gridPosition.y);
        }
        return null;
    }

    private void Update()
    {
        movedDirection = move.action.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (!deslizandose) 
        {
            playerAnimatorController.SetBool("Desliza", false);
            if (Mathf.Abs(movedDirection.x )>= 0.2f || Mathf.Abs(movedDirection.y) >= 0.2f)
            {
                playerAnimatorController.SetBool("Run", true);
            }
            else if (movedDirection.x <= 0.2f && movedDirection.y <= 0.2f)
            {
                playerAnimatorController.SetBool("Run", false);
            }
        }
        else
        {
            playerAnimatorController.SetBool("Desliza", true);
            playerAnimatorController.SetBool("Run", false);
        }
       
    }

    private void OnEnable()
    {
        if (interact == null || interact.action == null)
        {
            Debug.LogError("Interact no está asignado en el Inspector");
            return;
        }

        Debug.Log($"Acción: {interact.action.name}");
        Debug.Log($"Habilitada: {interact.action.enabled}");

        interact.action.started += Interactuar;
    }

    private void OnDisable()
    {
        if (interact != null && interact.action != null)
        {
            interact.action.started -= Interactuar;
        }
    }

    private void Interactuar(InputAction.CallbackContext obj)
    {
        Debug.Log("¡TECLA E DETECTADA!");
        deslizandose = !deslizandose;
    }
}
