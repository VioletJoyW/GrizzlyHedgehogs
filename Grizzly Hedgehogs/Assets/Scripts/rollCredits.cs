using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class rollCredits : MonoBehaviour
{
    [SerializeField] GameObject header;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject button;

    bool scrolling = false;

    Vector3 newPos;
    [SerializeField] float scrollSpeed;
    [SerializeField] int maxHeight;
    [SerializeField] int timerID;
    [SerializeField] int waitTime;

    // Start is called before the first frame update
    void Start()
    {
        Utillities.CreateGlobalTimer(waitTime, ref timerID);
    }

    // Update is called once per frame
    void Update()
    {
        if (header.activeInHierarchy == true)
        {
            Wait();
        }

        if (scrolling)
        {
            
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(settingsManager.sm.settingsCurr.interact) 
                || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.E))
            {
                newPos = credits.transform.position + new Vector3(0, scrollSpeed * 2, 0);
                credits.transform.position = Vector3.Lerp(credits.transform.position, newPos, Time.unscaledDeltaTime);
            }
            else
            {
                newPos = credits.transform.position + new Vector3(0, scrollSpeed, 0);
                credits.transform.position = Vector3.Lerp(credits.transform.position, newPos, Time.unscaledDeltaTime);
            }
            
            if (credits.transform.position.y > maxHeight && scrolling)
            {
                button.SetActive(true);
                scrolling = false;
            }
        }
    }

    void Wait()
    {
        Utillities.UpdateGlobalFixedTimer(timerID);

        if (Utillities.IsGlobalTimerDone(timerID))
        {
            header.SetActive(false);
            scrolling = true;
        }
    }
}
