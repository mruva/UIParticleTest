using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Attributes

    #region Campi privati
    private static int _credits; //crediti raccolti 
    private static int _totalLives; //vite totali 
    private static float _currentHealth; //salute attuale del giocatore
    private static float _maxHealth; //salute totale massima del giocatore
    private static float _deltaHealth; //differenza tra max e current health
    private static float _currentShield; //scudo attuale player
    private static float _maxShield; //scudo totale player
    private static float _deltaShield; //differenza tra max e current shield
    private static float _velocity;
    private static bool _shieldActivated;
    #endregion

    #region Proprietà
    public static int Credits //crediti raccolti 
    {
        get
        {
            return _credits;
        }

        private set
        {
            _credits = value;
        }
    }
         
    public static int TotalLives //vite totali 
    {
        get
        {
            return _totalLives;
        }

        private set
        {
            _totalLives = value;
        }
    }
    public static float CurrentHealth //salute attuale del giocatore
    {
        get
        {
            return _currentHealth;
        }

        private set
        {
            _currentHealth = value;
            //Debug.Log("CurrentHealth" + CurrentHealth.ToString());
            WildUfoEvents.playerStats.Invoke(new PlayerStatsEventData(0, CurrentHealth, PlayerStatsEventData.StatsDataType.currentHealth));
        }
    }
    public static float MaxHealth //salute totale massima del giocatore
    {
        get
        {
            return _maxHealth;
        }

        private set
        {
            _maxHealth = value;
        }
    }
    public static float CurrentShield //scudo attuale player
    {
        get
        {
            return _currentShield;
        }

        private set
        {
            _currentShield = value;
            //Debug.Log("CurrentShield" + CurrentShield.ToString());
            WildUfoEvents.playerStats.Invoke(new PlayerStatsEventData(0, _currentShield, PlayerStatsEventData.StatsDataType.currentShield));
        }
    }
    public static float MaxShield
    {
        get
        {
            return _maxShield;
        }

        private set
        {
            _maxShield = value;
        }
    }//scudo totale player

    public static float DeltaHealth
    {
        get
        {
            return (MaxHealth - CurrentHealth);
        }

    }

    public static float DeltaShield
    {
        get
        {
            return (MaxShield - CurrentShield);
        }

    }

    public bool PlayerDead { get; private set; }

    public static float Velocity
    {
        get
        {
            return _velocity;
        }

        set
        {
            _velocity = value;
        }
    }

    public static bool ShieldActivated
    {
        get
        {
            return _shieldActivated;
        }

        private set
        {
            _shieldActivated = value;
        }
    }
    #endregion

    #region PlayerPrefs
    private string _crediti = "Crediti";
    private string _vite = "Vite";
    private string _saluteAttuale = "Salute";
    private string _saluteMassima = "MaxSalute";
    private string _scudoAttuale = "Scudo";
    private string _scudoMassimo = "MaxScudo";
    #endregion

    private int _speed;


    #endregion

    private void Awake()
    {
        Credits = PlayerPrefs.GetInt(_crediti, 0);
        TotalLives = PlayerPrefs.GetInt(_vite, 1);
        CurrentHealth = PlayerPrefs.GetFloat(_saluteAttuale, 100f);
        MaxHealth = PlayerPrefs.GetFloat(_saluteMassima, 100f);
        CurrentShield = PlayerPrefs.GetFloat(_scudoAttuale, 100f);
        MaxShield = PlayerPrefs.GetFloat(_scudoMassimo, 100f);


        #region Gestione Eventi
        //GESTIONE EVENTI
        WildUfoEvents.boolHit.AddListener(OnShieldActivated);
        WildUfoEvents.entityHit.AddListener(ReceiveBonusMalus);
        #endregion
    }
    void Start()
    {
        ShieldActivated = false;
        CurrentShield = 0;

    }

    #region Metodi per eventi

    void ReceiveBonusMalus(EntityEventData entityData)
    {
        switch (entityData.currentEntityType)
        {
            case EntityEventData.EntityType.bonus:
                TotalLives += entityData.lives;
                Credits += entityData.credits;

                if (entityData.health >= DeltaHealth)
                {
                    CurrentHealth = MaxHealth;

                }
                if (entityData.health < DeltaHealth)
                {
                    CurrentHealth += entityData.health;
                }
                if (CurrentHealth > 50)
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(false, BoolEventData.BoolEvent.enemyZone));

                break;

            case EntityEventData.EntityType.malus:
                TotalLives -= entityData.lives;
                Credits -= entityData.credits;
                Debug.Log("Il danno è " + entityData.health.ToString());
                if (entityData.health <= CurrentShield)
                {
                    CurrentShield -= entityData.health;
                }
                else if (entityData.health > CurrentShield)
                {
                    float deltaShield = entityData.health - CurrentShield;
                    CurrentShield = 0;
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(false, BoolEventData.BoolEvent.shieldActivated));
                    if (deltaShield < CurrentHealth)
                    {
                        CurrentHealth -= deltaShield;
                        // va bene per le bombe ma non con le trappole a contatto continuo perchp questo metodo viene chiamato ripetutamente
                        WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.debris)); 
                    }
                    else if (deltaShield >= CurrentHealth)
                    {
                        CurrentHealth = 0;
                        PlayerDead = true;
                    }
                }
                if (CurrentHealth < 60)
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.enemyZone));
                if (CurrentHealth == 0)
                {
                    if (!GameManager.GameIsOver)
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.gameIsOver));
                }
                    
                break;

            default:
                //Debug.LogError("Errore nessun caso di statistica giocatore gestita");
                break;
        }

    }

    private void OnShieldActivated(BoolEventData boolData)
    {
        if (boolData.currentEvent == BoolEventData.BoolEvent.shieldActivated)
        {
            ShieldActivated = boolData.boolEvent;
            if (ShieldActivated == true)
            {
                CurrentShield = 100;
                CurrentHealth = 100;
                WildUfoEvents.boolHit.Invoke(new BoolEventData(false, BoolEventData.BoolEvent.enemyZone));
            }
        }
            
    }

    #endregion

}
