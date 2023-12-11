using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oracle_Intro_start : MonoBehaviour
{
    bool isRady = false;
	[SerializeField] Animator animator;

	public bool IsRady { get => isRady; set => isRady = value; }

	private void Awake()
	{
		if(animator == null)
			animator = GetComponent<Animator>();
	}

	private void Update()
	{
		if (animator == null) return;
		if(IsRady)
		{
			animator.Play("Run");
		}
	}


}
