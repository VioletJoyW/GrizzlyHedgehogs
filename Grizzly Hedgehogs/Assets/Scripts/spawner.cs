using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] GameObject mapMarker;
    [SerializeField] bool updatesGameGoal;
    [SerializeField] bool randomizedObjects;
    [SerializeField] bool randomizedPos;

    List<GameObject> spawnedObjects = new List<GameObject>();

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    int numberPos = -1;
    int numberObject = -1;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.addSpawner(this);

        if (updatesGameGoal)
        {
            gameManager.instance.updateGameGoal(numberToSpawn);
        }

        if(mapMarker != null)
        {
            mapMarker.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numberToSpawn)
        {
            StartCoroutine(Spawn());

            if (mapMarker != null)
            {
                mapMarker.SetActive(false);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

	/// <summary>
	/// Spawns the object.
	/// </summary>
	/// <returns></returns>
	IEnumerator Spawn()
    {
        isSpawning = true;

        if (randomizedObjects)
        {
            numberObject = Random.Range(0, objectToSpawn.Length);
        }
        else 
        {
            numberObject++;
            if (numberObject >= objectToSpawn.Length)
            {
                numberObject = 0;
            }
        }

        if (randomizedPos)
        {
            numberPos = Random.Range(0, spawnPos.Length);
        }
        else
        {
            numberPos++;
            if (numberPos >= spawnPos.Length)
            {
                numberObject = 0;
            }
        }

        GameObject objectClone = Instantiate(objectToSpawn[numberObject], spawnPos[numberPos].position, spawnPos[numberObject].transform.rotation);

        spawnedObjects.Add(objectClone);

        if (objectClone.CompareTag("Enemy") || objectClone.CompareTag("Enemy Sniper"))
        {
            objectClone.GetComponent<EnemyAI>().SetFromSpawner(true);
        }

        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }

    public void resetSpawn()
    {
        for(int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            Destroy(spawnedObjects[i]);
            spawnedObjects.RemoveAt(i);
        }

        if (updatesGameGoal)
        {
            gameManager.instance.updateGameGoal(numberToSpawn);
        }

        spawnCount = 0;
        numberObject = -1;
        numberPos = -1;

        isSpawning = false;
        startSpawning = false;
    }

}
