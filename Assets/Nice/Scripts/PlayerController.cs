using UnityEngine;

public class PlayerController : MonoBehaviourInstance<PlayerController>
{
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
        
    }
}
