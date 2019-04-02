using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuControl : MonoBehaviour
{
    public void playLevel1()
    {
        SceneManager.LoadScene(1);
        Map.level = 0;
    }
    public void quit()
    {
        Application.Quit();
    }
}
