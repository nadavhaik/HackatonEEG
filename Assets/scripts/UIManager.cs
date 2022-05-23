using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text healthCounter;
    [SerializeField] private Text scoreCounter;
    [SerializeField] private GameObject healthScoreBar;
    [SerializeField] private GameObject stressMeter;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mainMenuyouSurePrompt;
    [SerializeField] private GameObject loseMenuyouSurePrompt;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private Slider s;


    [SerializeField] private mouse_movement playerPrefab;    
    private mouse_movement player;    
    [SerializeField] private CharacterSpawner spawnerPrefab;    
    private CharacterSpawner spawner;    


    // Start is called before the first frame update
    void Start()
    {
        healthScoreBar.SetActive(false);
        stressMeter.SetActive(false);
        mainMenuyouSurePrompt.SetActive(false);
        loseMenuyouSurePrompt.SetActive(false);
        loseMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Update is called once per frame
    public void Update()
    {
        if (player != null){
            string hp = player.getHp().ToString();
            if (int.Parse(hp) >= 0)
                healthCounter.text = hp;
            scoreCounter.text = player.getScore().ToString();
            s.value = (float)StressData.GetInstance().GetStressGauge();
            // s.value = (float)StressData.GetInstance().getStressLevel();
            // s.value = player.temp;
        }
    }



    public void death(){
        Destroy(player.gameObject);
        Destroy(spawner.gameObject);

        setActive(stressMeter);
        setActive(loseMenu);
    }


    public void PlayButton(){
        player = Instantiate(playerPrefab);
        spawner = Instantiate(spawnerPrefab);
        setActive(healthScoreBar);
        setActive(stressMeter);
        setActive(mainMenu);
    }

    public void MainMenuQuitButton(){
        setActive(mainMenu);
        setActive(mainMenuyouSurePrompt);
    }

    public void LoseMenuQuitButton(){
        setActive(loseMenu);
        setActive(loseMenuyouSurePrompt);
    }

    public void MainMenuNotSureButton(){
        MainMenuQuitButton();
    }

    public void playAgain(){
        player = Instantiate(playerPrefab);
        spawner = Instantiate(spawnerPrefab);
        setActive(stressMeter);
        setActive(loseMenu);
    }
    
    public void LoseMenuNotSureButton(){
        LoseMenuQuitButton();
    }

    public void SureButton(){
        Application.Quit();
    }

    public void setActive(GameObject g){
        if(g.active)
            g.SetActive(false);
        else
            g.SetActive(true);
    }

}
