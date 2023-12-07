using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] ParticleSystem explosionEffect;


    // Start is called before the first frame update
    void Start()
    {
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);

        Destroy(gameObject, 0.1f);
    }



	void OnTriggerEnter(Collider other)
	{
        if (other.isTrigger) return;

        IDamage damagable = other.GetComponent<IDamage>();
        if (damagable != null)
        {
            damagable.TakeDamage(damage);
        }
		
	}
}
