using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    [SerializeField] float detonationTime;
    [SerializeField] float velocityUp;
    [SerializeField] float radious;
    [SerializeField] GameObject explosion;

	// Start is called before the first frame update
	void Start()
    {
        rb.velocity = Vector3.up * velocityUp + (transform.forward * speed);
        StartCoroutine(explode());
    }

    IEnumerator explode()
    {
        yield return new WaitForSeconds(detonationTime);
        if (explosion != null) 
        {
			GameObject tmp = Instantiate(explosion, transform.position, explosion.transform.rotation);
            tmp.transform.localScale = Vector3.one * radious;
        }
		Destroy(gameObject);
	}

}
