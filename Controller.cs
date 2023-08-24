
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class Controller : MonoBehaviour
{
    private HashSet<Vector2Int> direction = new HashSet<Vector2Int>()
    {
        new Vector2Int(1, 0),
        new Vector2Int(0,1),
        new Vector2Int(-1, 0),
        new Vector2Int(0,-1)
    };
    public GameObject del;
    public Text textCount;
    private int countNow;
    public Text time;
    private float t;
    public GameObject canvaMenu;
    public Text goal;
    public StartTrain ham;
    public GameObject house;
    public GameObject switchOb;
    public TileBase thang;
    public TileBase ngang;
    public Tilemap tileMap;
    private int level;
    private Vector2Int startPosition = new Vector2Int();
    private HashSet<Vector2Int> switchPosition = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> positionPath = new HashSet<Vector2Int>();
    private HashSet<Vector3Int> listColor = new HashSet<Vector3Int>();
    private bool[,] tablePosition = new bool[17, 11];
    private bool speedUp = true;
    private void settingLevel()
    {
        
        /*
         * 1. tạo vị trí của các vật thể
         * 2. từ vị trí vẽ đường đi từ ham đến nhà và đặt các switch ở đường nối
         */
        setListColor();
        startPosition = RandomPlane.RandomPositionObject(0);
     
        HashSet<Vector2Int> housePo = new HashSet<Vector2Int>();
        for (int i = 1; i <= 16; i++)
        {
            for (int j = 1; j <= 10; j++) tablePosition[i, j] = true;
        }
        int[] choosen = new int[17];
        for (int i = 1; i <= 16; i++) choosen[i] = 0;
        while (housePo.Count <= level)
        {
            Vector2Int t = RandomPlane.RandomPositionObject(1);
            if (Vector2Int.Distance(t, startPosition) > 2)
            {
                if (choosen[t.x] < 2)
                {
                    if (t.x == 3 || t.x == 14)
                    {
                        choosen[t.x]++;
                        housePo.Add(t);
                    }
                    else if (choosen[t.x - 1] == 0 && choosen[t.x + 1] == 0)
                    {
                        choosen[t.x]++;
                        housePo.Add(t);
                    }
                }
            }
        }
        if(startPosition.x == 1)
        {
            for (int i = -1; i < startPosition.x; i++) positionPath.Add(new Vector2Int(i, startPosition.y));
            Vector3 hamP = new Vector3();
            hamP.x = -2f + 0.5f;
            hamP.y = (float)startPosition.y * 1 + 0.5f;
            hamP.z = 0;
            StartTrain player = Instantiate(ham, hamP, Quaternion.identity);
            player.level = level;
            player.setStartVeclocity(Vector3.right);
            player.listColor = listColor;
        }
        else
        {
            for (int i = startPosition.x+1; i <= 17 ; i++) positionPath.Add(new Vector2Int(i, startPosition.y));
            Vector3 hamP = new Vector3();
            hamP.x = 18f + 0.5f;
            hamP.y = (float)startPosition.y * 1 + 0.5f;
            hamP.z = 0;
            StartTrain player = Instantiate(ham, hamP, Quaternion.identity);
            player.level = level;
            player.setStartVeclocity(Vector3.left);
            player.listColor = listColor;
        }

        foreach(var i in housePo)
        {
            tablePosition[i.x, i.y] = false;
        }
        pathTrain(housePo);
        foreach(var i in switchPosition)
        {
            positionPath.Remove(i);
        }
        tileMap.ClearAllTiles();
        foreach (var i in positionPath) {
            var p = tileMap.WorldToCell(new Vector3Int(i.x, i.y, 0));
            if(i.y == 1|| i.y == 10)
            {
                
                tileMap.SetTile(p, ngang);
            }
            else
            {
                tileMap.SetTile(p, thang);
            }
            
        }
        int houseNumber = 0;
        foreach(var i in housePo)
        {
            
            createObject(i, house, true, houseNumber);
            houseNumber++;
        }
        switchPosition.Add(startPosition);
        foreach (var i in switchPosition)
        {
            positionPath.Add(i);
            createObject(i, switchOb, false, 0);
        }
        HashSet<Vector2Int> delPosition = findPositionDel(switchPosition, positionPath);
        foreach(var i in delPosition) createObject(i, del, false, 0);
    }
    private HashSet<Vector2Int> findPositionDel(HashSet<Vector2Int> swP, HashSet<Vector2Int> pathP)
    {
        HashSet<Vector2Int> result = new HashSet<Vector2Int>();
        foreach(var i in swP)
        {

            Vector2Int tmp = Vector2Int.zero;
            foreach (var j in direction)
            {
                tmp = i + j;
  
                if (!pathP.Contains(tmp))
                {
                    result.Add(tmp);
                }

            }
           
        }
        return result;
    }
    private void setListColor()
    {
        while(listColor.Count <= level)
        {
            int x = Random.Range(50, 200);
            int y = Random.Range(1, 50);
            int z = Random.Range(1,5);
            listColor.Add(new Vector3Int(x, y*4, z*40));
            
        }
       
    }
    private void createObject(Vector2Int position, GameObject ob, bool type, int n)
    {
        Vector3 t = new Vector3();
        t.x = (float)position.x * 1 + 0.5f;
        t.y = (float)position.y * 1 + 0.5f;
        t.z = 0;
        GameObject gameOb = Instantiate(ob, t, Quaternion.identity);
        if (type)
        {
            Vector3Int color = listColor.ElementAt(n);

            gameOb.GetComponent<Renderer>().material.color = new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255);

            
        }
    }

    private void pathTrain(HashSet<Vector2Int> houseP)
    {
        if(startPosition.y == 1)
        {
            foreach (var position in houseP) houseDown(position);
        }
        else
        {
            foreach (var position in houseP) houseUp(position);
        }
        if(startPosition.x == 16)
        {
            int i = 1;
            for(i = 1; i < 16; i++)
            {
                if (positionPath.Contains(new Vector2Int(i, 1))) break;
            }
            for (int j = i; j < 16; j++) positionPath.Add(new Vector2Int(j, 1));

            i = 1;
            for (i = 1; i < 16; i++)
            {
                if (positionPath.Contains(new Vector2Int(i, 10))) break;
            }
            for (int j = i; j < 16; j++) positionPath.Add(new Vector2Int(j, 10));
        }
        else
        {
            int i = 16;
            for (i = 16; i > 0; i--)
            {
                if (positionPath.Contains(new Vector2Int(i, 1))) break;
            }
            for (int j = i; j > 0; j--) positionPath.Add(new Vector2Int(j, 1));

            i = 16;
            for (i = 16; i > 0; i--)
            {
                if (positionPath.Contains(new Vector2Int(i, 10))) break;
            }
            for (int j = i; j >0; j--) positionPath.Add(new Vector2Int(j, 10));
        }
    }
    
    private void houseDown(Vector2Int position)
    {
        bool other = true;
        for(int i = position.y - 1; i > 0; i--)
        {
            if (!tablePosition[position.x, i]) other = false;
        }
        if (other)
        {
            for (int i = position.y - 1; i > 0; i--) positionPath.Add(new Vector2Int(position.x, i));
            switchPosition.Add(new Vector2Int(position.x, 1));
        }
        else
        {
            switchPosition.Add(new Vector2Int(position.x, 10));
            for (int i = position.y + 1; i <= 10; i++) positionPath.Add(new Vector2Int(position.x, i));
            for (int i = 1; i <= 10; i++) positionPath.Add(new Vector2Int(startPosition.x, i));
            switchPosition.Add(new Vector2Int(startPosition.x, 10));
        }
    }
    private void houseUp(Vector2Int position)
    {
        bool other = true;
        for(int i = position.y + 1; i <= 10; i++)
        {
            if (!tablePosition[position.x, i]) other = false;
        }
        if (other)
        {
            for (int i = position.y + 1; i <= 10; i++) positionPath.Add(new Vector2Int(position.x, i));
            switchPosition.Add(new Vector2Int(position.x, 10));
        }
        else
        {
            switchPosition.Add(new Vector2Int(position.x, 1));
            switchPosition.Add(new Vector2Int(startPosition.x, 1));
            for (int i = position.y - 1; i > 0; i--) positionPath.Add(new Vector2Int(position.x, i));
            for (int i = 1; i <= 10; i++) positionPath.Add(new Vector2Int(startPosition.x, i));
        }
    }
    private void FixedUpdate()
    {
        t += Time.deltaTime;
        if (t > DataGame.time)
        {
            canvaMenu.SetActive(true);
            if(countNow >= DataGame.goal)
            {
                goal.text = "YOU PASS";
            }
            else
            {
                goal.text = "LOSER";
            }
        }
        else
        {
            countNow = DataGame.count;
            textCount.text = DataGame.count.ToString();
            time.text = t.ToString("0");
        }
        if (countNow % (int)(DataGame.goal / 3) == 0 && countNow != 0 && speedUp)
        {
            DataGame.speedAllTrain += 0.02f;
            speedUp = false;
        }
        else if (countNow % (int)(DataGame.goal / 3) != 0) speedUp = true;
        
    }
    public void home()
    {
        
        canvaMenu.SetActive(false);
        SceneManager.LoadScene("Start");
    }
    public void rePlayer()
    {
        
        canvaMenu.SetActive(false);
        SceneManager.LoadScene("GameState");
    }
    public void Start()
    {
        DataGame.speedAllTrain = 0.02f;
        t = 0;
        countNow = 0;
        DataGame.count = 0;
        level = DataGame.level;
        settingLevel();
    }
}
