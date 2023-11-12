using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class gunStats: MonoBehaviour
{

    [Header("----- Components -----")]
    [SerializeField] Transform shootPos;
    [SerializeField] Collider damageCollider;

    [Header("----- Gun Stats -----")]
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
}
