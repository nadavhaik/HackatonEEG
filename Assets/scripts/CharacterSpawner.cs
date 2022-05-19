using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] characters;
    float time;
    [SerializeField] float summon_time = 2f;
    private Env global_env;

    void Start()
    {
        global_env = GameObject.Find("global_env").GetComponent<Env>();
        time = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - time > summon_time){
            time = Time.realtimeSinceStartup;

            float shouldSpawn = Random.Range(0,1f);
            Vector3 spawnPos =  new Vector3 (0, -global_env.cameraHeight , global_env.cameraOffset);
            GameObject instChar;
            if (shouldSpawn < 0.5f)
                instChar = Object.Instantiate(characters[0], spawnPos, new Quaternion(0,0,0,0)); 
            else
                instChar = Object.Instantiate(characters[1], spawnPos, new Quaternion(0,0,0,0));
            Vector3 randomDirection = new Vector3(Random.Range(-10, 10), global_env.throwForce);
            instChar.GetComponent<Rigidbody>().AddForce(randomDirection, ForceMode.Impulse);
            
        }
    }
}