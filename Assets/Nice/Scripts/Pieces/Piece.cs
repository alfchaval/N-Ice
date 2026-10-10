using UnityEngine;

public abstract class Piece : MonoBehaviour
{
	public Tile[] tiles;

	private Renderer[] renderers;
	private MaterialPropertyBlock propertyBlock;

	private QuarterRotation pieceRotation;

	private const float previewAlpha = 0.5f;

	protected virtual void Awake()
	{
		foreach (Tile t in tiles)
		{
			t.piece = this;
		}

		renderers = GetComponentsInChildren<Renderer>(true);
		propertyBlock = new MaterialPropertyBlock();

		pieceRotation = DirectionHandler.GetRotation(transform.eulerAngles.y);
		SetRotation(pieceRotation);
	}

	public void SetRotation(QuarterRotation rotation)
	{
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, DirectionHandler.GetAngle(rotation), transform.eulerAngles.z);
	}

	public void RotateClockwise()
	{
		pieceRotation = DirectionHandler.Rotate(pieceRotation, QuarterRotation.r_90);
		SetRotation(pieceRotation);
	}

	public void RotateCounterClockwise()
	{
		pieceRotation = DirectionHandler.Rotate(pieceRotation, QuarterRotation.r_270);
		SetRotation(pieceRotation);
	}

	public void SetPreviewColor(Color? color)
	{
		if (renderers == null)
		{
			renderers = GetComponentsInChildren<Renderer>(true);
		}

		foreach (Renderer renderer in renderers)
		{
			renderer.GetPropertyBlock(propertyBlock);

			if (color.HasValue)
			{
				propertyBlock.SetColor("_Color", color.Value);
				propertyBlock.SetColor("_BaseColor", color.Value);
			}
			else
			{
				propertyBlock.Clear();
			}

			renderer.SetPropertyBlock(propertyBlock);
		}
	}
}
