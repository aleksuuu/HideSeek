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
                    ResetEverything();
                    Time.timeScale = 0f;
                    break;
                case State.Play:
                    GUIBehavior.Instance.ClearMessage();
                    GUIBehavior.Instance.ShowTitle(false);
                    SoundtrackBehavior.Instance.FadeInSoundtrack();
                    Time.timeScale = 1f;
                    break;
                case State.Pause:
                    GUIBehavior.Instance.ShowPauseMessage();
                    GUIBehavior.Instance.ShowTitle(true);
                    SoundtrackBehavior.Instance.LowPassSoundtrack();
                    Time.timeScale = 0f;
                    break;
                case State.Win:
                    BestTime = TimerBehavior.Instance.CurrSecInt;
                    GUIBehavior.Instance.ShowTitle(true);
                    SoundtrackBehavior.Instance.FadeOutSoundtrack();
                    Time.timeScale = 0f;
                    break;
                case State.Lose:
                    GUIBehavior.Instance.ShowLoseMessage();
                    GUIBehavior.Instance.ShowTitle(true);
                    SoundtrackBehavior.Instance.FadeOutSoundtrack();
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
        if ((CurrentState == State.Start || CurrentState == State.Pause)
            && Input.GetKey(KeyCode.W))
        {
            CurrentState = State.Play;
        }
        if ((CurrentState == State.Win || CurrentState == State.Lose)
            && Input.GetKey(KeyCode.Return))
        {
            CurrentState = State.Start;
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


        if (PlayerStats.Instance.BoxProgress < PlayerStats.Instance.NumberOfSecondsForBoxToBeAvailable)
        {
            PlayerStats.Instance.BoxIsAvailable = false;
            GUIBehavior.Instance.SetBoxSlider(PlayerStats.Instance.BoxProgress / PlayerStats.Instance.NumberOfSecondsForBoxToBeAvailable);
            GUIBehavior.Instance.MakeBoxIconTransparent();
            if (PlayerStats.Instance.DoStartBoxProgress)
            {
                PlayerStats.Instance.BoxProgress += Time.deltaTime;
            }
        }
        else
        {
            PlayerStats.Instance.BoxIsAvailable = true;
            GUIBehavior.Instance.SetBoxSlider(1f);
            GUIBehavior.Instance.MakeBoxIconSolid();
        }
    }

    void ResetEverything()
    {
        PlayerStats.Instance.Reset();
        EnemyStats.Instance.Reset();
        PlayerMovement.Instance.Reset();
        TimerBehavior.Instance.Reset();
    }
}
