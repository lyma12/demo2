using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void setTime(int t)
    {
        DataGame.time = t;
    }
    public void setGoal(int goal)
    {
        DataGame.goal = goal;
    }
    public void setlevel(int level)
    {
        DataGame.level = level;
        SceneManager.LoadScene("GameState");
    }
}
