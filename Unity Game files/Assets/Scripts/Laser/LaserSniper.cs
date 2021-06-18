using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSniper : MonoBehaviour
{

    private LayerMask layerMask = 1 << 8;
    private LineRenderer myLineRenderer;

    private void Awake()
    {
        myLineRenderer = GetComponent<LineRenderer>();

    }


    private void Update()
    {
        myLineRenderer.SetPosition(0, transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up,200f,layerMask);
        if(hit.collider && hit.collider.gameObject.tag == "Player")
        {
            myLineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, transform.position.z));
        }
        else
        {
            myLineRenderer.SetPosition(1, -transform.up*2000);
        }


        
    }


}
