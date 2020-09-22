using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret01_mk1 : MonoBehaviour
{
    #region Attributes

    #region Campi Serializzati

    private string _soundShoot = "Turret01Mk1Shoot";

    private ParticleSystem _LaserShotFlash;

    #endregion

    #endregion

    void Start()
    {

        _LaserShotFlash = gameObject.GetComponentInChildren<ParticleSystem>();

        #region Gestione eventi

        WildUfoEvents.boolHit.AddListener(Shoot);

        #endregion
    }

    #region Metodi Eventi
    void Shoot(BoolEventData boolData)
    {
        if (boolData.currentEvent == BoolEventData.BoolEvent.playerShoot)
        {
            BulletTurret01Mk1 bullet = Turret01Mk1Pool.Instance.GetFromPool();
            WildUfoEvents.audioEvent.Invoke(new AudioEventData(transform.position, _soundShoot, AudioEventData.GameObjectSource.PlayerShoot, false, true));
            _LaserShotFlash.Play();
            bullet.transform.position = Turret01Mk1Pool.Instance.transform.position;
            bullet.gameObject.SetActive(true);
        }

    }

    #endregion 

}
