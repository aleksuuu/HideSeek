using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;
using UnityEngine.UI;

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
    [SerializeField] Slider boxSlider;
    [SerializeField] Image boxIcon;

    public void UpdateEnemyHearts()
    {
        enemyHearts.text = GetHeartsString(EnemyStats.Instance.RemainingLives, EnemyStats.Instance.TotalLives);
    }
    public void UpdatePlayerHearts()
    {
        playerHearts.text = GetHeartsString(PlayerStats.Instance.RemainingLives, PlayerStats.Instance.TotalLives);
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
        sb[PlayerStats.Instance.RemainingLives - 1] = '♡';
        string newString = sb.ToString();
        int flicker = 0;
        while (flicker < 16)
        {
            playerHearts.text = flicker % 2 == 0 ? orig : newString;

            flicker++;

            Debug.Log("flicker" + flicker);

            yield return new WaitForSeconds(EnemyStats.Instance.SecondsBeforeHarmingPlayer / 16f);
        }
    }

    public void ShowTitle(bool visibility)
    {
        title.enabled = visibility;
    }

    public void ShowTutorialMessage()
    {
        message.text =
            @"W - Move Forward
LeftShift + W - Run
Mouse - Direction

To place an obstacle:
With RightShift held down, move the obstacle with arrow keys;
release RigthShift to place it

Return - Launch Fireworks (to destroy an obstacle)

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
        message.text = "LOSER\n(Press return to restart)";
    }
    public void ClearMessage()
    {
        message.text = "";
    }

    public void NewRecord(bool wasARecord)
    {
        if (wasARecord)
        {
            message.text = "NEW RECORD!\n(Press return to restart)";
        }
        else
        {
            int record = GameBehavior.Instance.BestTime;
            string formattedRecord = TimerBehavior.Instance.GetFormattedTime(record);
            message.text = "Record: " + formattedRecord + "\n(Press return to restart)";
        }
    }

    public void SetBoxSlider(float percentage)
    {
        boxSlider.value = Mathf.Clamp(percentage, 0f, 1f);
    }

    public void MakeBoxIconTransparent()
    {
        boxIcon.color = new(boxIcon.color.r, boxIcon.color.g, boxIcon.color.b, 0.25f);
    }

    public void MakeBoxIconSolid()
    {
        boxIcon.color = new(boxIcon.color.r, boxIcon.color.g, boxIcon.color.b, 1f);
    }


}
