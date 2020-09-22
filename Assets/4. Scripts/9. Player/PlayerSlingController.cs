using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlingController : MonoBehaviour
{
    #region Dichiarazione variabili

    private bool _isPressed;
    private float _releaseDelay;
    [SerializeField]
    private float _maxDrag = 2f;
    [SerializeField]
    private float _distance;
    [SerializeField]
    private Rigidbody2D _rb;
    private SpringJoint2D _sj;
    private Rigidbody2D _slingRb;

    //private Vector2 _mousePos;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sj = GetComponent<SpringJoint2D>();
        _slingRb = _sj.connectedBody;

        _releaseDelay = 1 / (_sj.frequency * 4);
    }

    // Start is called before the first frame update
    void Start()
    {
        //_isPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPressed)
        {
            //DragObject();
            DragBall();
            //_rb.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        /*
        //verifico coordinate mouse
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;
        float z = Input.mousePosition.z;

        Debug.Log("Mouse x:" + x);
        Debug.Log("Mouse y:" + y);
        Debug.Log("Mouse z:" + z);

        Vector3 camCoord = new Vector3(x, y, z);


        //verifico passando per la camera
        Vector3 _mouseCamera = Camera.main.ScreenToWorldPoint(camCoord);
        Debug.Log("Coordinate mouseCamera: x:" + Camera.main.ScreenToWorldPoint(camCoord).x + "
        y: " + Camera.main.ScreenToWorldPoint(camCoord).y + " z: " + Camera.main.ScreenToWorldPoint(camCoord).z);
        */

    }

    private void DragBall()
    {
        Vector2 mouseposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(mouseposition, _slingRb.position);

        if (distance > _maxDrag)
        {
            Vector2 direction = (mouseposition - _slingRb.position).normalized;
            _rb.position = _slingRb.position + direction * _maxDrag;

        } else
        {
            _rb.position = mouseposition;
        }
    }

    private void DragObject()
    {
        //preso da https://www.youtube.com/watch?v=VOEtOGmHoeE ma adattato per il 3D con la camera perspective
        Vector3 _mouse3D = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 450f);
        Vector3 _slingPos3D = new Vector3(_slingRb.position.x, _slingRb.position.y, 450f);

        //con la Cinemachine e la camera perspective si deve passare un vector3
        //Vector2 _mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 450f));
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(_mouse3D);

        //float distance = Vector2.Distance(_mousePos, _slingRb.position);

        _distance = Vector2.Distance(_mousePos, _slingRb.position);
        Debug.Log("Distance e': " + _distance.ToString());
        if (_distance > _maxDrag)
        {
            Vector2 direction = (_mousePos - _slingRb.position).normalized;
            Vector2 multiplier = new Vector2(direction.x * _maxDrag, direction.y * _maxDrag);
            Vector3 maxDistDrag = new Vector3((_slingRb.position.x + multiplier.x), (_slingRb.position.y + multiplier.y), 450f);
            Vector2 _rbPos = Camera.main.ScreenToWorldPoint(maxDistDrag);
            _rb.position = _rbPos;
            //_rb.position = new Vector2((_slingRb.position.x + multiplier.x),(_slingRb.position.y + multiplier.y));
            //_rb.position = _slingRb.position + direction * _maxDrag;
        }
        else
        {
            _rb.position = _mousePos;
        }
        

    }

    private void OnMouseDown()
    {
        _isPressed = true;
        _rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        _isPressed = false;
        _rb.isKinematic = false;
        StartCoroutine(Release());
    }

    private IEnumerator Release()
    {
        yield return new WaitForSeconds(_releaseDelay);
        _sj.enabled = false;
    }
}
