using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneSwitcher : MonoBehaviour
{
    public void Switch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }
}
