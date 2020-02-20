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
    [SerializeField]
    private GameObject _shotgunPrefab;

    private Vector3 _offsetLaser = new Vector3(0f, 1.08f, 0f);
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    
    
    private int _shieldHits = 3;

    [SerializeField]
    private int _ammoCount = 15;

    private SpawnManager _spawnManager;
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private bool _isThrusterActive = false;
    private bool _thrusterCooldown = false;
    private bool _isShotGunActive = false;


    //variable reference to shield visualizer
    [SerializeField]
    private GameObject _shieldIcon;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private CameraShake _cameraShake;


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

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if(_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL");
        }

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

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
        {
            FireLaser();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isThrusterActive = true;
            Thrusters();
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ResetThruster();
        }

    }

    private void ResetThruster()
    {
        _isThrusterActive = false;
        _speed = 10f;
    }

    private void Thrusters()
    {
        if(_speed < 30 && _isThrusterActive && !_thrusterCooldown)
        {
            _speed += 2;
            
        }


        if (_isThrusterActive && !_thrusterCooldown)
        {
            _uiManager.UpdateThrusters();

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
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        else if (_isShotGunActive)
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_shotgunPrefab, transform.position + new Vector3(0, 1.25f, 0), Quaternion.identity);
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        else
        {
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + _offsetLaser, Quaternion.identity);
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);

        }

        _audio.Play();
    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _shieldIcon.GetComponent<SpriteRenderer>().color = Color.white;
            switch (_shieldHits)
            {
                case 3:
                    _shieldHits--;
                    _shieldIcon.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case 2:
                    _shieldIcon.GetComponent<SpriteRenderer>().color = Color.red;
                    _shieldHits--;
                    break;
                case 1:
                    _shieldHits--;
                    _isShieldActive = false;
                    _shieldIcon.SetActive(false);
                    _shieldHits = 3;
                    break;
            }

            
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

        _cameraShake.Shake();

    }

    public void ThrusterOnCooldown()
    {
        _isThrusterActive = false;
        _thrusterCooldown = true;
    }

    public void ThrusterCooldownReset()
    {
        _thrusterCooldown = false;
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void ShotGunActive()
    {
        _isShotGunActive = true;
        StartCoroutine(ShotGunPowerDownRoutine());
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

    public void AddAmmo()
    {
        _ammoCount = 15;
        _uiManager.UpdateAmmo(_ammoCount);
    }

    public void Heal()
    {
        _lives++;
        _uiManager.UpdateLives(_lives);

        switch (_lives)
        {
            case 2: //Going 1 to 2 health
                _leftEngine.SetActive(false);
                break;
            case 3: //going 2 to 3 health
                _rightEngine.SetActive(false);
                break;
            default:
                break;
        }



    }


    IEnumerator ShotGunPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isShotGunActive = false;
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
