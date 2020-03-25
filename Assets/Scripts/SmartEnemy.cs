using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private GameObject _laserPrefab;

    private Animator _enemyAnimator;
    private AudioSource _explosionSource;

    private Player _player;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private int _randomLaserFire = 5;
    private bool _hasFired = false;

    [SerializeField]
    private LayerMask _onlyPlayer;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();


        
        if (_canFire < Time.time)
        {
            _fireRate = _randomLaserFire;
            _canFire = Time.time + _fireRate;
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, -.5f, 0), Quaternion.Euler(0, 0, 180));
        }
        StartCoroutine(CheckForPlayerBehind());
    }



    private void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -4)
        {
           float _randomXRange = Random.Range(-8f, 8f);
            transform.position = new Vector3(_randomXRange, 8, 0);
        }


        RaycastHit2D hitInfo = Physics2D.BoxCast(transform.position, transform.localScale * 3f ,0, Vector2.down * 3);
        
       
        if (hitInfo)
        {
            Debug.DrawRay(transform.position, Vector2.down * 2);
           // Debug.Log("Hit: " + hitInfo.collider.name);

            if (hitInfo.collider.CompareTag("Laser"))
            {
                Vector3 target = new Vector3((transform.position.x - 1), transform.position.y);
                transform.position = Vector3.Lerp(transform.position, target, 1);
            }
            else
            {
                return;
            }
        }



            

        


    }


    private IEnumerator CheckForPlayerBehind()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.up * 3, _onlyPlayer);
       
        for (int i = 0; i < hits.Length; ++i)
        {
            /*Debug.LogFormat("The name of collider {0} is \"{1}\".",
                i, hits[i].collider.gameObject.name);
            */

            if (hits[i].collider.gameObject.CompareTag("Player") && !_hasFired)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0f, 0.5f, 0), Quaternion.identity);
                _hasFired = true;
                yield return new WaitForSeconds(1.5f);
                _hasFired = false;
            }
               
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
                this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }

            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _fireRate = 0;

            _explosionSource.Play();
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            
            if (_player != null)
            {
                _player.AddScore(20);
                this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                _fireRate = 0;
            }
            _enemyAnimator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _explosionSource.Play();
            Destroy(this.gameObject);
        }

    }




}
