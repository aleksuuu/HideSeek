using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;

public class GUIBehavior : MonoBehaviour
{
    public static GUIBehavior Instance;
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



    [SerializeField] TextMeshProUGUI enemyHearts;
    [SerializeField] TextMeshProUGUI playerHearts;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] TextMeshProUGUI title;


    private int _enemyLives = 3;
    public int EnemyLives
    {
        get => _enemyLives;
        set
        {
            _enemyLives = value;
            enemyHearts.text = GetHeartsString(value, EnemyStats.Instance.TotalLives);
        }
    }
    private int _playerLives = 3;
    public int PlayerLives
    {
        get => _playerLives;
        set
        {
            _playerLives = value;
            playerHearts.text = GetHeartsString(value, PlayerStats.Instance.TotalLives);
        }
    }

    string GetHeartsString(int remaining, int total)
    {
        StringBuilder sb = new("", total);

        for (int i = 0; i < remaining; i++)
        {
            sb.Append('♥');
        }
        for (int i = 0; i < total-remaining; i++)
        {
            sb.Append('♡');
        }
        return sb.ToString();
    }

    public IEnumerator PlayerHeartFlicker()
    {
        string orig = playerHearts.text;
        StringBuilder sb = new(orig);
        sb[orig.Length - 1] = '♡';
        string newString = sb.ToString();
        int flicker = 0;
        while (flicker < 8)
        {
            playerHearts.text = flicker % 2 == 0 ? orig : newString;

            flicker++;

            yield return new WaitForSeconds(EnemyStats.Instance.SecondsBeforeHarmingPlayer / 8);
        }
    }

    public void ShowTitle(bool visibility)
    {
        title.enabled = visibility;
    }

    public void ShowTutorialMessage()
    {
        message.text =
            @"W -> Move Forward
LeftShift + W -> Run
Mouse -> Direction

Press RightShift to spawn an obstacle;
while holding RightShift, use arrow keys to move the obstacle;
release RightShift to place the obstacle
Return -> Launch Fireworks (to destroy an obstacle)

Press W to start
";
    }

    public void ShowPauseMessage()
    {
        message.text = "Paused\n(Press W to continue or esc to exit)";
    }
    //public void ShowWinMessage()
    //{
    //    message.text = "YOU WIN!\n(Press W to restart)";
    //}


    public void ShowLoseMessage()
    {
        message.text = "LOSER\n(Press W to restart)";
    }
    public void ClearMessage()
    {
        message.text = "";
    }

    public void NewRecord(bool wasARecord)
    {
        if (wasARecord)
        {
            message.text = "NEW RECORD!\n(Press W to restart)";
        }
        else
        {
            int record = GameBehavior.Instance.BestTime;
            string formattedRecord = TimerBehavior.Instance.GetFormattedTime(record);
            message.text = "Record: " + formattedRecord + "\n(Press W to restart)";
        }
    }


}
