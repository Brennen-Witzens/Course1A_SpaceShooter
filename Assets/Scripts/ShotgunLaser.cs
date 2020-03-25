using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunLaser : MonoBehaviour
{
    private float _spreadAngle;
    private float _rotateAngle;
  

    private Transform _player;
    private Rigidbody2D _tempRb;
   

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Transform>();
        if(_player == null)
        {
            Debug.LogError("Player Transform is NULL");
        }

        _spreadAngle = Random.Range(-30f, 30f);

        _tempRb = GetComponent<Rigidbody2D>();
        if(_tempRb == null)
        {
            Debug.LogError("Rigidbody is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        var x = transform.position.x;
        var y = transform.position.y;
        _rotateAngle = _spreadAngle + (Mathf.Atan2(y,x) * Mathf.Rad2Deg);

        var _moveDir = new Vector2(Mathf.Cos(_rotateAngle * Mathf.Deg2Rad), Mathf.Sin(_rotateAngle * Mathf.Deg2Rad)).normalized;

        _tempRb.velocity = _moveDir; 
    }
    

}
