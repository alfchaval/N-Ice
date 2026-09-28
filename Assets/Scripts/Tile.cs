using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [Header("Enter Directions")]
	[SerializeField] private bool enter_up = true;
	[SerializeField] private bool enter_right = true;
	[SerializeField] private bool enter_down = true;
	[SerializeField] private bool enter_left = true;

	[Header("Exit Directions")]
	[SerializeField] private bool exit_up = true;
	[SerializeField] private bool exit_right = true;
	[SerializeField] private bool exit_down = true;
	[SerializeField] private bool exit_left = true;

	protected bool CanEnter(QuarterDirection localDirection)
	{
		return localDirection switch
		{
			QuarterDirection.Up => enter_up,
			QuarterDirection.Right => enter_right,
			QuarterDirection.Down => enter_down,
			QuarterDirection.Left => enter_left,
			_ => false
		};
	}

    protected bool CanExit(QuarterDirection localDirection)
        {
            return localDirection switch
            {
                QuarterDirection.Up => exit_up,
                QuarterDirection.Right => exit_right,
                QuarterDirection.Down => exit_down,
                QuarterDirection.Left => exit_left,
                _ => false
            };
        }

        public void TryEnter(QuarterDirection worldDirection)
    {
    QuarterDirection localDirection = DirectionHandler.ToLocal(worldDirection, DirectionHandler.GetRotation(transform.eulerAngles.y));
        if(CanEnter(localDirection))
        {
            Enter(localDirection);
        }
    }

    public void TryExit(QuarterDirection worldDirection)
    {
        QuarterDirection localDirection = DirectionHandler.ToLocal(worldDirection, DirectionHandler.GetRotation(transform.eulerAngles.y));
        if(CanExit(localDirection))
        {
            Exit(localDirection);
        }
    }

    protected abstract void Enter(QuarterDirection localDirection);

    protected abstract void Exit(QuarterDirection localDirection);
}
