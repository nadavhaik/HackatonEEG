using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_movement : MonoBehaviour
{
 
    Camera cam;
    private Env global_env;

    void Start(){
        global_env = GameObject.Find("global_env").GetComponent<Env>();

        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1")){
            gameObject.GetComponent<TrailRenderer>().enabled = true; 
            Vector3 t = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 40));
            transform.position = t;
        }
        else{
            gameObject.GetComponent<TrailRenderer>().enabled = false; 
            transform.position = new Vector3(9999,9999,global_env.cameraOffset);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fruit")
        {
            Destroy(other.gameObject);
        }
    }



}