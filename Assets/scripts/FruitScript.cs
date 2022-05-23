using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitScript : MonoBehaviour
{
    private Env global_env;
   
    private float rotacionRandom;
    private float rotacionPorSegundos;

    public GameObject top;
    public GameObject bottom;

    public GameObject bombSoundEffect;
    public GameObject fruitSoundEffect;


    [SerializeField] mouse_movement player;

    private void OnEnable()
    {
        rotacionRandom = Random.Range(-1, 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        global_env = GameObject.Find("global_env").GetComponent<Env>();
    }

    void Update(){
        this.transform.Rotate(new Vector3(0, 0, rotacionRandom));
        if (transform.position.y < global_env.cameraBottom)
            death();
    }

    void death(){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        mouse_movement player = (players != null) ? players[0].GetComponent<mouse_movement>() : null;
        if (player != null && gameObject.tag == "Fruit")
            player.takeDamage();
        Destroy(gameObject);
    }
    float random(){
        return Random.Range(-1f,1f);
        }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameObject.tag == "Fruit")
        {
            top.transform.parent = null;
            bottom.transform.parent = null;
            top.GetComponent<Rigidbody>().useGravity = true;
            bottom.GetComponent<Rigidbody>().useGravity = true;
            top.GetComponent<Rigidbody>().isKinematic = false;
            bottom.GetComponent<Rigidbody>().isKinematic = false;
            if(top.transform.position.x < bottom.transform.position.x)
            {
                if(top.transform.position.y < bottom.transform.position.y)
                {
                    top.GetComponent<Rigidbody>().AddForce(new Vector3(-1,-1,0) * 7f, ForceMode.Impulse);
                    bottom.GetComponent<Rigidbody>().AddForce(new Vector3(1, 1, 0) * 7f, ForceMode.Impulse);
                }
                else
                {
                    top.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 1, 0) * 7f, ForceMode.Impulse);
                    bottom.GetComponent<Rigidbody>().AddForce(new Vector3(1, -1, 0) * 7f, ForceMode.Impulse);
                }
            }
            else
            {
                if (top.transform.position.y < bottom.transform.position.y)
                {
                    top.GetComponent<Rigidbody>().AddForce(new Vector3(1, -1, 0) * 7f, ForceMode.Impulse);
                    bottom.GetComponent<Rigidbody>().AddForce(new Vector3(-1, 1, 0) * 7f, ForceMode.Impulse);
                }
                else
                {
                    top.GetComponent<Rigidbody>().AddForce(new Vector3(1, 1, 0) * 7f, ForceMode.Impulse);
                    bottom.GetComponent<Rigidbody>().AddForce(new Vector3(-1, -1, 0) * 7f, ForceMode.Impulse);
                }
            }
            Destroy(top, 5);
            Destroy(bottom, 5);
            GameObject.Instantiate(fruitSoundEffect);
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Player" && gameObject.tag == "Bomb"){
            GameObject.Instantiate(bombSoundEffect);
            Destroy(gameObject);
        }
        
    }




}
