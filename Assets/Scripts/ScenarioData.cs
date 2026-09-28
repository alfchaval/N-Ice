using System;
using System.Collections.Generic;

[Serializable]
public class ScenarioData
{
	public int width;
	public int height;
	public float cellSize;

	public List<PieceData> pieces =
		new List<PieceData>();
}

[Serializable]
public class PieceData
{
	public string pieceId;

	public int x;
	public int z;

	public int rotation;
}