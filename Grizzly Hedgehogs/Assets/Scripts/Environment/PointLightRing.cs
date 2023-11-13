using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[ExecuteInEditMode]

// This script is used for creating a ring of point lights.

public class PointLightRing : MonoBehaviour
{
    [Header("========= Settings =========")]
    [SerializeField] new GameObject light;
    [SerializeField] [Range(0, 50)] int count;
    [SerializeField] [Range(0, 50)] double radious;

	List<double> positionX;
	List<double> positionZ;
    List<GameObject> lights;
	bool calculated = false;


	void Start() 
    {
        if (count == 0) return;
		lights = new List<GameObject>();
		positionX = new List<double>();
		positionZ = new List<double>();
		Vector3 center = transform.position;

        float x = 0, z = 0;
        for (int ndx = 0; ndx < count; ++ndx) 
        {
            CalculateCircle(ref x, ref z, ndx);
			lights.Add(Instantiate(light, new Vector3(x + center.x, center.y, z + center.z), transform.rotation, transform));
		}   
    }


    //private void Update() // Don't use until bug is fixed
    //{
    //    while (count != lights.Count)
    //    {
    //        Vector3 center = transform.position;
    //        int x = 0, z = 0, ndx = 0;
    //        calculated = false;
    //        if (count < lights.Count) // Remove lights
    //        {
    //            int size = lights.Count - count;
    //            lights.RemoveRange(lights.Count - size, size);
    //            for (; ndx < lights.Count; ++ndx) // Recalculate the new light positions
    //            {
    //                CalculateCircle(ref x, ref z, ndx);
    //                lights[ndx].transform.position = new Vector3(x + center.x, center.y, z + center.z);
    //            }
    //        }
    //        else if (count > lights.Count)
    //        {
    //            for (; ndx < lights.Count; ++ndx)
    //            {
    //                CalculateCircle(ref x, ref z, ndx);
    //                lights.Add(Instantiate(light, new Vector3(x + center.x, center.y, z + center.z), transform.rotation, transform));
    //            }
    //        }
    //    }
    //}

    void CalculateCircle(ref float x, ref float y, int index) 
    {
        if (index > positionX.Count) return;

		if (!calculated){

            for (int ndx = 0; ndx < count; ++ndx) 
            {
                if (ndx < positionX.Count)
                {
                    positionX[ndx] = radious * Mathf.Cos(2 * Mathf.PI * ndx / count);
                    positionZ[ndx] = radious * Mathf.Sin(2 * Mathf.PI * ndx / count);
                }
                else 
                {
					positionX.Add( radious * Mathf.Cos(2 * Mathf.PI * ndx / count));
					positionZ.Add( radious * Mathf.Sin(2 * Mathf.PI * ndx / count));
				}
            }
            calculated = true;
        }

        x = (int) positionX[index];
        y = (int) positionZ[index];
    }


}
