using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (other.CompareTag("PowerUp"))
            {
                other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, 10 * Time.deltaTime);
            }
        }
        if (other.CompareTag("Player"))
        {
            return;

        }




    }

  
}
