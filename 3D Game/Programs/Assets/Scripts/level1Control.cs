using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class level1Control : MonoBehaviour
{

    public void quitToMain()
    {
        SceneManager.LoadScene(0);
        Debug.Log("!!!");
    }
    public void reset()
    {
        if (!gameControl.Win && !gameControl.Lose)
            replay();
    }
    public void replay()
    {
        Map.gameControl = true;
        gameControl.reset = true;
    }
    public void nextLevel()
    {
        if(Map.level==0)
        {
            Map.gameControl = true;
            gameControl.levelUp = true;
            Map.level = 1;
            
        }
    }
}
