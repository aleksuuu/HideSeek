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


    public Vector3 initPosition = Vector3.zero;
    public int TotalLives = 3;


    private int _remainingLives = 3;
    public int RemainingLives
    {
        get => _remainingLives;
        set
        {
            _remainingLives = value;
            GUIBehavior.Instance.UpdatePlayerHearts();
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

    public float BoxProgress = 0f;

    public float NumberOfSecondsForBoxToBeAvailable = 15f;

    public bool BoxIsAvailable = false;

    public bool DoStartBoxProgress = true;

}
