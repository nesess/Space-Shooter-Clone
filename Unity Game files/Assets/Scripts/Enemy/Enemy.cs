using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int damage;
    public float speed;
    public int exp;
    public float fireRate;

    public bool canMove = true;

    private bool imDead = false;


    private void Start()
    {
        maxHealth += maxHealth / 5 *GameManager.instance.getDifficulty();
        health = maxHealth;
        damage += damage / 5 *  GameManager.instance.getDifficulty();
        speed += speed  / 10 * GameManager.instance.getDifficulty();
        exp += exp * GameManager.instance.getDifficulty();
    }

    private void FixedUpdate()
    {
        
    }

    public void dead()
    {
        
        GameManager.instance.setTotalExp(exp);
        Destroy(GetComponent<CircleCollider2D>());
        Destroy(GetComponentInChildren<SpriteRenderer>());
        GetComponentInChildren<Animator>().SetTrigger("enemyDeath");
        LineRenderer line = GetComponentInChildren<LineRenderer>();
        if(line !=null)
        {
            line.gameObject.SetActive(false);
        }
        CircleCollider2D childCollider = GetComponentInChildren<CircleCollider2D>();
        if(childCollider != null)
        {
            childCollider.gameObject.SetActive(false);
        }
        LevelManager.instance.enemyDead();
        Destroy(this.gameObject, 0.5f);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerLaser" && !imDead)
        {
            health -= other.gameObject.GetComponent<Laser>().damage;
            Destroy(other.gameObject);
            if (health <= 0)
            {
                imDead = true;
                dead();
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
        if(other.gameObject.tag == "Player" && !imDead)
        {
            
            Player player = other.gameObject.GetComponent<Player>();
            health -= player.maxHealth / 5;
            player.damagePlayer( this.maxHealth / 5);
            if (health <= 0)
            {
                dead();
            }
            else
            {
                Vector3 direction = other.contacts[0].point;
                direction -= transform.position;
                direction.z = 0;
                player.pushPlayer(0.3f,direction);
                direction = -direction;
                if(canMove)
                {
                    StartCoroutine(pushEnemy(direction));
                }
               
                

            }
        }
    }

    private IEnumerator pushEnemy(Vector3 direction)
    {

        GetComponent<Rigidbody2D>().AddForce(direction);
        GetComponent<Rigidbody2D>().freezeRotation = true;
        canMove = false;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    
}
