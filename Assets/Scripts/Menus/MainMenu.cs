using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text record;
    public void Start()
    {
        Save.Load();
        record.text = "Ваш рекорд по световым годам: " + Save.instance.record.ToString();
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
