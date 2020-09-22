using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    #region Attributes

    #region Campi Serializzati

    #region Esplosione 
    //esplosioni 
    //usare per effetto esplosione player
    [SerializeField]
    private ParticleSystem _playerExplosion1;
    [SerializeField]
    private ParticleSystem _playerExplosion2;
    #endregion

    #region Effetto fumo
    //ParticleSystems
    [SerializeField]
    private ParticleSystem _smoke; //usare per effetto fumo e luci

    #endregion

    #region Effetto detriti
    [SerializeField]
    private ParticleSystem _debris1, _debris2, _debris3; //effetto detriti su esplosione

    private bool _debrisCounter1, _debrisCounter2, _debrisCounter3;

    #endregion

    #region Luci
    [SerializeField]
    private GameObject _ufoLight;
    [SerializeField]
    private GameObject _ufoLight1;
    [SerializeField]
    private GameObject _ufoLight2;
    [SerializeField]
    private GameObject _ufoLight3;
    #endregion

    #region Scudo
    //Scudo
    [SerializeField]
    private GameObject _shield;
    #endregion

    #region Sostituzione immagini Player
    [SerializeField]
    private Sprite _body100;
    [SerializeField]
    private Sprite _body80;
    [SerializeField]
    private Sprite _body60;
    [SerializeField]
    private Sprite _body40;
    [SerializeField]
    private Sprite _body20;
    [SerializeField]
    private Sprite _bodyDead;

    #endregion

    #region Gestione Fisica
    private CircleCollider2D _ccPlayer2D;
    #endregion

    #endregion

    private string _playerDeathSound = "PlayerExplosion";
    private ParticleSystem _shieldIntensity;
    private SpriteRenderer _playerSpriteRenderer;
    #endregion

    private void Awake()
    {
        #region Gestion eventi

        WildUfoEvents.boolHit.AddListener(Debris);
        WildUfoEvents.boolHit.AddListener(PlayerDead);
        WildUfoEvents.playerStats.AddListener(Damage);
        //WildUfoEvents.playerStats.AddListener(Smoke);


        #endregion
    }

    void Start()
    {
        _smoke.Stop();
        _shield.SetActive(PlayerStats.ShieldActivated); // rivedere la gestione
        _shieldIntensity = _shield.GetComponentInChildren<ParticleSystem>();
        _ccPlayer2D = gameObject.GetComponent<CircleCollider2D>();
        _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _shield.SetActive(PlayerStats.ShieldActivated); //rivedere la gestione
        if (!GameManager.GameIsOver)
            ShiedlIntensity();
    }



    #region Metodi a eventi
    void Debris(BoolEventData boolData)
    {
        // non controllo il valore perchè lo invio dal caller solamente quando il valore è true
        if (boolData.currentEvent == BoolEventData.BoolEvent.debris)
        {
            if (PlayerStats.CurrentHealth < 80)
            {
                if (_debrisCounter1 == false)
                {
                    _debris1.Play();
                    _debrisCounter1 = true;
                }
            }

            if (PlayerStats.CurrentHealth < 60)
            {
                if (_debrisCounter2 == false)
                {
                    _debris2.Play();
                    _debrisCounter2 = true;
                }
            }

            if (PlayerStats.CurrentHealth < 40)
            {
                if (_debrisCounter3 == false)
                {
                    _debris3.Play();
                    _debrisCounter3 = true;
                }
            }
        } 
    }

    void PlayerDead(BoolEventData boolData)
    {
        if (boolData.currentEvent == BoolEventData.BoolEvent.gameIsOver)
        {
            //questo è il caso in cui finisce il tempo
            _smoke.Stop();
            //questo è il caso in cui la salute è zero
            if (PlayerStats.CurrentHealth == 0)
            {
                float randomRange = Random.Range(6f, 12f);
                //CineMachineShake.Instance.ShakeCameraRaw(randomRange, .3f);
                _playerExplosion1.Play();
                _playerExplosion2.Play();
                WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _playerDeathSound, AudioEventData.GameObjectSource.PlayerDeath, false, false));
                _playerSpriteRenderer.sprite = _bodyDead;
            }
            
        }
            
    }

    void Damage(PlayerStatsEventData playerData)
    {
        if (playerData.currentStatsDataType == PlayerStatsEventData.StatsDataType.currentHealth)
        {
            switch (playerData.floatStats)
            {
                case float n when (n < 100 && n >= 80):
                    _smoke.Stop();
                    _playerSpriteRenderer.sprite = _body80;
                    _ufoLight1.SetActive(false);
                    _shield.SetActive(false);
                    _ufoLight2.SetActive(true);
                    _ufoLight3.SetActive(true);
                    _ufoLight.SetActive(true);
                    break;

                case float n when (n < 80 && n >= 60):
                    _smoke.Stop();
                    _playerSpriteRenderer.sprite = _body60;
                    _ufoLight2.SetActive(false);
                    _ufoLight1.SetActive(false);
                    _ufoLight.SetActive(true);
                    _ufoLight3.SetActive(true);
                    break;

                case float n when (n < 60 && n >= 40):
                    _smoke.Play();
                    _playerSpriteRenderer.sprite = _body40;
                    _ufoLight1.SetActive(false);
                    _ufoLight3.SetActive(false);
                    _ufoLight.SetActive(true);
                    break;

                case float n when (n < 40):
                    _smoke.Play();
                    _playerSpriteRenderer.sprite = _body20;
                    _ccPlayer2D.radius = 1f;
                    _ufoLight1.SetActive(false);
                    _ufoLight.SetActive(false);
                    break;

                case float n when (n == 0):
                    _smoke.Stop();
                    _playerExplosion1.Play();
                    _playerExplosion2.Play();
                    _playerSpriteRenderer.sprite = _bodyDead;
                    break;

                default:
                    _smoke.Stop();
                    _playerSpriteRenderer.sprite = _body100;
                    _ufoLight1.SetActive(true);
                    _shield.SetActive(true);
                    _ufoLight2.SetActive(true);
                    _ufoLight3.SetActive(true);
                    _ufoLight.SetActive(true);
                    break;
            }
        }
        
    }


    #endregion



    void ShiedlIntensity()
    {
        if (PlayerStats.ShieldActivated == true)
        {
            var emission = _shieldIntensity.emission;
            emission.rateOverTime = PlayerStats.CurrentShield / 2;
        }
        
    }


}
