using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherAI : MonoBehaviour
{

    private Enemy enemyScript;


    private bool rushMove = false;
    private GameObject playerObj;
    private bool rushing = false;
    private float rushRate;
    private CircleCollider2D rushCollider;
    private CircleCollider2D myCollider;
    private Vector3 playerPos;

    [SerializeField]
    private GameObject shield1;
    [SerializeField]
    private GameObject shield2;
    [SerializeField]
    private GameObject shield3;

    private void Awake()
    {
        enemyScript = GetComponent<Enemy>();
        rushRate = enemyScript.fireRate;
        rushCollider = GetComponentsInChildren<CircleCollider2D>()[1];
        myCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        rushCollider.enabled = false;
    }

    private void FixedUpdate()
    {

        if (enemyScript.canMove && !GameManager.instance.gamePaused)
        {
            //rotate
            float angle = Mathf.Atan2(playerObj.transform.position.y - transform.position.y, playerObj.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 50 * Time.deltaTime);

            //move
            if (Mathf.Abs(playerObj.transform.position.x - transform.position.x) > 1.5 || Mathf.Abs(playerObj.transform.position.y - transform.position.y) > 2.5f)
            {
                if (!rushing)
                {
                    transform.position = Vector3.MoveTowards(transform.position, playerObj.transform.position, enemyScript.speed * Time.deltaTime);
                }
            }
            else if (!rushing && transform.rotation == targetRotation)
            {
                StartCoroutine(rush());
            }

            


        }

        if (rushMove)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 5*enemyScript.speed * Time.deltaTime);
        }
    }

    private IEnumerator rush()
    {
        rushing = true;
        shield1.SetActive(true);
        rushCollider.enabled = true;
        if(myCollider != null)
        {
            myCollider.enabled = false;
        }
       
        yield return new WaitForSeconds(rushRate / 3);
        shield1.SetActive(false);
        shield2.SetActive(true);
        playerPos = playerObj.transform.position;
        yield return new WaitForSeconds(rushRate / 3);
        shield2.SetActive(false);
        shield3.SetActive(true);


        rushMove = true;
        while (transform.position != playerPos)
        {
            yield return new WaitForSeconds(0.1f);

        }
        rushMove = false;
        shield3.SetActive(false);
        rushCollider.enabled = false;
        if (myCollider != null)
        {
            myCollider.enabled = true;
        }
        yield return new WaitForSeconds(rushRate / 3);
        rushing = false;

    }

    
}
