using UnityEngine;
using UnityEngine.InputSystem;

public class ScenarioEditor : MonoBehaviour
{
	public GameMapGrid grid;

	public Color correctPlacement;
	public Color wrongPlacement;

	[Header("Piece Prefabs")]
	public IcePiece icePiecePrefab;
	public GroundPiece groundPiecePrefab;
	public WallPiece wallPiecePrefab;

	private Piece selectedPiece;

	private const float draggingFromCamera = 2f;

	private void Update()
	{
		CheckSelection();
		UpdateDrag();
	}

	public void SpawnIcePiece()
	{
		SpawnPiece(icePiecePrefab.gameObject);
	}

	public void SpawnGroundPiece()
	{
		SpawnPiece(groundPiecePrefab.gameObject);
	}

	public void SpawnWallPiece()
	{
		SpawnPiece(wallPiecePrefab.gameObject);
	}

	private void SpawnPiece(GameObject piecePrefab)
	{
		GameObject newPiece = Instantiate(piecePrefab);
		SelectPiece(newPiece.GetComponent<Piece>());
	}

	private void SelectPiece(Piece piece)
	{
		if (selectedPiece)
        {
            UnselectPiece();
        }

		selectedPiece = piece;
		grid.RemovePiece(piece);
	}

	private void CheckSelection()
	{
		if(Mouse.current.leftButton.wasPressedThisFrame)
		{
			if(selectedPiece)
			{
				UnselectPiece();
			}
			else
			{
				Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, draggingFromCamera));
				Vector3 cameraToMouse = Vector3.Normalize(mouseInWorld - Camera.main.transform.position);
				float denominator = Vector3.Dot(cameraToMouse, grid.transform.up);

				if (Mathf.Abs(denominator) >= 0.1f)
				{
					float t = Vector3.Dot(grid.transform.position - Camera.main.transform.position, grid.transform.up) / denominator;
					Vector3 pointInGrid = Camera.main.transform.position + t * cameraToMouse;
					Vector2Int tilePosition = grid.WorldToGrid(pointInGrid);
					Tile tile = grid.GetTile(tilePosition.x, tilePosition.y);
					if (tile)
					{
						selectedPiece = tile.piece;
						grid.RemovePiece(tile.piece);
					}
				}
			}
		}
	}

	private void UpdateDrag()
	{
		if (selectedPiece)
        {
			Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, draggingFromCamera));
			Vector3 cameraToMouse = Vector3.Normalize(mouseInWorld - Camera.main.transform.position);
			float denominator = Vector3.Dot(cameraToMouse, grid.transform.up);

			if (Mathf.Abs(denominator) >= 0.1f)
			{
				float t = Vector3.Dot(grid.transform.position - Camera.main.transform.position, grid.transform.up) / denominator;
				Vector3 pointInGrid = Camera.main.transform.position + t * cameraToMouse;
				Vector2 gridSize = grid.GetSize();
				
				if(Mathf.Abs(pointInGrid.x) < GameMapGrid.cellSize * gridSize.x / 2
				&& Mathf.Abs(pointInGrid.z) < GameMapGrid.cellSize * gridSize.y / 2)
				{
					selectedPiece.transform.position = pointInGrid;
					selectedPiece.transform.position = grid.ClosestTileCenter(selectedPiece.tiles[0].transform.position) - selectedPiece.tiles[0].transform.localPosition;
					if(grid.CanPlacePiece(selectedPiece))
					{
						//selectedPiece.SetPreviewColor(correctPlacement);
					}
					else
					{
						//selectedPiece.SetPreviewColor(wrongPlacement);
					}
				}
				else
				{
					selectedPiece.transform.position = mouseInWorld;
				}
			}
			else
			{
				selectedPiece.transform.position = mouseInWorld;
			}
        }
	}

	private void UnselectPiece()
	{
		if (selectedPiece)
		{
			Vector3 mouseInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, draggingFromCamera));
			Vector3 cameraToMouse = Vector3.Normalize(mouseInWorld - Camera.main.transform.position);
			float denominator = Vector3.Dot(cameraToMouse, grid.transform.up);

			if (Mathf.Abs(denominator) >= 0.1f)
			{
				float t = Vector3.Dot(grid.transform.position - Camera.main.transform.position, grid.transform.up) / denominator;
				Vector3 pointInGrid = Camera.main.transform.position + t * cameraToMouse;
				Vector2 gridSize = grid.GetSize();
				
				if(Mathf.Abs(pointInGrid.x) < GameMapGrid.cellSize * gridSize.x / 2
				&& Mathf.Abs(pointInGrid.z) < GameMapGrid.cellSize * gridSize.y / 2)
				{
					selectedPiece.transform.position = pointInGrid;
					selectedPiece.transform.position = grid.ClosestTileCenter(selectedPiece.tiles[0].transform.position) - selectedPiece.tiles[0].transform.localPosition;
					if(grid.CanPlacePiece(selectedPiece))
					{
						bool placed = grid.TryPlacePiece(selectedPiece);
						if(!placed)
						{
							Destroy(selectedPiece.gameObject);
						}
					}
					selectedPiece = null;
					Destroy(selectedPiece.gameObject);
				}
				else
				{
					Destroy(selectedPiece.gameObject);
				}
			}
			else
			{
				Destroy(selectedPiece.gameObject);
			}

			selectedPiece = null;
		}
	}

	private void RotateClockwise()
	{
		if (selectedPiece)
		{
			selectedPiece.RotateClockwise();
		}
	}

	private void RotateCounterClockwise()
	{
		if (selectedPiece)
		{
			selectedPiece.RotateCounterClockwise();
		}
	}
}