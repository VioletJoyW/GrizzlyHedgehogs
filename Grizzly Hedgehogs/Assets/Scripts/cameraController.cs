using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    //[SerializeField] int camSensitivity;
    [SerializeField] Camera adsCamera;
    [SerializeField] int camMin;
    [SerializeField] int camMax;

    float rotationX;
    //[SerializeField] bool invertY;

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;

        if (settingsManager.sm.settingsCurr.invertY)
        {
            rotationX += mouseY;
        }
        else
        {
            rotationX -= mouseY;
        }

        rotationX = Mathf.Clamp(rotationX, camMin, camMax);
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    
}
