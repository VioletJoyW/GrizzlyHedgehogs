using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oracleCutscene : MonoBehaviour
{
    [Header("---- Components ----")]
    [SerializeField] Material mat1;
    [SerializeField] Material mat2;
    [SerializeField] Animator anim;
    [SerializeField] new Renderer renderer;
    [SerializeField] Mesh mesh;
    [SerializeField] MeshFilter filter;
    [SerializeField] GameObject particle1;
    [SerializeField] GameObject particle2;

    Color newColor;
    Vector3 newSize;
    bool cubeMode = false;
    bool changing = false;
    bool absorbing = false;
    bool talking = false;
    bool waiting = false;
    int talkNum = 0;
    float oldScale;
    [Header("---- Values ----")]
    [SerializeField] int dialogWait;

    // Start is called before the first frame update
    void Start()
    {
        newColor = mat1.color;
        newSize = particle2.transform.localScale;

        renderer.material.color = mat1.color;

        gameManager.instance.stateUnPause();
        gameManager.instance.player.SetActive(false);

        particle1.SetActive(false);
        particle2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (talkNum < 6 && !changing && !absorbing)
        {
            if (!talking)
            {
                StartCoroutine(Talk());
                talkNum++;
            }
        }

        if (absorbing)
        {
            particle2.SetActive(true);

            if (!waiting)
            {
                StartCoroutine(Waiting());
            }
            newSize = Vector3.Lerp(newSize, Vector3.zero, Time.deltaTime / 1.5f);
            particle2.transform.localScale = newSize;
        }

        if (changing)
        {
            newColor = Color.Lerp(newColor, mat2.color, Time.deltaTime);
            renderer.material.color = newColor;
            if (!waiting)
            {
                StartCoroutine(Waiting());
            }
            newSize = Vector3.Lerp(newSize, Vector3.zero, Time.deltaTime / 2);
            particle1.transform.localScale = newSize;
        }

        if (cubeMode)
        {
            anim.SetBool("Rotate", true);
            filter.mesh = mesh;
        }
    }

    IEnumerator Waiting()
    {
        waiting = true;

        if (absorbing)
        {
            yield return new WaitForSeconds(3);
            absorbing = false;
            newSize = particle1.transform.localScale;
        }

        if (changing)
        {
            particle2.SetActive(false);
            yield return new WaitForSeconds(4);
            changing = false;
            particle1.SetActive(false);
            cubeMode = true;
        }

        waiting = false;
    }

    IEnumerator Talk()
    {
        talking = true;

        switch (talkNum)
        {
            case 0:
                gameManager.instance.ShowDialog("You're awesome, you know that?");
                
                break;

            case 1:
                gameManager.instance.ShowDialog("You're the only one whose managed to entertain me for this long!");
                break;

            case 2:
                gameManager.instance.ShowDialog("... hmmm...");
                break;

            case 3:
                gameManager.instance.ShowDialog("Let's see how clever you can be.");
                absorbing = true;
                break;
            
            case 4:
                gameManager.instance.ShowDialog("As your final act, you will show me...");

                break;

            case 5:
                gameManager.instance.ShowDialog("How you will escape an enemy you can't defeat!");
                particle2.SetActive(false);
                particle1.SetActive(true);
                changing = true;
                break;

            default:
                gameManager.instance.ShowDialog("You shouldn't see this.");
                break;
        }

        yield return new WaitForSeconds(0.01f);
        talking = false;
    }
}
