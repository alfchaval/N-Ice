using UnityEngine;

public class ScenarioEditor : MonoBehaviour
{
	public GameMapGrid grid;

	[Header("Piece Prefabs")]
	public IcePiece icePiecePrefab;
	public GroundPiece groundPiecePrefab;
	public WallPiece wallPiecePrefab;

	private Piece selectedPiece;

	private void Update()
	{
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

	public void SelectPiece(Piece piece)
	{
		if (selectedPiece)
        {
            UnselectPiece();
        }

		selectedPiece = piece;
		grid.RemovePiece(piece);
	}

	public void UpdateDrag()
	{
		if (selectedPiece)
        {
			float denominator = Vector3.Dot(Camera.main.transform.forward, grid.transform.up);
			if (Mathf.Abs(denominator) >= 0.1f)
			{
				float t = Vector3.Dot(grid.transform.position - Camera.main.transform.position, grid.transform.up) / denominator;
				Vector3 pointInGrid = Camera.main.transform.position + t * Camera.main.transform.forward;
				Vector2 gridSize = grid.GetSize();
				
				if(Mathf.Abs(pointInGrid.x) < GameMapGrid.cellSize * gridSize.x / 2
				&& Mathf.Abs(pointInGrid.z) < GameMapGrid.cellSize * gridSize.y / 2)
				{
					/*
					if(LegalPosition())
					{
						selectedPiece.SetPreviewColor(green);
					}
					else
					{
						selectedPiece.SetPreviewColor(red);
					}
					*/
				}
				else
				{
					selectedPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
				}
			}
			else
			{
				selectedPiece.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
			}
        }
	}

	public void UnselectPiece()
	{
		if (selectedPiece)
		{
			float denominator = Vector3.Dot(Camera.main.transform.forward, grid.transform.up);
			if (Mathf.Abs(denominator) >= 0.1f)
			{
				float t = Vector3.Dot(grid.transform.position - Camera.main.transform.position, grid.transform.up) / denominator;
				Vector3 pointInGrid = Camera.main.transform.position + t * Camera.main.transform.forward;
				Vector2 gridSize = grid.GetSize();
				
				if(Mathf.Abs(pointInGrid.x) < GameMapGrid.cellSize * gridSize.x / 2
				&& Mathf.Abs(pointInGrid.z) < GameMapGrid.cellSize * gridSize.y / 2)
				{
					/*
					selectedPiece.transform.position = PositionInGrid
					if(LegalPosition())
					{
						AddToGrid(selectedPiece);
					}
					*/
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

	public void RotateClockwise()
	{
		if (selectedPiece)
		{
			selectedPiece.RotateClockwise();
		}
	}

	public void RotateCounterClockwise()
	{
		if (selectedPiece)
		{
			selectedPiece.RotateCounterClockwise();
		}
	}
}