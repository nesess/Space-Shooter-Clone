using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperAI : MonoBehaviour
{

    private Enemy enemyScript;
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject laserPrefab;

    private GameObject playerObj;
    private bool shooting = false;
    private float fireRate;
    

    private void Awake()
    {
        enemyScript = GetComponent<Enemy>();
        fireRate = enemyScript.fireRate;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        
    }


    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (enemyScript.canMove && !GameManager.instance.gamePaused)
        {
            //rotate
            float angle = Mathf.Atan2(playerObj.transform.position.y - transform.position.y, playerObj.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 60 * Time.deltaTime);

            //move
            if (Mathf.Abs(playerObj.transform.position.x - transform.position.x) > 4 || Mathf.Abs(playerObj.transform.position.y - transform.position.y) > 7f)
            {
                if (!shooting)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, enemyScript.speed * Time.deltaTime);
                }
            }
            else if (!shooting && transform.rotation == targetRotation)
            {
                StartCoroutine(shoot());
            }

        }
    }

    private IEnumerator shoot()
    {
        shooting = true;
        yield return new WaitForSeconds(fireRate / 3);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(fireRate / 3);
        enemyScript.canMove = false;
        yield return new WaitForSeconds(fireRate / 8);
        lineRenderer.enabled = false;
        Vector3 laserPos = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject laserObj = Instantiate(laserPrefab, laserPos, transform.rotation);
        laserObj.transform.Rotate(0, 0, 180);
        Laser laser = laserObj.GetComponent<Laser>();
        laser.damage = enemyScript.damage;
        laser.laserSpeed = 15f;
        laser.direction = playerObj.transform.position;
        enemyScript.canMove = true;
        
        yield return new WaitForSeconds(fireRate/2);
        shooting = false;
        
        
    }

}
