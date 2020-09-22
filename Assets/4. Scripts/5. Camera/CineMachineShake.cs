using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Cinemachine;

/*
public class CineMachineShake : MonoBehaviour
{
    #region Attributes

    #region Campi Privati

    private CinemachineVirtualCamera _cineVC;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    #endregion

    #region Singleton

    public static CineMachineShake Instance { get; private set; }

    #endregion

    #endregion


    private void Awake()
    {
        Instance = this;
        _cineVC = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCameraRaw (float intensity, float time)
    {

        _startingIntensity = intensity;
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        
        _shakeTimer = time;
        _shakeTimerTotal = time;

    }

    /*
    public void ShakeCameraSmoothly(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }
    */
/*
    private void Update()
    {
        if (_shakeTimer > 0f)
        {
            _shakeTimer -= Time.deltaTime;
            //Tempo finito
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            //cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, (1 - (_shakeTimer / _shakeTimerTotal)));
        }
    }

}
*/