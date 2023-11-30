using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gravityController : MonoBehaviour
{
    float oldGravity;
    [Range(-2, -100)][SerializeField] float newGravity;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            oldGravity = gameManager.instance.playerScript.GetPlayerGravity();
            gameManager.instance.playerScript.SetPlayerGravity(newGravity);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.SetPlayerGravity(oldGravity);
        }
    }
}
