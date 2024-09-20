using Photon.Deterministic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen;

    public void ShowDeath()
    {
        deathScreen.SetActive(true);

        var mouseLocker = FindAnyObjectByType<MouseLocker>();
        if (mouseLocker != null && mouseLocker.Locked)
        {
            mouseLocker.Toggle();
        }
        
    }
    public void HideDeath()
    {
        deathScreen.SetActive(false);

        var mouseLocker = FindAnyObjectByType<MouseLocker>();
        if (mouseLocker != null && !mouseLocker.Locked)
        {
            mouseLocker.Toggle();
        }
    }
}
