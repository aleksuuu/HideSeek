using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    public int TotalLives = 3;


    private int _remainingLives = 3;
    public int RemainingLives
    {
        get => _remainingLives;
        set
        {    
            GUIBehavior.Instance.PlayerLives = value;
            _remainingLives = value;
            if (value < 1)
            {
                GameBehavior.Instance.CurrentState = State.Lose;
            }
        }
    }

    public void Reset()
    {
        RemainingLives = TotalLives;
    }

}
