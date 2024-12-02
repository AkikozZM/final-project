using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegeneratePlayerPosition : MonoBehaviour
{
    public GameObject player;
    public GUIController guiController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            guiController.StartFadeOutAndRestart();
            guiController.updateCurrentTime();
        }
    }
}
