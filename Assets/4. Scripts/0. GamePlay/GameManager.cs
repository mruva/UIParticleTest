using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
//using Minimap;

public class GameManager : MonoBehaviour
{
    #region Dichiarazione variabili

    #region Variabili Private Statiche

    #region Controllori Musica ed Effetti Sonori

    private static bool _gameIsOver;
    private static bool _enemiesAround;

    #endregion

    #region Controllori Livello

    private static bool _levelIsWon;
    private static bool _levelEnded;
    private static int _levelPlayed;
    private static bool _lastLevel;

    #endregion

    #region Controllori GamePlay

    private static bool _gameIsPaused;

    #endregion

    #region Controllori Punteggio
    private static int _points; //punti giocatore (valutare se spostare in PlayerStats)
    private static float _extraPoints; //extra punti dati dal tempo restante (valutare se spostare in PlayerStats)
    #endregion
    
    #endregion

    #region Proprietà Pubbliche Statiche
    public static bool GameIsOver
    {
        get
        {
            return _gameIsOver;
        }

        private set
        {
            _gameIsOver = value;
        }
    }

    public static bool LevelWon
    {
        get
        {
            return _levelIsWon;
        }

        private set
        {
            _levelIsWon = value;
        }

    }

    public static bool LevelEnded
    {
        get
        {
            return _levelEnded;
        }

        private set
        {
            _levelEnded = value;
        }
    }

    public static bool EnemiesAround
    {
        get
        {
            return _enemiesAround;
        }

        private set
        {
            _enemiesAround = value;
        }
    }

    public static int Points
    {
        get
        {
            return _points;
        }

        private set
        {
            _points = value;
        }
    }

    public static float ExtraPoints
    {
        get
        {
            return _extraPoints;
        }

        private set
        {
            _extraPoints = value;
        }
    }

    
    public static bool GameIsPaused
    {
        get
        {
            return _gameIsPaused;
        }

        private set
        {
            _gameIsPaused = value;
        }
    }

    public static int LevelPlayed
    {
        get
        {
            return _levelPlayed;
        }

        private set
        {
            _levelPlayed = value;
        }

    }

    public static bool LastLevel
    {
        get
        {
            return _lastLevel;
        }

        private set
        {
            _lastLevel = value;
        }
    }

    #endregion

    #region Variabile Private

    #region Variabili per gestione Livello
    
    private int _levelToReach;
    
    #endregion

    #region Variabili per gestione Punteggio
    private float _actualHighscore;
    private float _punteggioFinale;
    #endregion
    #endregion

    #region Proprietà Pubbliche

    #region Proprietà per gestione livello
    

    public int LevelToReach
    {
        get
        {
            return _levelToReach;
        }

        private set
        {
            _levelToReach = value;
        }
    }


    #endregion

    #region Proprietà per gestione Punteggio

    public float ActualHighScore
    {
        get
        {
            return _actualHighscore;
        }

        private set
        {
            _actualHighscore = value;
        }
    }

    public float PunteggioFinale
    {
        get
        {
            return _punteggioFinale;
        }

        private set
        {
            _punteggioFinale = value;
        }
    }

    #endregion

    #endregion

    #region Audio
    // Riferimenti File Audio
    private string _gameOverSound = "GameOverSound"; // probabilmente da spostare nell'Audio Manager
    private string _gamePlayMusic = "Musica"; // probabilmente da spostare nell'Audio Manager
    private string _enemyMusic = "MusicEnemy"; // probabilmente da spostare nell'Audio Manager
    //private string _ambientMusic; // probabilmente da spostare nell'Audio Manager

    #endregion

    #region Fader
    //[SerializeField]
    //private SceneFader _sceneFader;
   
    #endregion

    #region Livelli
    [SerializeField]
    private int _totalLevels;
    #endregion

    #region Punteggio
 
    
    #endregion

    #region GUI

    private string _finalScreen = "FinalScreen";
    #endregion

    #region World Objects
    //credits variables
    private GameObject[] _coins;
    #endregion

    #endregion


