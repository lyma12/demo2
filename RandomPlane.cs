using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RandomPlane 
{
    public static Vector2Int RandomPositionObject(int type)
    {
        if (type == 0)
        {
            int t = Random.Range(0, 3);
            if (t == 0) return new Vector2Int(1, 1);
            else if (t == 1) return new Vector2Int(1, 10);
            else if (t == 2) return new Vector2Int(16, 1);
            else return new Vector2Int(16, 10);
            
        }
        else
        {
            return new Vector2Int(Random.Range(3, 14), Random.Range(3, 8));
        }
    }

    
}
