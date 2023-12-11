using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("IsOpen", true);
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("IsOpen", false);
    }
}
