using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTrick : MonoBehaviour
{

    [SerializeField] GameObject light;
    int done = 0;
	[ExecuteInEditMode]
	void Start()
    {
		switch (done)
		{
			case 0:
				{
					light.SetActive(false);
					++done;
				}
				break;
			case 1:
				{
					light.SetActive(true);
					++done;
				}
				break;
		}
	}

    // Update is called once per frame
    void Update()
    {
		switch (done)
		{
			case 0:
				{
					light.SetActive(false);
					++done;
				}
				break;
			case 1:
				{
					light.SetActive(true);
					++done;
				}
				break;
		}
	}
}
