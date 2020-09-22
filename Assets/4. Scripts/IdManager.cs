using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdManager : MonoBehaviour
{
    #region Attributes

    #region Singleton

    private static IdManager _instance;

    public static IdManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Nessuna Istanza di IdManager");
            }

            return _instance;
        }
    }

    #endregion


    #region Proprietà

    private static int _id;

    public static int ID
    {
        get
        {
            return _id;
        }

        private set
        {
            _id = value;
        }
    }

    #endregion

    #endregion

    private void Awake()
    {
        _instance = this;
    }

    public int CreateID()
    {
        ID += 1;

        return ID;
    }

}