    private void Awake()
    {
        #region Gestione Eventi
        //GESTIONE EVENTI
        WildUfoEvents.enemyHit.AddListener(TrapHit);
        WildUfoEvents.entityHit.AddListener(BonusHit);
        WildUfoEvents.boolHit.AddListener(BoolHit);
        #endregion

        #region Gestione Minimap
        //Inizializzo la minimap
        //MinimapClass.Init();
        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        #region Inizializzazione Musiche GamePlay
        WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _gamePlayMusic, AudioEventData.GameObjectSource.Music, true, false));
        WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _enemyMusic, AudioEventData.GameObjectSource.Music, true, false));
        #endregion

        #region Inizializzazione Proprietà
        GameIsOver = false;
        LevelWon = false;
        LevelEnded = false;
        EnemiesAround = false;
        Points = 0;
        PunteggioFinale = 0;
        #endregion

        _coins = GameObject.FindGameObjectsWithTag("PickUp");

        //carico il contatore del livello che sto giocando
        LevelPlayed = SceneManager.GetActiveScene().buildIndex - 1;
 
        //carico il livello massimo sbloccato
        LevelToReach = PlayerPrefs.GetInt("levelReached", LevelPlayed);

        ActualHighScore = PlayerPrefs.GetFloat(_levelPlayed.ToString());
         //spostare nel GUI MANAGER
        CheckEndLevel();
    }


    // Update is called once per frame
    void Update()
    {

    }

    // GESTIONE EVENTI

    #region Eventi
    void TrapHit(HitEventData hit)
    {
        //aggiornare il livello di salute del giocatore, 
        //quindi aggiornare il GUI manager
        //Debug.Log(hit.victim.ToString());
    }

    void BonusHit(EntityEventData entityData)
    {
        Points += entityData.points;
    }

    void BoolHit(BoolEventData boolData)
    {
        switch (boolData.currentEvent)
        {
            case BoolEventData.BoolEvent.endLevel:
                LevelEnded = boolData.boolEvent;
                CheckLevelHighScore();
                break;
            case BoolEventData.BoolEvent.playerDead:
                GameIsOver = boolData.boolEvent;
                StartCoroutine(GameOverSound());
                //WildUfoEvents.audioEvent.Invoke(new AudioEventData(gameObject.transform.position, _gameOverSound, AudioEventData.GameObjectSource.GameOver, false, false));
                //EndGame();
                break;
            case BoolEventData.BoolEvent.gameIsPaused:
                GameIsPaused = boolData.boolEvent;
                break;
            case BoolEventData.BoolEvent.levelWon:
                LevelWon = boolData.boolEvent;
                break;
            case BoolEventData.BoolEvent.gameIsOver:
                GameIsOver = boolData.boolEvent;
                StartCoroutine(GameOverSound());
                //WildUfoEvents.audioEvent.Invoke(new AudioEventData(gameObject.transform.position, _gameOverSound, AudioEventData.GameObjectSource.GameOver, false, false));
                break;
            default:
                //Debug.LogError("Errore nessun caso di evento bool gestito");
                break;
        }
   
    }

    #endregion

    IEnumerator GameOverSound()
    {
        if (PlayerStats.CurrentHealth == 0)
        {
            yield return new WaitForSeconds(1.5f);
        }
        else if (PlayerStats.CurrentHealth > 0)
        {
            yield return new WaitForSeconds(.5f);
        }
        WildUfoEvents.audioEvent.Invoke(new AudioEventData(gameObject.transform.position, _gameOverSound, AudioEventData.GameObjectSource.GameOver, false, false));
    }

    void CheckExtraPoints()
    {
        if (GameIsOver)
        {
            ExtraPoints = Timer.CurrentTime;
        }
    }

    void UpdateLevelPlayed()
    {
        //mando avanti il livello sbloccato solamente se il livello che sto giocando e' uguale al massimo livello sbloccato
        if (LevelPlayed == LevelToReach)
        {
            LevelToReach++;
            PlayerPrefs.SetInt("levelReached", LevelToReach);
        }
    }

    void CheckLevelHighScore()
    {
        if (Points > ActualHighScore)
        {
            PlayerPrefs.SetFloat(LevelPlayed.ToString(), Points);
        }

    }

    void CheckEndLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 == _totalLevels)
        {
            LastLevel = true;

        } else
        {
            LastLevel = false;
        }
    }
}
