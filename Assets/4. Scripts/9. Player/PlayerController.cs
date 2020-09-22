using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
//using Minimap;
using System.Diagnostics.Tracing;

public class PlayerController : MonoBehaviour
{
    #region Dichiarazione variabili

    #region Campi Serializzati
    [SerializeField]
    private float _speed;
    /*[SerializeField]
    private Joystick _joystick;*/
    [SerializeField]
    private ParticleSystem _engineFX1;
    #endregion

    #region Altre variabili private
    private Rigidbody2D _rb2d;
    private UnityEngine.Object _bulletRef;
    //private AudioListener _audioListener;
    private bool _isMobile;
    private bool _showMap;
    

    #endregion

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Gestione eventi

        //WildUfoEvents.boolHit.AddListener(OnLevelEnded);

        #endregion

        #region Gestione bullet

        _bulletRef = Resources.Load("Sprites/testBullet");

        #endregion

        _rb2d = GetComponent<Rigidbody2D>();
        //_audioListener = GetComponent<AudioListener>();
        _showMap = true;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            #region Invoco l'evento Sparo
            if (GameManager.GameIsOver == false)
            WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.playerShoot));
            #endregion 

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            #region Invoco l'evento Missile
            if (GameManager.GameIsOver == false)
                WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.playerRocket));
            #endregion 
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //aggiungere se gioco non e' gameover o vinto
        {
            if (GameManager.GameIsOver == false)
            {
                if (GameManager.GameIsPaused)
                {
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(false, BoolEventData.BoolEvent.gameIsPaused));
                }
                else if (!GameManager.GameIsPaused)
                {
                    WildUfoEvents.boolHit.Invoke(new BoolEventData(true, BoolEventData.BoolEvent.gameIsPaused));
                }
            }
                
        }
    }

    void FixedUpdate()
    {


        //float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveHorizontal = 0f; 
        float moveVertical = Input.GetAxisRaw("Vertical");
        float rotation = Input.GetAxisRaw("Horizontal");

        //float moveHorizontalMobile = _joystick.Horizontal;
        //float moveVerticalMobile = _joystick.Vertical;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        
        if (moveVertical == 1)
            _engineFX1.Play();
        else if (moveVertical < 1)
            _engineFX1.Stop();
        
        //se il gioco non e finito applico la forza per muovere il giocatore 
        //if (GameManager.GameIsOver == false && GameManager.LevelWon == false)
        if (GameManager.GameIsOver != true ^ GameManager.LevelWon == true)
        {
            //Minimap();
            //ZoomMinimap();
            transform.Rotate(0f, 0f, -rotation);
            //_rb2d.AddForce(movement * _speed);
            //_rb2d.velocity = new Vector2(moveHorizontal, moveVertical);
            _rb2d.AddRelativeForce(movement * _speed);

            //PlayerStats.Velocity = _rb2d.velocity.;
        }
        else //altrimenti blocco il giocatore
        {
            _rb2d.velocity = new Vector2 (0,0);
            //_rb2d.rotation = 0f;
        }
    }

    //orrida gestione ma per il momento vediamo se funziona
    void OnLevelEnded(BoolEventData boolData)
    {
        if (boolData.currentEvent == BoolEventData.BoolEvent.endLevel)
        {
            if (boolData.boolEvent == true)
            {
                _rb2d.velocity = new Vector2(0, 0);
                _rb2d.rotation = 0f;
            }
        }
    }
    /*
    void Minimap()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_showMap == false)
            {
                MinimapClass.ShowWindow();
                _showMap = true;
            } else
            {
                MinimapClass.HideWindow();
                _showMap = false;
            }
        }
    }
    */
    /*
    void ZoomMinimap()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            MinimapClass.ZoomIn();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            MinimapClass.ZoomOut();
        }
    }
    */

    void Shoot()
    {
        //funzione per sparare
        GameObject bullet = (GameObject)Instantiate(_bulletRef);
        bullet.transform.position = gameObject.transform.position;
        bullet.transform.rotation = gameObject.transform.rotation;
    }


}
