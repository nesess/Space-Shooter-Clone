using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerLaser")
        {
            
            Destroy(other.gameObject);
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            Enemy enemy = GetComponentInParent<Enemy>();
            player.damagePlayer(enemy.damage);
            Vector3 direction = other.contacts[0].point;
            direction -= transform.position;
            direction.z = 0;
            player.pushPlayer(0.5f,direction);
        }
    }
}
