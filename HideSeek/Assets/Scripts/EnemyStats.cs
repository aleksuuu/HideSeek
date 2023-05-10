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

    [SerializeField] Vector3 initPosition = Vector3.zero;
    public int TotalLives = 3;
    private int _remainingLives = 3;
    public int RemainingLives
    {
        get => _remainingLives;
        set
        {
            _remainingLives = value;
            GUIBehavior.Instance.UpdateEnemyHearts();
            if (value < 1)
            {
                GameBehavior.Instance.CurrentState = State.Win;
            }
        }
    }
    public float SecondsBeforeHarmingPlayer = 8f;
    public int SecondsBeforePossiblyChasing = 30;

    public void Reset()
    {
        RemainingLives = TotalLives;
        transform.position = initPosition;
        SecondsBeforePossiblyChasing = Random.Range(0, 1);
    }
}
