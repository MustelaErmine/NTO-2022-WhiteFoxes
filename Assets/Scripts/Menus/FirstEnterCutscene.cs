using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstEnterCutscene : MonoBehaviour
{
    public void Enter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlanetChoice");
    }
}
