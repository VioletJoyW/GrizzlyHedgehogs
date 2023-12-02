using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapMarker : MonoBehaviour
{
    [SerializeField] float mapRange;
    [SerializeField] GameObject objectMarked;

    Vector3 origScale;

    Vector3 vectorFromPlayer;

    void Start()
    {
        origScale = transform.localScale;
    }

    void Update()
    {
        vectorFromPlayer = objectMarked.transform.position - gameManager.instance.player.transform.position;

        if ((Mathf.Abs(vectorFromPlayer.x) > mapRange || Mathf.Abs(vectorFromPlayer.y) > 5 || Mathf.Abs(vectorFromPlayer.z) > mapRange) && vectorFromPlayer.magnitude < 50)
        {
            transform.position = new Vector3(gameManager.instance.player.transform.position.x + (vectorFromPlayer.normalized.x * mapRange), gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z + (vectorFromPlayer.normalized.z * mapRange));

            transform.localScale = origScale - new Vector3(vectorFromPlayer.normalized.magnitude, vectorFromPlayer.normalized.magnitude, vectorFromPlayer.normalized.magnitude)*2;
        }
        else
        {
            transform.position = new Vector3(objectMarked.transform.position.x, gameManager.instance.player.transform.position.y, objectMarked.transform.position.z);
            transform.localScale = origScale;
        }
    }
}
