using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private float _speedMultiplier = 2f;
    private float _yBounds = 0f;
    private float _lowerBounds = 3.8f;
    private float _xBounds = 12f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;

    private Vector3 _offsetLaser = new Vector3(0f, 1.08f, 0f);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;


    //variable reference to shield visualizer
    [SerializeField]
    private GameObject _shieldIcon;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;


    [SerializeField]
    private GameObject _rightEngine, _leftEngine;


    [SerializeField]
    private AudioClip _laserClip;
    private AudioSource _audio;
    private AudioSource _explosionSource;

    // Start is called before the first frame update
    void Start()
    {
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);

        _audio = GetComponent<AudioSource>();
        _explosionSource = GameObject.Find("Explosion_Clip").GetComponent<AudioSource>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        transform.position = new Vector3(0, 0, 0);

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_audio == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
        else
        {
            _audio.clip = _laserClip;
        }

        if (_explosionSource == null)
        {
            Debug.LogError("ExplosionSource is NULL");
        }



    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }


    }


    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= _yBounds)
        {
            transform.position = new Vector3(transform.position.x, _yBounds, transform.position.z);
        }
        else if (transform.position.y <= -_lowerBounds)
        {
            transform.position = new Vector3(transform.position.x, -_lowerBounds, transform.position.z);
        }

        if (transform.position.x >= _xBounds)
        {
            transform.position = new Vector3(-_xBounds, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -_xBounds)
        {
            transform.position = new Vector3(_xBounds, transform.position.y, transform.position.z);
        }

    }

    void FireLaser()
    {
        if (_isTripleShotActive)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);
        }

        _audio.Play();
    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldIcon.SetActive(false);
            return;
        }

        _lives -= 1;


        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();


            _explosionSource.Play();

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostActiveRoutine());

    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldIcon.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Enemy Fire"))
        {
            Damage();

        }

    }


    IEnumerator SpeedBoostActiveRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }


    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    //method to add 10 to score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.CheckScore(_score);

    }

}
