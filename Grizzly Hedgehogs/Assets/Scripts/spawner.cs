using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numberToSpawn;
    [SerializeField] int timeBetweenSpawns;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;
    bool isSpawning;
    bool startSpawning;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.updateGameGoal(numberToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && !isSpawning && spawnCount < numberToSpawn)
        {
            StartCoroutine(spawn());
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;

        int randomEne = Random.Range(0, objectToSpawn.Length);
        int randomNum = Random.Range(0, spawnPos.Length);

        if (objectToSpawn[randomEne].CompareTag("Enemy")) 
        {
           objectToSpawn[randomEne].GetComponent<enemyAI>().SetCanAddToGoal(true);

		}

        Instantiate(objectToSpawn[randomEne], spawnPos[randomNum].position, spawnPos[randomNum].rotation);
        spawnCount++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }
}
