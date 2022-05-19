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
        float x = random() * global_env.cameraWidth;
        float y = global_env.cameraHeight;
        transform.position = new Vector3 (x,y, global_env.cameraOffset);
        float [] randos = new float[3];
        for (int i = 0; i < randos.Length; i++) randos[i] = random() * global_env.rotationScaler;
        transform.rotation = new Quaternion(randos[0],randos[1],randos[2],0); 
    }

    void Update(){
        GetComponent<Rigidbody>().AddForce(new Vector3(1, 1, 0) * force, ForceMode.Impulse);
    }


    float random(){
        return Random.Range(-1f,1f);
        }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
