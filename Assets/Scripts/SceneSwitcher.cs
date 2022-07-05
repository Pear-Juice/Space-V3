using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher instance;
    public static string currentScene = "Menu";
    public static string lastScene = "Menu";

    private void Awake()
    {
        instance = this;
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void goBack()
    {
        SceneManager.LoadScene(lastScene);
    }

    public void goSettings()
    {
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Settings");
    }

    public void goMenu()
    {
        lastScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Menu");
    }

    public void goMain()
    {
        SceneManager.LoadScene("World");
    }
}
