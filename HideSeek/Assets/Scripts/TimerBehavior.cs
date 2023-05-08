using UnityEngine;
using TMPro;

public class TimerBehavior : MonoBehaviour
{
    public static TimerBehavior Instance;
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
    TextMeshProUGUI timer;
    float currTime = 0f;
    public int CurrSecInt;
    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        CurrSecInt = Mathf.FloorToInt(currTime);

        timer.text = GetFormattedTime(CurrSecInt);
    }

    public string GetFormattedTime(int totalSec)
    {
        int sec = totalSec % 60;
        int min = totalSec / 60;
        return string.Format("{0}:{1}", min.ToString("D2"), sec.ToString("D2"));
    }

    public void Reset()
    {
        currTime = 0f;
    }
}
