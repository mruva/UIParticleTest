using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpAnim : MonoBehaviour
{
    private Animator anim;
    [SerializeField]
    private ParticleSystem _ps;

    [SerializeField]
    private GameObject _light;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //_light.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        SetLight();

    }

    void SetLight()
    {
        if (_ps.isPlaying == true)
        {
            anim.SetBool("IsPlaying", true);
        } else if (_ps.isPlaying == false)
        {
            anim.SetBool("HasPlayed", true);
        }
    }
}
