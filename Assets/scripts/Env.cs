using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env : MonoBehaviour
{
    public float cameraOffset = 40f;
    public float cameraHeight;
    public float cameraWidth;
    public float screenAspect;
    public float rotationScaler; 

    void Start(){
        cameraHeight = Camera.main.orthographicSize * 4;

        screenAspect = (float)Screen.width / (float)Screen.height;

        cameraWidth = cameraHeight * screenAspect;

        rotationScaler = 10f;
    }

    
    
}
