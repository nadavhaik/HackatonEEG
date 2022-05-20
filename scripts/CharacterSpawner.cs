using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] float summon_rate = 2f;
    private Env global_env;
    private StressData sdb;
        
    void Start()
    {
        sdb = StressData.GetInstance();

        global_env = GameObject.Find("global_env").GetComponent<Env>();
        last_time_spawned_character = Time.realtimeSinceStartup;
    }

    float last_time_spawned_character;
    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - last_time_spawned_character > (50/summon_rate)){
            last_time_spawned_character = Time.realtimeSinceStartup;

            float shouldSpawn = Random.Range(0,1f);
            Vector3 spawnPos =  new Vector3 (0, -global_env.cameraHeight , global_env.cameraOffset);
            GameObject instChar = (shouldSpawn < 0.5f) ? Object.Instantiate(characters[0], spawnPos, new Quaternion(0,0,0,0))
                                                    : Object.Instantiate(characters[1], spawnPos, new Quaternion(0,0,0,0));
            
            Vector3 randomDirection = new Vector3(Random.Range(-10, 10), global_env.throwForce);
            instChar.GetComponent<Rigidbody>().AddForce(randomDirection, ForceMode.Impulse);
            
        }
    }

    
}
