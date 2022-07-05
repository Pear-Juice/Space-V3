using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settings;
    public GameObject maxPlayers;
    public TMP_InputField settingsAddress;
    public TMP_InputField settingsPort;
    public TMP_InputField settingsMaxPlayers;
    public GameObject mainMenu;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void play()
    {
        SceneSwitcher.instance.goMain();
    }
}
