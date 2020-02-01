using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private float _lowerScreenBound = 5.5f;
    private float _xBounds = 9f;
    private float _randomXRange;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;


    private Player _player;

    [SerializeField]
    private GameObject _laserPrefab;

    private Animator _enemyAnimator;


    private AudioSource _explosionSource;

    private void Start()
    {

        _explosionSource = GameObject.Find("Explosion_Clip").GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player was NULL");
        }

        if (_explosionSource == null)
        {
            Debug.LogError("ExplosionSource is NULL");
        }


        _enemyAnimator = GetComponent<Animator>();

        if (_enemyAnimator == null)
        {
            Debug.LogError("Animator is NULL");
        }

    }


    void Update()
    {
        CalculateMovement();

        int _randomLaserFire = Random.Range(3, 8);

        if (_canFire < Time.time)
        {
            _fireRate = _randomLaserFire;
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0.2f, -1f, 0), Quaternion.Euler(0, 0, 180));
        }

    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -_lowerScreenBound)
        {
            _randomXRange = Random.Range(-_xBounds, _xBounds);
            transform.position = new Vector3(_randomXRange, 8, 0);
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _explosionSource.Play();

            Destroy(this.gameObject, 2.6f);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);


            if (_player != null)
            {
                _player.AddScore(10);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _explosionSource.Play();
            Destroy(this.gameObject, 2.6f);
        }

    }


}
