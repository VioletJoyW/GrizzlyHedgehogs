using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripts : MonoBehaviour
{

	[SerializeField] GameObject triggerCall;
	[SerializeField] GameObject lifeWorld;
	[SerializeField] GameObject voidWorld;
	[SerializeField] Material voidWorldSky;
	[SerializeField] TV tv;


	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("Player")) 
        {
            tv.Toggle();
            lifeWorld.SetActive(false);
            RenderSettings.skybox = voidWorldSky;
            voidWorld.SetActive(true);
            triggerCall.SetActive(true);
        }
	}

}
