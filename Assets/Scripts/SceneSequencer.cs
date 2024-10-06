using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSequencer : MonoBehaviour
{
    public void ChangeToReversi() {
        SceneManager.LoadScene("Reversi");
    }
}
