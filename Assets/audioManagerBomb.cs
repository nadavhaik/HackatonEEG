using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManagerBomb : MonoBehaviour
{
    public AudioSource audiosource;

    void Update(){
        if (!audiosource.isPlaying){
            Destroy(gameObject);
        }
    }
}
