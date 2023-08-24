using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Random = UnityEngine.Random;

public class StartTrain : MonoBehaviour
{
    public train gameObjectTrain;
    private Vector3 startVeclotity = Vector3.zero;
    public HashSet<Vector3Int> listColor = new HashSet<Vector3Int>();
    public int level = 0;
    private float t = 0;
    private float check = 6f;
    private float tCheck = 0;

    private void FixedUpdate()
    {
        t += Time.deltaTime;
        tCheck += Time.deltaTime;
        if(t > check)
        {
            t = 0;
            Process();
        }
        if(tCheck > DataGame.time / 3)
        {
            check -= 1;
            tCheck = 0;
        }
        if (check < 0) check = 2;
    }
  

    private void Process()
    {
        if (gameObjectTrain != null && startVeclotity != Vector3.zero)
        {
            train player = Instantiate(gameObjectTrain, transform.position, Quaternion.identity);
            player.setVeclocity(startVeclotity);
            int n = Random.Range(0, level + 1);
            Vector3Int color = listColor.ElementAt(n);
            player.GetComponent<Renderer>().material.color = new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255);
        }
       
    }
    public void setStartVeclocity(Vector3 v)
    {
        startVeclotity = v;
    }
}
