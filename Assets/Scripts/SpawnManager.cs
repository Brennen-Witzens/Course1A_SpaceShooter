using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    
    [SerializeField]
    private GameObject[] _enemiesArray;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private GameObject _newEnemy;
    private Animator _newEnemyAnim;
    private UIManager _uiManager;

    private bool _stopSpawning = false;
    private bool _hasSpawned = false;

    [SerializeField]
    private float _randomValue;

    [SerializeField]
    private int _currentWave = 0;
    private int _finalWave = 3;
    [SerializeField]
    private bool _isFinalWave = false;
    [SerializeField]
    private int _enemiesToSpawn = 3;
    [SerializeField]
    private int _enemyCount;
    [SerializeField]
    private float _nextSpawn = 0f;
    private float _spawnDelay = 1f;

    private bool _bossSpawned = false;
   

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
    }

    public void StartSpawning()
    {

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }


    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (_currentWave < _finalWave && _isFinalWave == false)
            {
                if (_enemyCount == 0)
                {
                    for (int i = 0; i < _enemiesToSpawn; i++)
                    {
                        
                        Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
                        yield return new WaitForSeconds(_spawnDelay);
                        _newEnemy = Instantiate(_enemiesArray[Random.Range(0,2)], posToSpawn, Quaternion.identity);
                        _newEnemy.transform.parent = _enemyContainer.transform;
                        _newEnemy.name = "Enemy0";
                    }


                    _enemiesToSpawn *= 2;
                    _currentWave++;
                }
            }
            if (_currentWave == _finalWave)
            {
                _isFinalWave = true;
                
                _enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

                if (_enemyCount == 0 && _bossSpawned == false)
                {
                    _uiManager.TurnOnBossHealthBar();
                    Instantiate(_enemiesArray[2], new Vector3(0, 8, 0), Quaternion.identity);
                    _bossSpawned = true;
                }


            }


            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            _randomValue = Random.Range(0, 101);

            if (_randomValue >= 50) //50% chance
            {
                Instantiate(_powerups[0], posToSpawn, Quaternion.identity);
            }
            else if (_randomValue < 50 && _randomValue > 10) //% chance 
            {
                int randomPowerUp = Random.Range(1, 4);
                Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            }
            else
            {
                Instantiate(_powerups[Random.Range(4, _powerups.Length)], posToSpawn, Quaternion.identity);
            }

            float timeToSpawn = Random.Range(3f, 7f);
            yield return new WaitForSeconds(timeToSpawn);

        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }


}
