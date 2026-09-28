using UnityEngine;

public static class DirectionHandler
{
	public static QuarterDirection ToLocal(QuarterDirection worldDirection, QuarterRotation localRotation)
	{
			return (QuarterDirection)(((int)worldDirection - (int)localRotation + 4) % 4);
	}

	public static QuarterDirection ToWorld(QuarterDirection localDirection, QuarterRotation localRotation)
	{
			return (QuarterDirection)(((int)localDirection + (int)localRotation) % 4);
	}

	public static QuarterRotation GetRotation(float angle)
	{
			return (QuarterRotation)Mathf.RoundToInt(angle / 90f);
	}

	public static float GetAngle(QuarterRotation quarterRotation)
		{
			return quarterRotation switch
			{
				QuarterRotation.r_0 => 0,
				QuarterRotation.r_90 => 90,
				QuarterRotation.r_180 => 180,
				QuarterRotation.r_270 => 270,
				_ => 0
			};
		}

		public static QuarterRotation Rotate(QuarterDirection worldDirection, QuarterRotation localRotation)
		{
			return (QuarterRotation)(((int)worldDirection + (int)localRotation) % 4);
	}

		public static QuarterRotation Rotate(QuarterRotation rotation1, QuarterRotation rotation2)
		{
			return (QuarterRotation)(((int)rotation1 + (int)rotation2) % 4);
	}
}

public enum QuarterDirection
 {
	Up = 0,
	Right = 1,
	Down = 2,
	Left = 3
 }

public enum QuarterRotation
{
	r_0 = 0,
	r_90 = 1,
	r_180 = 2,
	r_270 = 3
}
