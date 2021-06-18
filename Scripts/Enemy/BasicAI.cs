using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour
{
    private Enemy enemyScript;

    [SerializeField]
    private GameObject laserPrefab;

    private GameObject playerObj;
    private bool shooting = false;
    private float fireRate;

    private void Awake()
    {
        enemyScript = GetComponent<Enemy>();
        fireRate = enemyScript.fireRate;
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        
        if(enemyScript.canMove && !GameManager.instance.gamePaused)
        {
            //rotate
            float angle = Mathf.Atan2(playerObj.transform.position.y - transform.position.y, playerObj.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 80 * Time.deltaTime);

            //move
            if(Mathf.Abs(playerObj.transform.position.x - transform.position.x) > 2 || Mathf.Abs(playerObj.transform.position.y - transform.position.y) > 4.5f)
            {
                if(!shooting)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, enemyScript.speed * Time.deltaTime);
                } 
            }
            else if(!shooting && transform.rotation == targetRotation)
            {
                StartCoroutine(shoot());
            }

        }
    }

    private IEnumerator shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(fireRate / 2);
        Vector3 laserPos = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject laserObj = Instantiate(laserPrefab, laserPos, transform.rotation );
        laserObj.transform.Rotate(0, 0, 180);
        Laser laser = laserObj.GetComponent<Laser>();
        laser.damage = enemyScript.damage;
        laser.laserSpeed = 5f;
        laser.direction = playerObj.transform.position;
        yield return new WaitForSeconds(fireRate / 2);
        shooting = false;
    }
}
