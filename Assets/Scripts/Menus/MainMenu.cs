using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Save.Load();
    }

    void EnterGame()
    {
        if (Save.instance.firstEnter)
        {
            Save.instance.firstEnter = false;
            SceneManager.LoadScene("FirstEnterCutscene");
        }
        else
        {
            SceneManager.LoadScene("PlanetChoice");
        }
    }
}
