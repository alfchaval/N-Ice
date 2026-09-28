using UnityEngine;

public class ScenarioEditor : MonoBehaviour
{
	public Grid grid;

	[Header("Piece Prefabs")]
	public IcePiece icePiecePrefab;
	public GroundPiece groundPiecePrefab;
	public WallPiece wallPiecePrefab;

	private Piece selectedPiece;
	private bool isDragging;
	private bool isCurrentPositionValid;

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

	}

	public void BeginDrag(Piece piece)
	{
		if (isDragging)
        {
            EndDrag();
        }

		selectedPiece = piece;
		grid.RemovePiece(piece);

		isDragging = true;
	}

	public void UpdateDrag(Vector3 mouseWorldPosition)
	{
		if (isDragging && selectedPiece)
        {
            Vector2Int gridPosition = grid.WorldToGrid(mouseWorldPosition);

            //currentGridPosition = gridPosition;

            isCurrentPositionValid = grid.CanPlacePiece(selectedPiece);

            //if (currentPositionValid)
            {
                selectedPiece.transform.position = grid.GridToWorld(gridPosition.x, gridPosition.y);

                //selectedPiece.SetPreviewState(PiecePreviewState.Valid);
            }
            //else
            {
                selectedPiece.transform.position = mouseWorldPosition;

                //selectedPiece.SetPreviewState(PiecePreviewState.Invalid);
            }
        }
	}

	public void EndDrag()
	{
		if (!isDragging || selectedPiece == null)
			return;

		isDragging = false;

		Piece piece = selectedPiece;

		if (isCurrentPositionValid)
		{
			//bool added = grid.AddPiece(piece, currentGridPosition);

			//if (added)
			{
				//piece.SetPreviewState(PiecePreviewState.None);
			}
			//else
			{
				Destroy(piece.gameObject);
			}
		}
		else
		{
			Destroy(piece.gameObject);
		}

		selectedPiece = null;
		isCurrentPositionValid = false;
	}

	public void RotateClockwise()
	{
		if (!isDragging || selectedPiece == null)
			return;

		selectedPiece.RotateClockwise();

		RecalculateCurrentPosition();
	}

	public void RotateCounterClockwise()
	{
		if (!isDragging || selectedPiece == null)
			return;

		selectedPiece.RotateCounterClockwise();

		RecalculateCurrentPosition();
	}

	private void RecalculateCurrentPosition()
	{
		//isCurrentPositionValid = grid.CanPlacePiece(selectedPiece, currentGridPosition);

		if (isCurrentPositionValid)
		{
			//selectedPiece.transform.position = grid.GridToWorld(currentGridPosition.x, currentGridPosition.y);

			//selectedPiece.SetPreviewState(PiecePreviewState.Valid);
		}
		else
		{
			//selectedPiece.SetPreviewState(PiecePreviewState.Invalid);
		}
	}

	public void CancelDrag()
	{
		if (!isDragging || selectedPiece == null)
			return;

		Piece piece = selectedPiece;

		selectedPiece = null;
		isDragging = false;

		Destroy(piece.gameObject);
	}
}
