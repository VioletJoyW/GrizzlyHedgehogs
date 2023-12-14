using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripts : MonoBehaviour
{

	[SerializeField] GameObject sun;
	[SerializeField] GameObject triggerCall;
	[SerializeField] GameObject lifeWorld;
	[SerializeField] GameObject voidWorld;
	[SerializeField] Material voidWorldSky;
	[SerializeField] TV tv;

    new Light light;

    bool isVoid = false;
	// Start is called before the first frame update
	void Start()
    {
        light = sun.GetComponent<Light>();
    }

    void Update()
    {
        if (isVoid) 
        {
            Color c = voidWorld.GetComponent<MeshRenderer>().material.color;
            Color sun_c = light.color;

			voidWorld.GetComponent<MeshRenderer>().material.color = Color.Lerp(c, Color.black, Time.deltaTime * 0.5f);
            light.color = Color.Lerp(sun_c,  new Color(0,0,1,0), Time.deltaTime * 2.5f);
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player")) // Turns on the void;
        {
            tv.Toggle();
            lifeWorld.SetActive(false);
            RenderSettings.skybox = voidWorldSky;
            voidWorld.SetActive(true);
            triggerCall.SetActive(true);
            isVoid = true;
        }
	}

}
