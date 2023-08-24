using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//type of switch 
/*
 * type 3: down - right; left - up
 * type 2: left - down; up - right
 * type 4: down - left; right - up
 * type 1: up - left; right - down
 * type 5: null
 */

public class Switch : MonoBehaviour
{
    private int type = 1;
    private Rigidbody2D rig;
    public Sprite newSprite;
    public Sprite oldSprite;
    public SpriteRenderer spriteRe;
    private int rotationTab = 1;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }
    public void OnMouseDown()
    {
        
        setVeclocity();
    }
    public void setVeclocity()
    {
        type++;
        rotationTab++;
        if (type > 5)
        {
            type = 1;
            rotationTab = 1;
        }

        if(rotationTab == 5)
        {
            rig.rotation = 0;
            spriteRe.sprite = newSprite;

        }
        else if(rotationTab == 1)
        {
            spriteRe.sprite = oldSprite;
        }
        else
        {
            rig.rotation += 90;
        }

    }
    public int getType()
    {
        return type;
    }
}
