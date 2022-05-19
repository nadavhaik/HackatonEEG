using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] fruits;
    float time;
    [SerializeField] float summon_time = 2f;
    // Start is called before the first frame update
    void Start()
    {
        time = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup - time > summon_time){
            time = Time.realtimeSinceStartup;
            foreach (GameObject fruit in fruits) Object.Instantiate(fruit);
        }
    }
}
