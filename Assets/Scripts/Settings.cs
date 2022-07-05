using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //General
    //public static int fontSize;
    //public static int uiScale;

    //Flight

    public List<GameObject> settingPanes = new List<GameObject>();
    public GameObject currentPane;

    private void Start()
    {
        currentPane = settingPanes[0];
    }

    [System.Serializable]
    public struct Setting
    {
        public GameObject valueObj;
        public GameObject displayObj;
    }
    
    public Setting flight_flightSpeed;
    public Setting flight_reverseFlightSpeed;
    public Setting flight_rollSpeed;
    public Setting flight_turnSpeed;

    public static ShipSettings shipSettings = new ShipSettings();
    public static LanderSettings landerSettings = new LanderSettings();
    
    public class ShipSettings
    {
        public float speedMultiplier = 1;
        public float verticalSpeedMultiplier = 1;
        public float boostMultiplier = 1;
        public float rollSpeedMultiplier = 1;
        public float turnSpeedMultiplier = 1;
    }

    //Controls
    public void setFlightSpeed()
    {
        shipSettings.speedMultiplier = flight_flightSpeed.valueObj.GetComponent<Slider>().value;
        setDisplayVal(flight_flightSpeed.displayObj, (Mathf.Round(shipSettings.speedMultiplier * 100) * 0.01f) + "");
    }

    public void setReverseFlightSpeed()
    {
        shipSettings.speedMultiplier = flight_reverseFlightSpeed.valueObj.GetComponent<Slider>().value;
        setDisplayVal(flight_reverseFlightSpeed.displayObj, (Mathf.Round(shipSettings.speedMultiplier * 100) * 0.01f) + "");
    }

    public void setRollSpeed()
    {
        shipSettings.rollSpeedMultiplier = flight_rollSpeed.valueObj.GetComponent<Slider>().value;
        setDisplayVal(flight_rollSpeed.displayObj, (Mathf.Round(shipSettings.rollSpeedMultiplier * 100) * 0.01f) + "");
    }

    public void setTurnSpeed()
    {
        shipSettings.turnSpeedMultiplier = flight_turnSpeed.valueObj.GetComponent<Slider>().value;
        setDisplayVal(flight_turnSpeed.displayObj, (Mathf.Round(shipSettings.turnSpeedMultiplier * 100) * 0.01f) + "");
    }
    
    public class LanderSettings
    {
        public float speedMultiplier = 50;
        public float verticalLookSpeedMultiplier = 1;
        public float turnSpeedMultiplier;
        public float jumpHeightMultiplier = 10;
    }

    void setDisplayVal(GameObject display,string val)
    {
        display.GetComponent<TextMeshProUGUI>().text = val;
    }

    public void switchSettingsPane(int pane)
    {
        currentPane.SetActive(false);
        settingPanes[pane].SetActive(true);
    }

}
