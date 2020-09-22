using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.GameObject _player;

    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _offset = transform.position - _player.transform.position;
    }

    private void Update()
    {
        if (GameManager.GameIsOver) //se il gioco e finito disabilito la camera
        {
            this.enabled = false;
            return;
        }

        //anche se il livello e' completo disabilito la camera
        if (GameManager.LevelWon)
        {
            this.enabled = false;
            return;
        }
        _offset = transform.position - _player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _player.transform.position + _offset;
        //prova per sistemare problema che il giocatore va fuori dallo schermo nella minimap
        //_offset = transform.position - _player.transform.position;
    }
}
