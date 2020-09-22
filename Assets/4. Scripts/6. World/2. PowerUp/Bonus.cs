using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    #region Attributes

    #region Campi serializzati
    [SerializeField]
    private string _soundCollided;
    [SerializeField]
    private ParticleSystem _psFXLight;
    [SerializeField]
    private ParticleSystem _psFX2;
    [SerializeField]
    private int _pointsGiven;
    [SerializeField]
    private int _creditsGiven;
    [SerializeField]
    private float _healthGiven;
    [SerializeField]
    private int _livesGiven;

    #endregion

    #region Proprietà
    public int Points
    {
        get
        {
            return _pointsGiven;
        }

        private set
        {
            _pointsGiven = value;
        }
    }

    public int Credits
    {
        get
        {
            return _creditsGiven;
        }

        private set
        {
            _creditsGiven = value;
        }
    }

    public int Lives
    {
        get
        {
            return _livesGiven;
        }

        private set
        {
            _livesGiven = value;
        }
    }

    public float Health
    {
        get
        {
            return _healthGiven;
        }

        private set
        {
            _healthGiven = value;
        }
    }
    #endregion

    #endregion

    private void Awake()
    {
       
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _soundCollided, AudioEventData.GameObjectSource.CoinCollected, false, true));
            PlayFXLight();
            PlayFX2();
            WildUfoEvents.entityHit.Invoke(new EntityEventData(Points, Credits, Lives, Health, EntityEventData.EntityType.bonus));
            
        }
    }

    
    void PlayFXLight()
    {
        ParticleSystem fx1 = Instantiate(_psFXLight, transform.position, Quaternion.identity);
    }
    

    void PlayFX2()
    {
        ParticleSystem fx2 = Instantiate(_psFX2, transform.position, Quaternion.identity);
        fx2.Play();
    }
}
