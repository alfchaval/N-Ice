using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMapGrid : MonoBehaviourInstance<GameMapGrid>
{
	[SerializeField] private Renderer gridRenderer;

	[SerializeField] private int width = 20;
	[SerializeField] private int height = 20;

	[SerializeField] private Dictionary<Vector2Int, Tile> tiles = new Dictionary<Vector2Int, Tile>();
	[SerializeField] private List<Piece> pieces = new();

    public const float cellSize = 1f;

	private void Update()
	{
		foreach(Piece p in pieces)
		{
			print(p.transform.position);
		}
	}

    protected override void OnInstance()
    {
		gridRenderer.material.mainTextureScale = new Vector2(0.5f * width, 0.5f * height);
		gridRenderer.transform.localScale = new Vector3(width * cellSize / 10, 0, height * cellSize / 10);
    }

	public bool IsInside(int x, int z)
	{
		print("coordinates: " + x + ", " + z);
		return x >= -width * cellSize / 2 && x < width * cellSize / 2 && z >= -height * cellSize / 2 && z < height * cellSize / 2;
	}

	public Tile GetTile(int x, int z)
	{
		if (!IsInside(x, z))
        {
            return null;
        }

		return tiles[new Vector2Int(x, z)];
	}

	public Vector3 GridToWorld(int x, int z)
	{
		return transform.position + new Vector3(x * cellSize, 0f, z * cellSize);
	}

	public Vector2Int WorldToGrid(Vector3 worldPosition)
	{
		Vector3 localPosition = worldPosition - transform.position;
		return new Vector2Int(Mathf.RoundToInt(localPosition.x / cellSize), Mathf.RoundToInt(localPosition.z / cellSize));
	}

	public bool IsFree(int x, int z)
	{
		print("IsInside: " + IsInside(x, z));
		print("IsNull: " + tiles[new Vector2Int(x, z)] == null);
		return IsInside(x, z) && tiles.ContainsKey(new Vector2Int(x, z)) && tiles[new Vector2Int(x, z)] == null;
	}

	public Vector3 ClosestTileCenter(Vector3 p)
	{
		float x = width % 2 == 0 ? ((int)p.x) + 0.5f * Mathf.Sign(p.x) : Mathf.Round(p.x);
		float z = height % 2 == 0 ? ((int)p.z) + 0.5f * Mathf.Sign(p.z) : Mathf.Round(p.z);
		return new Vector3(x, 0, z);
	}

	public bool CanPlacePiece(Piece piece)
	{
		Vector2Int tilePosition = WorldToGrid(ClosestTileCenter(piece.transform.position));
		print(tilePosition);
		return IsFree(tilePosition.x, tilePosition.y);
	}

	public bool TryPlacePiece(Piece piece)
	{
		if (!pieces.Contains(piece) && CanPlacePiece(piece))
        {
			Vector2Int tilePosition = WorldToGrid(ClosestTileCenter(piece.transform.position));
			tiles[tilePosition] = piece.tiles[0];
            pieces.Add(piece);
			print(pieces.Count);
            return true;
        }
		
        return false;
	}

	public bool RemovePiece(Piece piece)
	{
		if (piece && pieces.Contains(piece))
        {
			List<Tile> tilesToRemove = piece.tiles.ToList();
			foreach (KeyValuePair<Vector2Int, Tile> pair in tiles)
			{
				if (tilesToRemove.Contains(pair.Value))
				{
					tilesToRemove.Remove(pair.Value);
					if(tilesToRemove.Count == 0)
					{
						break;
					}
				}
			}

		    pieces.Remove(piece);
            return true;
        }

        return false;
	}

/*
	public void FitToTiles()
	{
        if(pieces.Count > 0)
        {
            int minX = int.MaxValue;
            int minZ = int.MaxValue;
            int maxX = int.MinValue;
            int maxZ = int.MinValue;

            bool foundTile;

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    foundTile = tiles[x, z];
                    if (foundTile)
                    {
                        minX = Mathf.Min(minX, x);
                        minZ = Mathf.Min(minZ, z);
                        maxX = Mathf.Max(maxX, x);
                        maxZ = Mathf.Max(maxZ, z);
                    }
                }
            }

            int newWidth = maxX - minX + 1;
		    int newHeight = maxZ - minZ + 1;

            if (newWidth <= tiles.GetLength(0) || newHeight <= tiles.GetLength(1))
            {
                Tile[,] newTiles = new Tile[newWidth, newHeight];

                for (int x = minX; x <= maxX; x++)
                {
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        Tile tile = tiles[x, z];

                        if (tile)
                        {
                            int newX = x - minX;
                            int newZ = z - minZ;

                            newTiles[newX, newZ] = tile;
                        }
                    }
                }

                tiles = newTiles;
				width = newWidth;
				height = newHeight;
				gridRenderer.material.mainTextureScale = new Vector2(0.5f * width, 0.5f * height);
				gridRenderer.transform.localScale = new Vector3(width * cellSize / 10, 0, height * cellSize / 10);

                foreach (Piece piece in pieces)
                {
                    piece.transform.position = GridToWorld(
                        WorldToGrid(piece.transform.position).x - minX,
                        WorldToGrid(piece.transform.position).y - minZ
                    );
                }
            }
        }
	}

	*/

	public void Enlarge()
	{
		width += 5;
		height += 5;
	}

	public Vector2 GetSize()
	{
		return new Vector2(width, height);
	}
}