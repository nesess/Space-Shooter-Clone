using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraManager : MonoBehaviour
{
    private Transform target;

    public Tilemap theMap;
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    private float halfHeight;
    private float halfWidth;


    // Start is called before the first frame update
    void Start()
    {
        //target = PlayerController.instance.transform;
        target = FindObjectOfType<Player>().transform;



        halfHeight = Camera.main.orthographicSize ; 
        halfWidth = halfHeight * Camera.main.aspect; 

        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight +6 , 0f);
        topRightLimit = theMap.localBounds.max - new Vector3(halfWidth, halfHeight - 6, 0f);

        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //keeping camera ins
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
