using UnityEngine;

public class GameManager : MonoBehaviourInstance<GameManager>
{
    public GameInputActions Input;

    protected override void OnInstance()
    {
        Input = new GameInputActions();
    }
}
