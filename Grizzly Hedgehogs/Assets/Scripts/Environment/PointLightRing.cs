using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

//[ExecuteInEditMode]

// This script is used for creating a ring of point lights.

public class PointLightRing : MonoBehaviour
{
    [Header("========= Objects =========")]
    [SerializeField] new GameObject light;
    [Header("========= Settings =========")]
    [SerializeField] [Range(0, 100)] int count;
    [SerializeField] [Range(0, 50)] int angleCount;
    [SerializeField] [Range(0, 50)] double radious;
    [SerializeField] [Range(0, 500)] float activationDistance = 200.0f;
    [SerializeField] bool update;
    [SerializeField] bool intensity;
    [SerializeField] bool refresh;

	List<double> positionX;
	List<double> positionZ;
    List<GameObject> lights;
	Light centerLight;
	bool calculated = false;
	bool childrenOn = false;
	bool childrenWasOn = false;
	void Start() 
    {
		centerLight = GetComponent<Light>();
		lights = new List<GameObject>();
		positionX = new List<double>();
		positionZ = new List<double>();
		Refresh();

	}

	private void Refresh()
	{
		if (count == 0) return;
		foreach(var light in lights) DestroyImmediate(light);
		lights.Clear();

		Vector3 center = transform.position;
		float _range = GetComponent<Light>().range;
		float _intensity = GetComponent<Light>().intensity;
		Color _color = GetComponent<Light>().color;
		float x = 0, z = 0;
		Quaternion oldRot = transform.rotation;
		transform.rotation = Quaternion.identity;
		for (int ndx = 0; ndx < count; ++ndx)
		{
			CalculateCircle(ref x, ref z, ndx, true);
			light.GetComponent<Light>().range = _range;
			light.GetComponent<Light>().color = _color;
			if(intensity) light.GetComponent<Light>().intensity = _intensity;
			lights.Add(Instantiate(light, new Vector3(x + center.x, center.y, z + center.z), transform.rotation, transform));
		}
		transform.rotation = oldRot;
	}

	private void Update()
	{
		if(gameManager.instance.player == null) return;
		Vector3 playerPos = gameManager.instance.player.transform.position;
		Vector3 distance =  playerPos - transform.position; // the distance from the player
		UnityEngine.Debug.DrawRay(transform.position, distance, Color.magenta);
		childrenOn = (distance.magnitude < activationDistance);
		if (!childrenOn) // Truns off the lights if the player is not near by.
		{
			if(centerLight != null) centerLight.enabled = false;
			foreach (Transform child in transform)
				child.gameObject.SetActive(false);
			childrenWasOn = false;
			return;
		}
		else if (!childrenWasOn) 
		{
			if (centerLight != null) centerLight.enabled = true;
			foreach (Transform child in transform)
				child.gameObject.SetActive(true);
			childrenWasOn = true;
		}

		if(angleCount > 0)
		{
			float x = 0, z = 0;
			Vector3 center = transform.position;
			Quaternion oldRot = transform.rotation;
			transform.rotation = Quaternion.identity;
			calculated = false;
			for (int ndx = 0; ndx < lights.Count; ++ndx) 
			{
				CalculateCircle(ref x, ref z, ndx);
				lights[ndx].transform.position = new Vector3(x + center.x, center.y, z + center.z);
			}
			//angleCount = 0;
			transform.rotation = oldRot;
		}
		if(update) 
		{
			float _range = GetComponent<Light>().range;
			Color _color = GetComponent<Light>().color;
			Quaternion _oldRot = transform.rotation;
			transform.rotation = Quaternion.identity;
			calculated = false;
			float x = 0, z = 0;
			Vector3 center = transform.position;
			if(refresh) 
			{
				Refresh();
				refresh = false;
			}

			for (int ndx = 0; ndx < lights.Count; ++ndx) // Recalculate the new light positions
			{
				CalculateCircle(ref x, ref z, ndx);
				lights[ndx].GetComponent<Light>().range = _range;
				lights[ndx].GetComponent<Light>().color = _color;
				lights[ndx].transform.position = new Vector3(x + center.x, center.y, z + center.z);
			}
			transform.rotation = _oldRot;
		}
	}

	// private void Update() // Don't use until bug is fixed
	// {
	//     if (count != lights.Count || radious != rad)
	//     {
	//         Vector3 center = transform.position;
	//         rad = radious;
	//         int ndx = 0;
	//         float x = 0, z = 0;
	//calculated = false;
	//         if (count < lights.Count) // Remove lights
	//         {
	//             int size = lights.Count - count;
	//             int rangeIndex = lights.Count - size;

	//             for (int i = 0; i < lights.Count; ++i) DestroyImmediate(lights[i]);
	//             lights.Clear();
	//             for (; ndx < lights.Count; ++ndx) // Recalculate the new light positions
	//             {
	//                 CalculateCircle(ref x, ref z, ndx);
	//                 lights.Add(Instantiate(light, new Vector3(x + center.x, center.y, z + center.z), transform.rotation, transform));
	//                 //lights[ndx].transform.position = new Vector3(x + center.x, center.y, z + center.z);
	//             }
	//         }
	//         else if (count > lights.Count)
	//         {
	//             for (; ndx < count; ++ndx)
	//             {
	//                 CalculateCircle(ref x, ref z, ndx);
	//                 lights.Add(Instantiate(light, new Vector3(x + center.x, center.y, z + center.z), transform.rotation, transform));
	//             }
	//         }
	//         else 
	//         {


	//}
	//     }
	// }

	void CalculateCircle(ref float x, ref float y, int index, bool _firstRun = false) 
    {
        if (index > positionX.Count) return;

		if (!calculated){

            for (int ndx = 0; ndx < count; ++ndx) 
            {
                if (ndx < positionX.Count)
                {
                    positionX[ndx] = radious * Mathf.Cos(2 * Mathf.PI * ndx / (count + (_firstRun ? 0 : angleCount) ));
                    positionZ[ndx] = radious * Mathf.Sin(2 * Mathf.PI * ndx / (count + (_firstRun ? 0 : angleCount)));
                }
                else 
                {
					positionX.Add( radious * Mathf.Cos(2 * Mathf.PI * ndx / (count + (_firstRun ? 0 : angleCount))));
					positionZ.Add( radious * Mathf.Sin(2 * Mathf.PI * ndx / (count + (_firstRun ? 0 : angleCount))));
				}
            }
            calculated = true;
        }

        x = (float) positionX[index];
        y = (float) positionZ[index];
    }


}
