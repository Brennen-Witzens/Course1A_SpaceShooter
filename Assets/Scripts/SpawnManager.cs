using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;

    private GameObject _newEnemy;
    private Animator _newEnemyAnim;

    private bool _stopSpawning = false;

    [SerializeField]
    private float _randomValue;

    // Start is called before the first frame update
    void Start()
    {
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
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            _newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            _newEnemy.transform.parent = _enemyContainer.transform;
            _newEnemy.name = "Enemy0";


            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 8, 0);
            _randomValue = Random.value;
            if(_randomValue >= 0.2)
            {
                int randomPowerUp = Random.Range(0, _powerups.Length - 1);
                Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            } else
            {
                Instantiate(_powerups[5], posToSpawn, Quaternion.identity);
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
