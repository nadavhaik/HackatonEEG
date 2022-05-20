using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text healthCounter;
    [SerializeField] private Text scoreCounter;
    private mouse_movement player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player").GetComponent<mouse_movement>();
    }

    // Update is called once per frame
    public void Update()
    {
        string hp = player.getHp().ToString();
        if (int.Parse(hp) >= 0)
            healthCounter.text = hp;
        scoreCounter.text = player.getScore().ToString();
    }



    public void death(){
        Debug.Log("implement death stuff");
    }
}
