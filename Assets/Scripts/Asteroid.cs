using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private GameObject _explosionPrefab;

    private SpawnManager _spawnManager;


    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GameObject.Find("Explosion_Clip").GetComponent<AudioSource>();

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();


        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager was NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL");
        }


    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();

            _audioSource.Play();

            Destroy(this.gameObject, 0.25f);

        }

    }
}
