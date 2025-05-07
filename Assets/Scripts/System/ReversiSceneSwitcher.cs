using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReversiSceneSwitcher : MonoBehaviour
{
    public void Switch()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Reversi");
    }
}
