using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelHandler : MonoBehaviour
{
    void Update()
    {
        // ESCキーが押されたらゲームを終了する
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
        }
    }
}
