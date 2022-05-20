using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse_movement : MonoBehaviour
{
 
    Camera cam;
    private Env global_env;
    private UIManager ui_manager;
    [SerializeField] private int hp = 5;
    private int score = 0;

    public float test = 0f;

    void Start(){
        global_env = GameObject.Find("global_env").GetComponent<Env>();
        ui_manager = GameObject.Find("ui_manager").GetComponent<UIManager>();

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
        if (test < 1)
            test += 0.01f;
        else
            test = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            takeDamage();
        }
        if (other.gameObject.tag == "Fruit"){
            addScore();
            Destroy(other.gameObject);
        }
    }


    void takeDamage(){
        hp--;
        if (hp < 0)
            die();
    }

    void addScore(){
        score++;
    }

    void die(){
        ui_manager.death();
        Destroy(gameObject);
    }


    public int getHp(){
        return hp;
    }

    public int getScore(){
        return score;
    }

}