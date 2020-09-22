using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI.Extensions;

public class GUIManager : MonoSingleton<GUIManager>
{
    #region Attributes

    #region GUI
    //UI elements
    [SerializeField]
    private GameObject _gameOverUI;
    [SerializeField]
    private GameObject _gameOverNoLife;
    [SerializeField]
    private GameObject _levelCompletedUI;
    //carico e mostro il livello che si sta giocando
    [SerializeField]
    private TextMeshProUGUI _levelTextnumberTMP;
    //public Text punteggioText;
    [SerializeField]
    private TextMeshProUGUI _punteggioTextTMP;
    //highscore variable
    [SerializeField]
    private TextMeshProUGUI _highScorePoints;
    [SerializeField]
    private TextMeshProUGUI _healthValueTextTMP;
    [SerializeField]
    private TextMeshProUGUI _shieldValueTextTMP;
    [SerializeField]
    //come GameObject perchè UIParticleSystem eredita le impostazioni di ParticleSystem senza però 
    //poterle gestire quindi possiamo solamente attivare o disattivare il GameObject
    private UIParticleSystem _pointsFX1;
    [SerializeField]
    private UIParticleSystem _pointsFX2;



    private string _finalScreen = "FinalScreen";

    #endregion

    #region Player Health

    //public HealthBar healthBar; 
    //public ShieldBar shieldBar; 

    #endregion

    #region Fader
    [SerializeField]
    //private SceneFader _sceneFader;

    #endregion

    #endregion

    private void Awake()
    {
        #region Gestione eventi
        WildUfoEvents.boolHit.AddListener(GameScreenBehaviour);
        WildUfoEvents.playerStats.AddListener(PlayerStatsGUI);
        WildUfoEvents.entityHit.AddListener(PointsCollected);
        #endregion

        //I stop the Particle System asap because otherwise it flows in loop
        _pointsFX1.StopParticleEmission();
        _pointsFX2.StopParticleEmission();
    }

    void Start()
    {


        #region Inizializzazione GUI

        _highScorePoints.SetText("");
        SetLevelText();

        #endregion 
    }


    void Update()
    {
        SetCountText();
        
    }

    #region Gestione metodi di eventi
    void GameScreenBehaviour(BoolEventData booldata)
    {
        switch (booldata.currentEvent)
        {
            case BoolEventData.BoolEvent.gameIsOver:
                StartCoroutine(SetGameOverUI());
                //_gameOverUI.SetActive(true);
                break;
            case BoolEventData.BoolEvent.endLevel:
                StartCoroutine(LevelWonRoutine());
                break;
        }
    }

    void PlayerStatsGUI(PlayerStatsEventData playerStats)
    {
        if (playerStats.currentStatsDataType == PlayerStatsEventData.StatsDataType.currentHealth)
        {
            float healthValue = playerStats.floatStats;
            //Debug.Log("Value PlayerStats CurrentHealth è" + healthValue.ToString());
            _healthValueTextTMP.text = ((int)healthValue).ToString();
        }
        if (playerStats.currentStatsDataType == PlayerStatsEventData.StatsDataType.currentShield)
        {
            float shieldValue = playerStats.floatStats;
            //Debug.Log("Value PlayerStats CurrentShield è" + shieldValue.ToString());
            _shieldValueTextTMP.text = ((int)shieldValue).ToString();
        }
    }

    #endregion

    IEnumerator SetGameOverUI()
    {
        //ricordarsi di applicare la stessa logica nella coroutine gameOver del GameManager
        if (PlayerStats.CurrentHealth == 0)
        {
            yield return new WaitForSeconds(1.5f);
        }
        else if (PlayerStats.CurrentHealth > 0)
        {
            yield return new WaitForSeconds(.5f);
        }
        _gameOverUI.SetActive(true);
    }

    void SetLevelText()
    {
        _levelTextnumberTMP.SetText(GameManager.LevelPlayed.ToString());
    }

    void SetCountText()
    {
        _punteggioTextTMP.text = "Scores: " + GameManager.Points.ToString();
    }

    IEnumerator LevelWonRoutine() //integrare schermata dell ultimo livello
    {
        LoadHighscore();
        yield return new WaitForSeconds(1.2f);

        if (GameManager.LastLevel == false)
        {
            _levelCompletedUI.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            //_sceneFader.FadeTo(_finalScreen);
        }

    }

    private void LoadHighscore()
    {
        float pointsToText = PlayerPrefs.GetFloat(GameManager.LevelPlayed.ToString());
        _highScorePoints.SetText(pointsToText.ToString());
    }

    void PointsCollected(EntityEventData entityData)
    {
        if (entityData.currentEntityType == EntityEventData.EntityType.bonus)
        {
            if (entityData.points > 0)
            {
                //particle system FX
                _pointsFX1.StartParticleEmission();
                _pointsFX2.StartParticleEmission();
                //_pointsFX1.StopParticleEmission();
                StartCoroutine(FXduration());
            }
                
        }
        
    }

    IEnumerator FXduration()
    {
        yield return new WaitForSeconds(.5f);
        _pointsFX1.PauseParticleEmission();
        _pointsFX2.PauseParticleEmission();
    }
}
