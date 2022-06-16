using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public GameObject spawning;
    public GameObject spirits;
    public int spiritHave;
    public int spiritHaveMax;

    public UI gameScript;


    public Vector3 center;
    public Vector3 size;
    public float timer = 5.0f;
    public float timeToSpawn = 5.0f;
    public float spawnCount = 0f;
    public Vector3 volume;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameScript = GameObject.Find("Main Camera").GetComponent<UI>();
        timer = 1000f;
        SpawnSpirits();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameScript.gameState == 1)
        { SpawnHandMonster(); }



    }

    void SpawnSpirits()
    {
        Vector3 pos = new Vector3(Random.Range(player.position.x - volume.x, player.position.x + volume.x), 0.0f, Random.Range(player.position.z - volume.z, player.position.z + volume.z));
        if (spiritHave < spiritHaveMax && gameScript.gameLevel == 0)
        {
            GameObject objS = Instantiate(spirits, center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0f, Random.Range(-size.z / 2, size.z / 2)), Quaternion.identity);
            spiritHave += 1;
        }
        //center = new Vector3(player.position.x, 0f, player.position.z);
        //objS.transform.position = center + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0f, Random.Range(-size.z / 2, size.z / 2));
        
        Invoke(nameof(SpawnSpirits), 10f);
    }

    void SpawnHandMonster()
    {
        if (!Input.GetKey("w") && gameScript.gameLevel == 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && spawnCount < 4f)
            {
                spawnCount += 1f;
                Vector3 pos = new Vector3(Random.Range(player.position.x - volume.x, player.position.x + volume.x), 0.0f, Random.Range(player.position.z - volume.z, player.position.z + volume.z));
                GameObject obj = Instantiate(spawning, pos, player.rotation);
                //obj.transform.parent = null;
                timer = timeToSpawn;
            }

        }
        if (Input.GetKey("w"))
        {
            timer = timeToSpawn;

        }
    }

}
