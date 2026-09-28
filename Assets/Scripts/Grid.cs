using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	[SerializeField] private Renderer gridRenderer;

	[SerializeField] private int width = 20;
	[SerializeField] private int height = 20;

	[SerializeField] private Tile[,] tiles;
	[SerializeField] private List<Piece> pieces = new();

    private const float cellSize = 10f;

    private void Awake()
    {
        tiles = new Tile[width, height];
		gridRenderer.material.mainTextureScale = new Vector2(0.5f * width, 0.5f * height);
    }

	public bool IsInside(int x, int z)
	{
		return x >= 0 && x < width && z >= 0 && z < height;
	}

	public Tile GetTile(int x, int z)
	{
		if (!IsInside(x, z))
        {
            return null;
        }

		return tiles[x, z];
	}

	private bool TrySetTile(int x, int z, Tile tile)
	{
		if (!IsInside(x, z))
		{
            return false;
        }

		tiles[x, z] = tile;
		return true;
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
		return IsInside(x, z) && tiles[x, z] == null;
	}

	public bool CanPlacePiece(Piece piece)
	{
		if (piece)
        {
            foreach (Tile tile in piece.tiles)
            {
                if (!CanPlaceTile(tile))
                {
                    return false;
                }
            }

            return true;
        }

        return false;
	}

    public bool CanPlaceTile(Tile tile)
    {
        Vector2Int gridPosition = WorldToGrid(tile.transform.position);
        return IsFree(gridPosition.x, gridPosition.y);
    }

	public bool AddPiece(Piece piece)
	{
		if (!pieces.Contains(piece) && CanPlacePiece(piece))
        {
            foreach (Tile tile in piece.tiles)
            {
                
            }

            pieces.Add(piece);
            return true;
        }
		
        return false;
	}

	public bool RemovePiece(Piece piece)
	{
		if (piece && pieces.Contains(piece))
        {
            foreach (Tile tile in piece.tiles)
            {

		    }

		    pieces.Remove(piece);
            return true;
        }

        return false;
	}

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

	public void Enlarge()
	{
		const int amount = 5;

		int oldWidth = width;
		int oldHeight = height;

		Tile[,] oldTiles = tiles;

		int newWidth = oldWidth + amount * 2;

		int newHeight = oldHeight + amount * 2;

		Tile[,] newTiles = new Tile[newWidth, newHeight];

		for (int x = 0; x < oldWidth; x++)
		{
			for (int z = 0; z < oldHeight; z++)
			{
				Tile tile = oldTiles[x, z];

				if (tile == null)
					continue;

				int newX = x + amount;
				int newZ = z + amount;

				newTiles[newX, newZ] = tile;
			}
		}

		width = newWidth;
		height = newHeight;
		tiles = newTiles;
		gridRenderer.material.mainTextureScale = new Vector2(0.5f * width, 0.5f * height);
		

		for (int x = 0; x < width; x++)
		{
			for (int z = 0; z < height; z++)
			{
				Tile tile = tiles[x, z];

				if (tile != null)
				{
					tile.transform.position =
						GridToWorld(x, z);
				}
			}
		}

		foreach (Piece piece in pieces)
		{
			if (piece == null)
				continue;

			piece.transform.position += new Vector3(amount * cellSize, 0f, amount * cellSize);
		}
	}
}