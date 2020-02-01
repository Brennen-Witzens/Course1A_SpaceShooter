using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed;

    [SerializeField] // 0 = Triple Shot, 1 = Speed, 2 = Shields
    private int powerupID;

    [SerializeField]
    private AudioClip _clip;


    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if (player != null)
            {
                switch (powerupID)
                {

                    case 0: //Triple Shot
                        player.TripleShotActive();
                        break;
                    case 1: //Speed Boost
                        player.SpeedBoostActive();
                        break;
                    case 2: //Shields
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("You somehow managed to get here?");
                        break;
                }

            }

            Destroy(this.gameObject);
        }
    }





}
