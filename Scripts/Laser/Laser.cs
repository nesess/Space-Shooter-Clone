using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    
    public float laserSpeed = 12;
    public int damage;
    public Vector3 direction;

    void Update()
    {
        laserMovement(direction);
    }

    public void laserMovement(Vector3 direction)
    {
       
        transform.position += transform.up * laserSpeed * Time.deltaTime;

        if (transform.position.y > 25 || transform.position.y < -25)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        Destroy(this.gameObject, 10f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && this.gameObject.tag == "EnemyLaser")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if(player == null)
            {
                player = other.gameObject.GetComponentInParent<Player>();
            }
            player.damagePlayer(damage);
        }
    }
}
