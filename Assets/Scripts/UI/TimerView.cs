using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    public event Action Elapsed;
    [SerializeField] private TextMeshProUGUI text;

    private int _timer = 3 * 60;

    public void Init(int timerSeconds)
    {
        _timer = timerSeconds;
    }

    private IEnumerator Start()
    {
        var wait = new WaitForSeconds(1);
        
        var timeSpan = TimeSpan.FromSeconds(_timer);
        var oneSecond = TimeSpan.FromSeconds(1);
        
        while (timeSpan.TotalSeconds > 0)
        {
            text.text = timeSpan.ToString(@"m\:ss");
            timeSpan = timeSpan.Subtract(oneSecond);
            
            yield return wait;
        }
        text.text = "0:00";

        Elapsed?.Invoke();
    }
}
