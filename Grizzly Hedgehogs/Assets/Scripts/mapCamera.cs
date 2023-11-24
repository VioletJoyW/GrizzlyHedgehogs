using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapCamera : MonoBehaviour
{
    [SerializeField] Camera mapCam;
    void Update()
    {
        mapCam.nearClipPlane = transform.position.y - gameManager.instance.player.transform.position.y - 5;
        mapCam.farClipPlane = transform.position.y - gameManager.instance.player.transform.position.y + 10;
    }
}
