using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using IngameDebugConsole;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text record;
    static bool createdSrc = false;
    Animator an;
    public void Start()
    {
        Save.Load();
        record.text = /*"Ваш рекорд по световым годам: " +*/ Save.instance.record.ToString();
        if (!createdSrc)
        {
            DontDestroyOnLoad(GameObject.Find("BtnSource"));
            createdSrc = true;
        }
        DebugLogConsole.AddCommand("clearSave", "", () => {
            Save.instance = new Save();
            Save.Keep();
        });
    }

    public void EnterGame()
    {
        if (Save.instance.session != null)
        {
            SceneManager.LoadScene("BlockConstruction");
        }
        else if (Save.instance.firstEnter)
        {
            Save.instance.firstEnter = false;
            Save.Keep();
            PlayFirstEnterCutscene();
        }
        else
        {
            SceneManager.LoadScene("BlockConstruction");
        }
    }

    public void PlayFirstEnterCutscene()
    {
        SceneManager.LoadScene("FirstEnterCutscene");
    }
    public void EndGame()
    {
        Save.Keep();
        Application.Quit();
    }
}
