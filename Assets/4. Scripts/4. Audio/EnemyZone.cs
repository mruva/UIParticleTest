using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    #region Attributes

    #region Variabili private
    private bool _isInEnemyZone;
    #endregion

    #region Proprietà Pubbliche
    public bool IsInEnemyZone
    {
        get
        {
            return _isInEnemyZone;
        }

        private set
        {
            _isInEnemyZone = value;
            WildUfoEvents.boolHit.Invoke(new BoolEventData(IsInEnemyZone, BoolEventData.BoolEvent.enemyZone));
        }
    }
    #endregion

    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //conversione in proprietà ed eventi riuscita
            IsInEnemyZone = true;
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //conversione in proprietà ed eventi riuscita
            IsInEnemyZone = false;
            
        }
    }

}
