using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSizeModifier : MonoBehaviour
{
    private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mainCamera.orthographicSize = Mathf.Max(0.1f, mainCamera.orthographicSize - 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            mainCamera.orthographicSize += 0.1f;
        }
    }
}
