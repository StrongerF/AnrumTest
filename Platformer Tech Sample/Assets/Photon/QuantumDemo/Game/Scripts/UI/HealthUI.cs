using Photon.Deterministic;
using Quantum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Text healthText;


    public void UpdateText(FP value)
    {
        // TODO: bring value to the ceiling
        healthText.text = FPMath.CeilToInt(value).ToString();
    }

}
