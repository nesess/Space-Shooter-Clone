using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject laserPrefab;

    public int damage = 0;
    public float fireRate = 0.22f;
    private float canFire = 0;


    private void Update()
    {
        if (!GameManager.instance.gamePaused && !GameManager.instance.noEnemy)
        {
            if (Time.time > canFire)
            {
                Vector3 laserPos = new Vector3(transform.position.x-0.03f, transform.position.y+0.1f, 0);
                GameObject laser = Instantiate(laserPrefab, laserPos, Quaternion.identity);
                laser.GetComponent<Laser>().damage = damage;

                Vector3 laserPos2 = new Vector3(transform.position.x+0.03f, transform.position.y+0.1f, 0);
                GameObject laser2 = Instantiate(laserPrefab, laserPos2, Quaternion.identity);
                laser2.GetComponent<Laser>().damage = damage;
                canFire = Time.time + fireRate;
            }
        }
    }
}
