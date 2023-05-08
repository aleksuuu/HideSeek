using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
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
        _bestTime = PlayerPrefs.GetInt("bestTime");
    }


    private int _bestTime;
    public int BestTime
    {
        get => _bestTime;
        set
        {
            if (_bestTime == 0 || value < _bestTime)
            {
                _bestTime = value;
                PlayerPrefs.SetInt("bestTime", value);
                GUIBehavior.Instance.NewRecord(true);
            }
            else
            {
                GUIBehavior.Instance.NewRecord(false);
            }
        }
    }


    private State _currentState;
    public State CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            switch (value)
            {
                case State.Start:
                    GUIBehavior.Instance.ShowTutorialMessage();
                    GUIBehavior.Instance.ShowTitle(true);
                    PlayerStats.Instance.Reset();
                    EnemyStats.Instance.Reset();
                    TimerBehavior.Instance.Reset();
                    Time.timeScale = 0f;
                    break;
                case State.Play:
                    GUIBehavior.Instance.ClearMessage();
                    GUIBehavior.Instance.ShowTitle(false);
                    Time.timeScale = 1f;
                    break;
                case State.Pause:
                    GUIBehavior.Instance.ShowPauseMessage();
                    GUIBehavior.Instance.ShowTitle(true);
                    Time.timeScale = 0f; // On pause set timeScale to 0 so that powerups and portals do not disappear
                    break;
                case State.Win:
                    BestTime = TimerBehavior.Instance.CurrSecInt;
                    GUIBehavior.Instance.ShowTitle(true);
                    Time.timeScale = 0f;
                    break;
                case State.Lose:
                    GUIBehavior.Instance.ShowLoseMessage();
                    GUIBehavior.Instance.ShowTitle(true);
                    Time.timeScale = 0f;
                    break;
            }
        }
    }
    void Start()
    {
        CurrentState = State.Start;
    }

    void Update()
    {
        if (CurrentState != State.Play && Input.GetKey(KeyCode.W))
        {
            CurrentState = State.Play;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == State.Play)
            {
                CurrentState = State.Pause;
            }
            else
            {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
