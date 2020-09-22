using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BulletTurret01Mk1 : MonoBehaviour
{
    #region Attributes

    #region Campi privati
    private Rigidbody2D _rbBullet;
    private string _laserHitSound = "Turret01Mk1LaserHit";
    private float _speed = 500;
    private float _rawDamage; 
    #endregion

    #region Campi Serializzati
    [SerializeField]
    private ParticleSystem _laserHit;
    #endregion

    #endregion


    private void Awake()
    {
        _rbBullet = gameObject.GetComponent<Rigidbody2D>();

    }
    private void OnEnable()
    {
        transform.rotation = Turret01Mk1Pool.Instance.transform.rotation;
        _rbBullet.velocity = transform.up * _speed;
        _rawDamage = Random.Range(5, 25);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("TriggerEnter" + collision.tag);

        if (collision.gameObject.CompareTag("Untagged"))
        {
            return;
        } else if (collision.gameObject.CompareTag("Player"))
        {
            return;
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            BulletExplosion();
            float randomRange = Random.Range(1f, 5f);
            float randomLenght = Random.Range(.1f, .2f);
            //CineMachineShake.Instance.ShakeCameraRaw(randomRange, randomLenght);
            //Enemy enemyCollided = collision.gameObject.GetComponent<Enemy>();
            //WildUfoEvents.enemyHit.Invoke(new HitEventData( enemyCollided.ID , _rawDamage));
        } else
        {
            BulletExplosion();
        }
            
    }

    void BulletExplosion()
    {
        ParticleSystem ps = Instantiate(_laserHit, transform.position, Quaternion.identity); //valutare come sostituire con object pool
            ps.Play();
            _laserHit.Play();
            WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _laserHitSound, AudioEventData.GameObjectSource.LaserHit, false, true));
            Turret01Mk1Pool.Instance.ReturnToPool(this);
    }



}
