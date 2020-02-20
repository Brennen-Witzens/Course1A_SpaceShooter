using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    private float _repeatHeight;
    private Vector3 _startPos;
    


    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _repeatHeight = GetComponent<BoxCollider2D>().size.y / 2;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < _startPos.y - _repeatHeight - 7f)
        {
            transform.position = _startPos;
        }

    }


    



}
