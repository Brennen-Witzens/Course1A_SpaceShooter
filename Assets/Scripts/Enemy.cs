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
    private bool _newMovemnet = false;
    
    [SerializeField]
    private float _randomValue;
    [SerializeField]
    private bool _isEnemyShielded = false;

    private Player _player;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldIcon;
    [SerializeField]
    private GameObject _negativePickup;

    private Animator _enemyAnimator;


    private AudioSource _explosionSource;

    private void OnEnable()
    {
        _randomValue = Random.value;
        _shieldIcon.SetActive(false);
        EnemyShields();
    }


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


        PowerUpCheck();

    }

    private void PowerUpCheck()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);

        if (hit.collider != null)
        {

            Debug.DrawRay(transform.position, Vector2.down * 2, Color.red); //Shows what the ray would look like.

            if (hit.collider.CompareTag("PowerUp"))
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0.2f, -1f, 0), Quaternion.Euler(0, 0, 180));
            }
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


        if (_randomValue > 0.65)
        {
            _newMovemnet = true;
            if (_newMovemnet)
            {
                transform.Translate(Mathf.Cos(Time.time) * 0.09f,( -1 * _speed * Time.deltaTime), 0);
            }
        }


    }

    private void EnemyShields()
    {
        if(_randomValue > 0.8)
        {
            _isEnemyShielded = true;
            if (_isEnemyShielded)
            {
                _shieldIcon.SetActive(true);
                return;
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isEnemyShielded)
            {
                _isEnemyShielded = false;
                _shieldIcon.SetActive(false);

            }

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _fireRate = 0;

            _explosionSource.Play();
            OnDeath();
            Destroy(this.gameObject, 2.6f);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            if (_isEnemyShielded)
            {
                _isEnemyShielded = false;
                _shieldIcon.SetActive(false);
                return;
            }


            if (_player != null)
            {
                _player.AddScore(10);
                this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
                _fireRate = 0;
            }
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _explosionSource.Play();
            OnDeath();
            Destroy(this.gameObject, 2.6f);
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Magnet"))
        {
            transform.position = Vector3.MoveTowards(transform.position, other.transform.position, 2.5f * Time.deltaTime);
        }
    }

    private void OnDeath()
    { 
        Instantiate(_negativePickup, transform.position, Quaternion.identity);
    }


}
