using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour, iInteract
{
    [SerializeField] int speed;

    bool open = false;

    Quaternion rot;

    void Update()
    {
        if (open)
        {
            rot = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            rot = Quaternion.Euler(0f, 0f, 0f);
        }
        transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, rot, Time.deltaTime * speed);
    }
    public void interact()
    {
        open = !open;
    }
}
