using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Start()
    {
        Save.Load();
    }

    public void EnterGame()
    {
        if (Save.instance.session != null)
        {
            SceneManager.LoadScene("PlanetChoice");
        }
        else if (Save.instance.firstEnter)
        {
            Save.instance.firstEnter = false;
            Save.Keep();
            PlayFirstEnterCutscene();
        }
        else
        {
            Save.instance.session = new Session();
            SceneManager.LoadScene("PlanetChoice");
        }
    }

    void PlayFirstEnterCutscene()
    {
        SceneManager.LoadScene("FirstEnterCutscene");
    }
}
