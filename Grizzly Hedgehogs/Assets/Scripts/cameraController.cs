using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    //[SerializeField] int camSensitivity;
    [SerializeField] Camera _camera;
    [SerializeField] int camMin;
    [SerializeField] int camMax;
    [SerializeField] int adscamFOV;

    float rotationX;
    //[SerializeField] bool invertY;
    int TimerID;
    // Update is called once per frame
    private void Awake()
    {
        Utillities.CreateGlobalTimer(.5f, ref TimerID);
    }

    void Update()
    {
        Utillities.UpdateGlobalTimer(TimerID);
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;
        _camera.fieldOfView = settingsManager.sm.settingsCurr.camFOV;

        if(_camera != null)
        {
            if (Input.GetKeyDown(settingsManager.sm.settingsCurr.aim) && Utillities.IsGlobalTimerDone(TimerID))
            {
                _camera.fieldOfView = adscamFOV;
                Utillities.ResetGlobalTimer(TimerID);
            }
        }

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
