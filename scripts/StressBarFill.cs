using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressBarFill : MonoBehaviour
{
    [SerializeField] private Slider s;
    [SerializeField] private GameObject[] statuses;
    private Env global_env;
    private StressData sdb;
    private bool changed;


    // Start is called before the first frame update
    void Start()
    {
        changed = false;
        global_env = GameObject.Find("global_env").GetComponent<Env>();
        sdb = StressData.GetInstance();
        s.maxValue = global_env.max_stress_level;
    }

    // Update is called once per frame
    void Update()
    {
        s.value = (float)sdb.getStressLevel();
        setColor();
    }

    void setColor(){
        Vector2 size = s.fillRect.sizeDelta;
        Vector2 pixelPivot;
        
        if (s.value < s.maxValue/5f)
            pixelPivot = statuses[0].gameObject.GetComponent<Image>().sprite.pivot;
            // s.fillRect = statuses[0];

        else if (s.value < s.maxValue/2f)
            pixelPivot = statuses[1].gameObject.GetComponent<Image>().sprite.pivot;
            // s.fillRect = statuses[1];
        else
            pixelPivot = statuses[2].gameObject.GetComponent<Image>().sprite.pivot;
            // s.fillRect = statuses[2];
        foreach (GameObject g in statuses) changeActive(g);
        Vector2 percentPivot = new Vector2(pixelPivot.x / size.x, pixelPivot.y / size.y);
        s.fillRect.pivot = percentPivot;
    }

    void changeActive(GameObject g){
        if (g.activeSelf) g.SetActive(false);
        else g.SetActive(true);
    }
}
