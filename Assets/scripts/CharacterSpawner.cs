using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    [SerializeField] mouse_movement player;
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
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players == null) return;
        else player = players[0].GetComponent<mouse_movement>();
        if (Time.realtimeSinceStartup - last_time_spawned_character > 1 - Mathf.Min(0.7f, player.temp/global_env.max_stress_level)){
            last_time_spawned_character = Time.realtimeSinceStartup;
            Debug.Log(player.temp/global_env.max_stress_level);
            float shouldSpawn = Random.Range(0,1f);
            Vector3 spawnPos =  new Vector3 (0, -global_env.cameraHeight , global_env.cameraOffset);
            GameObject instChar = (shouldSpawn < Mathf.Min(0.5f + player.temp/global_env.max_stress_level, 0.7f)) ? Object.Instantiate(characters[0], spawnPos, new Quaternion(0,0,0,0))
                                                    : Object.Instantiate(characters[1], spawnPos, new Quaternion(0,0,0,0));
            
            Vector3 randomDirection = new Vector3(Random.Range(-10, 10), global_env.throwForce);
            instChar.GetComponent<Rigidbody>().AddForce(randomDirection, ForceMode.Impulse);
            
        }
    }

    
}
