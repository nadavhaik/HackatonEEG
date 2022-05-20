using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public Vector2 mousePosition;
    public Vector2 mousePosition2;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition2 = Input.mousePosition;
    }
}
