using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    #region Attributes

    

    #region Campi Serializzati

    [SerializeField]
    private float _startingTime = 5f;
    [SerializeField] 
    private TextMeshProUGUI countdownTextTMP;

    #endregion

    #region Campi privati e proprietà

    private static float _currentTime = 0f;

    public static float CurrentTime
    {
        get
        {
            return _currentTime;
        }

        set
        {
            _currentTime = value;
        }
    }
    #endregion

    #endregion

    void Start()
    {
        CurrentTime = _startingTime;
    }

    void Update()
    {

        if (!GameManager.LevelWon && !GameManager.GameIsOver)
        {
            CurrentTime -= 1 * Time.deltaTime;
        }
        
        //countdownText.text = currentTime.ToString("0");
        countdownTextTMP.SetText(_currentTime.ToString("0"));

        if (CurrentTime <= 0)
        {
            CurrentTime = 0;
            if (!GameManager.GameIsOver)
            WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.gameIsOver));
        }
    }
}
