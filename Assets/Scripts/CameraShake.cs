using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform _location;

    [SerializeField]
    private float _shakeDur = 0f;
    [SerializeField]
    private float _shakeMagnitude = 0.5f;
    [SerializeField]
    private float _shakeDampening = 1.0f;

    private Vector3 _initial;


    // Start is called before the first frame update
    void Start()
    {
        if(transform == null)
        {
            _location = GetComponent<Transform>();
        }

        _initial = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        if(_shakeDur > 0)
        {
            transform.position = _initial + Random.insideUnitSphere * _shakeMagnitude;

            _shakeDur -= Time.deltaTime * _shakeDampening;
        }
        else
        {
            _shakeDur = 0f;
            transform.position = _initial;
        }
        
    }

    public void Shake()
    {
        _shakeDur = 1.0f;
    }

}
