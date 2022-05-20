using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    private Env global_env;
   
    public float force;

    // Start is called before the first frame update
    void Start()
    {
        global_env = GameObject.Find("global_env").GetComponent<Env>();
    }

    void Update(){
        if (transform.position.y < global_env.cameraBottom)
            Destroy(gameObject);
    }


    float random(){
        return Random.Range(-1f,1f);
        }


    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "Player")
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
