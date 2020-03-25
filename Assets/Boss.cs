using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private float _speed = 7f;
    private float _maxHealth = 100f;

    private UIManager _uiManager;

    [SerializeField]
    private float _currentHealth;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _canFire = -1f;
    private float _fireRate = 1.3f;

    private void OnEnable()
    {
        _currentHealth = _maxHealth;
        

    }


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Mathf.Cos(Time.time);

        if (transform.position.y  > 5)
        {    
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            
        }
        
        transform.position = new Vector3(x * _speed, 5, 0);
        





        Shoot();
        
    }

    private void Shoot()
    {
        if (_canFire < Time.time)
        { 
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0.2f, -2f, 0), Quaternion.Euler(0, 0, 180));
        }
        if(_currentHealth <= 50)
        {
            _fireRate = 0.5f;

        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
            Damage(5f);
            _uiManager.UpdateBossHealthBar(5f, _currentHealth, _maxHealth);
            Destroy(other.gameObject);
        }


    }


    private void Damage(float damageValue)
    {
        _currentHealth -= damageValue;

    }

}
