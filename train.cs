using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class train: MonoBehaviour
{
    public Vector3 veclovity;
    private Vector3 newVeclocity;
    private Vector3 switchPosition = Vector3.zero;
    
   

    private void run()
    {
        if(switchPosition == Vector3.zero)
        transform.Translate(veclovity * DataGame.speedAllTrain);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, switchPosition, DataGame.speedAllTrain);
        }
        
    }

    private void FixedUpdate()
    {
            if(transform.position ==  switchPosition )
            {
                veclovity = newVeclocity;
                switchPosition = Vector3.zero;
            }
        run();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.gameObject.tag == "Switch")
        {
            
            GameObject other = coll.gameObject;
            switchPosition = other.transform.position;
                int type = other.GetComponent<Switch>().getType();
                if (type == 3)
                {
                    if (veclovity == Vector3.down) newVeclocity = Vector3.right;
                    else if (veclovity == Vector3.left) newVeclocity = Vector3.up;
                    else DestroyTrain();
                }
                else if (type == 2)
                {
                    if (veclovity == Vector3.left) newVeclocity = Vector3.down;
                    else if (veclovity == Vector3.up) newVeclocity = Vector3.right;
                    else DestroyTrain();
                }
                else if (type == 4)
                {
                    if (veclovity == Vector3.down) newVeclocity = Vector3.left;
                    else if (veclovity == Vector3.right) newVeclocity = Vector3.up;
                    else DestroyTrain();
                }
                else if (type == 1)
                {
                    if (veclovity == Vector3.up) newVeclocity = Vector3.left;
                    else if (veclovity == Vector3.right) newVeclocity = Vector3.down;
                    else DestroyTrain();
                }
                else if(type == 5)
                {
                    if (veclovity == Vector3.left) newVeclocity = Vector3.left;
                    else if (veclovity == Vector3.right) newVeclocity = Vector3.right;
                    else DestroyTrain();
                }
            
        }
        else if(coll.gameObject.tag == "Del")
        {
            DestroyTrain();
        }
        return;
    }
    public void setVeclocity(Vector3 vector)
    {
        veclovity = vector;
    }

    public void DestroyTrain()
    {
        // DestroyImmediate(gameObject, false);
        Destroy(gameObject);
    }
}
