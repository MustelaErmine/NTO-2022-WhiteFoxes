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

    public void EnterGame()
    {
        if (Save.instance.firstEnter)
        {
            Save.instance.firstEnter = false;
            PlayFirstEnterCutscene();
        }
        else
        {
            SceneManager.LoadScene("PlanetChoice");
        }
    }

    void PlayFirstEnterCutscene()
    {
        SceneManager.LoadScene("FirstEnterCutscene");
    }
}
