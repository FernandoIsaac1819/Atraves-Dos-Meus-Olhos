using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private string sceneToLoad; // Name of the scene to load

    private void OnTriggerEnter(Collider other) 
    {
        // Check if the object that entered the trigger has a tag "Player"
        // You can adjust this condition based on your game's setup
        if (other.CompareTag("Player"))
        {
            // Ensure the LoadScreen instance exists
            if (WaitScreen.Instance != null)
            {
                WaitScreen.Instance.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError("LoadScreen instance not found!");
            }
        }
    }
}
