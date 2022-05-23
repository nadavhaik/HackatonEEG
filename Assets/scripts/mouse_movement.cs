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
        // move();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            takeDamage();
        }
        if (other.gameObject.tag == "Fruit"){
            score += 5;
        }
    }


    public void takeDamage(){
        hp--;
        if (hp < 0)
            die();
    }

    void addScore(){
        score += 5;
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

    public float f = 10f;
    public float temp;
    void move(){
        StressData s = StressData.GetInstance();
        // double d = (double)s.getStressLevel();
        // if (d != null){
        //     if (Input.GetAxis("Horizontal") > 0)
        //         s.SetStressGauge(d + f);
        //     else if (Input.GetAxis("Horizontal") < 0)
        //         s.SetStressGauge(d - f);
        // }
        float d = Input.mouseScrollDelta.y;
        temp += d*f;
        if (temp < 0) temp = 0;
        if (temp > global_env.max_stress_level) temp = (float)global_env.max_stress_level;
    }

}