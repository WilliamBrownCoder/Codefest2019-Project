using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Click : MonoBehaviour
{
    public GameObject hud;
    public GameObject start;
    public GameObject text;
    public GameObject text2;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            text.SetActive(false);
            text2.SetActive(true);
            start.SetActive(false);
            hud.SetActive(true);
        }
    }
}
