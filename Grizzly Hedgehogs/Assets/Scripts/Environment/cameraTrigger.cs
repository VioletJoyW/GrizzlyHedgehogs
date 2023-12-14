using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrigger : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] float viewDistance;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            camera.farClipPlane = viewDistance;
        }
    }
}
