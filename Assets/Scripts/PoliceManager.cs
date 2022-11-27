using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class PoliceManager : MonoBehaviour
{
    public static PoliceManager Instance;
    public static bool FoundWristwatch = false;
    
    public float ShiftLength = 5 * 60; // В секундах, 5 минуты

    public GameObject Clock;

    public GameObject GamePanel;
    public GameObject Terminal;

    public UnityEngine.Object[] GamePrefabs;

    public bool EventInProgress = false;

    private DateTime _shiftStart;
    private int _lastMinutes = 0;

    private TextMeshProUGUI _clockText;

    private bool _endingShift;

    private int _runningEvents = 0;

    private void Start()
    {
        Instance = this;
        
        _clockText = Clock.GetComponent<TextMeshProUGUI>();
        
        StartShift();        
    }

    public void StartMinigame(int minigameIndex, IEvent callingEvent)
    {
        var game = (GameObject) Instantiate(GamePrefabs[minigameIndex], GamePanel.transform);
        game.GetComponent<Minigame>().CallingEvent = callingEvent;
        Terminal.SetActive(true);
    }

    public void EndMinigame()
    {
        Terminal.SetActive(false);
    }

    public void AddEvent()
    {
        _runningEvents++;
    }

    public void EndEvent()
    {
        if (_runningEvents <= 0)
            throw new Exception("Ивентов высвобождено больше, чем их было!");
        
        _runningEvents--;
    }

    public bool HasFreeEvents()
    {
        return _runningEvents < 5;
    }

    public void StartShift()
    {
        _shiftStart = DateTime.UtcNow - TimeSpan.FromSeconds(ShiftLength * (5f / 24f));
        _endingShift = false;
    }

    private void Update()
    {
        if (_endingShift)
            return;
        
        var time = (float) (DateTime.UtcNow - _shiftStart).TotalSeconds;
        int minutes = (int) (time / ShiftLength * (24 * 60));

        if (minutes != _lastMinutes)
        {
            _lastMinutes = minutes % (24 * 60);

            UpdateClock();
            
            if (minutes >= 23 * 60 + 30) // Конец смены в 23:30
            {
                _endingShift = true;
                Debug.LogWarning("Конец смены!");
                StartCoroutine(EndShift());
                return;
            }
        }
    }

    private void UpdateClock()
    {
        int mins = _lastMinutes % 60;
        int hours = _lastMinutes / 60;
        string suffix = "AM";
        if (hours > 12)
        {
            hours -= 12;
            suffix = "PM";
        }

        _clockText.text = $"{hours:00}:{mins:00}{suffix}";
    }

    private IEnumerator EndShift()
    {
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("Corkboard");
    }
}
