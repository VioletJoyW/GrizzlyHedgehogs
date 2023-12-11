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

    bool waiting = false;
    bool scrolling = false;
    bool ending = false;

    Vector3 newPos;
    [SerializeField] float scrollSpeed;
    [SerializeField] int maxHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ending)
            StartCoroutine(hold());

        if (header.activeInHierarchy == true && !waiting)
        {
            StartCoroutine(Wait());
        }

        if (scrolling)
        {
            newPos = credits.transform.position + new Vector3(0, scrollSpeed, 0);
            credits.transform.position = Vector3.Lerp(credits.transform.position, newPos, Time.unscaledDeltaTime);
            
            if (credits.transform.position.y > maxHeight && scrolling)
            {
                button.SetActive(true);
                scrolling = false;
            }
        }
    }

    IEnumerator Wait()
    {
        waiting = true;

        yield return new WaitForSeconds(5);

        header.SetActive(false);
        scrolling = true;

        waiting = false;
    }

    IEnumerator hold()
    {
        ending = true;
        //gameManager.instance.playerWon = false;

        yield return new WaitForSeconds(0.5f);

        gameManager.instance.youWin();
        ending = false;

    }
}
