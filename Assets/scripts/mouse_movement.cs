using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_movement : MonoBehaviour
{
 
    Camera cam;
    
    void Start(){
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        Vector3 t = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 40));
        transform.position = t;
    }

    // void OnTriggerEnter3D(Collider collision)
    // {
    //     Debug.Log("Hello");
    // }
    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("Hello2");
    // }

}