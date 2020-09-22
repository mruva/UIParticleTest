using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret01Bullet : MonoBehaviour
{
    #region Attributes

    #region Campi Serializzati

    [SerializeField]
    private float _speed = 500;
    //[SerializeField]
    //private Rigidbody2D _rbLaserBullet;

    private Rigidbody2D _rbBullet;

    #endregion

    #endregion

    private void OnEnable()
    {
        
    }

    void Start()
    {
        transform.position = GetComponentInParent<Turret01Mk1Pool>().transform.position;
        _rbBullet = gameObject.GetComponent<Rigidbody2D>();
        _rbBullet.velocity = transform.up * _speed;

        //_rbLaserBullet.velocity = transform.up * _speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter" + collision.tag);

    }

    void Hit()
    {
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
