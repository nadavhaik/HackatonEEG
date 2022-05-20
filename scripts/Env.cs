using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env : MonoBehaviour
{
    public float cameraOffset = 40f;
    public float rotationScaler = 10f; 
    public float throwForce = 26f; 
    public float max_stress_level = 20f;
    public float cameraHeight;
    public float cameraBottom;
    public float cameraWidth;
    public float screenAspect;
    
    void Start(){
        cameraHeight = Camera.main.orthographicSize * 4;

        cameraBottom = -2f * cameraHeight;

        screenAspect = (float)Screen.width / (float)Screen.height;

        cameraWidth = cameraHeight * screenAspect;

    }

    
}
