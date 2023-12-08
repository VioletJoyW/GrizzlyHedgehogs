using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
	//[SerializeField] int camSensitivity;
	[SerializeField] Camera _camera;
    [SerializeField] int camMin;
    [SerializeField] int camMax;
    [SerializeField][Range(1f, 100f)] float aimZoomSpeed = 5f;
    [SerializeField][Range(1f, 179f)] float aimFOV = 50f;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject gunAimOrigin;
    [SerializeField] GameObject gunAimDestination;

    float rotationX;
    //[SerializeField] bool invertY;
    // Update is called once per frame
    private void Awake()
    {
        if(_camera == null) _camera = GetComponent<Camera>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * settingsManager.sm.settingsCurr.camSensitivity;

        if(_camera != null)
        {
            if (Input.GetKey(settingsManager.sm.settingsCurr.aim))
            {
				_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, aimFOV, Time.deltaTime * aimZoomSpeed);
                gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, gunAimDestination.transform.localPosition, Time.deltaTime * aimZoomSpeed);
                gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, gunAimDestination.transform.localRotation, Time.deltaTime * aimZoomSpeed);
			}
            else 
            {
				_camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, settingsManager.sm.settingsCurr.camFOV, Time.deltaTime * aimZoomSpeed);
				gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, gunAimOrigin.transform.localPosition, Time.deltaTime * aimZoomSpeed);
				gun.transform.localRotation = Quaternion.Lerp(gun.transform.localRotation, gunAimOrigin.transform.localRotation, Time.deltaTime * aimZoomSpeed);
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
