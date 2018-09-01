﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour {

    public float startFuel; // Tankfüllung Spielanfang
    public float maxFuel = 1000f;
    public float tankVerbrauch;
    private Slider tankSlider;
    private Text tankTxt;

    // Use this for initialization
    void Start () {
        // Tank Obergrenze
        if (startFuel > maxFuel)
            startFuel = maxFuel;

        tankSlider.maxValue = maxFuel;
        UpdateUI();
    }

    void Awake()
    {
        tankTxt = GameObject.FindWithTag("FuelText").GetComponent<Text>();
        tankSlider = GameObject.FindWithTag("FuelSlider").GetComponent<Slider>();
    }

    public void ReduceFuel()
    {
        // Tankfüllung aufbrauchen und UI updaten..
        startFuel -= Time.deltaTime * tankVerbrauch;
        UpdateUI();
    }

    void UpdateUI ()
    {
        tankSlider.value = startFuel;
        tankTxt.text = "Fuel: " + startFuel.ToString("0");

        if(startFuel <=0)
        {
            startFuel = 0;
            tankTxt.text = "Out of fuel";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            startFuel += 200f;
            if(startFuel >= maxFuel)
                startFuel = maxFuel;
            UpdateUI();
        }
    }
}
