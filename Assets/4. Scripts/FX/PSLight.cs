using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSLight : MonoBehaviour
{
    [SerializeField]
    private GameObject _light;

    private void Awake()
    {
        _light.SetActive(true);
    }

    public void TurnOff()
    {
        StartCoroutine(TurnOffLight());
    }

    IEnumerator TurnOffLight()
    {
        _light.SetActive(false);

        yield return new WaitForSeconds(1f);
    }
}
