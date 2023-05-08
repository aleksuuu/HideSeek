using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public static EnemyStats Instance;
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
            GUIBehavior.Instance.EnemyLives = value;
            _remainingLives = value;
            if (value < 1)
            {
                GameBehavior.Instance.CurrentState = State.Win;
            }
        }
    }
    public float SecondsBeforeHarmingPlayer = 8f;

    public void Reset()
    {
        RemainingLives = TotalLives;
    }
}
