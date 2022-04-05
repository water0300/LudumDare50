using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject lossUI;
    public GameObject winUI;
    public GameObject shopUI;
    public Text timerText;
    public Text coinText;
    public int time;
    public Player player {get; set; }

    public List<GameObject> inOrderHealth;

    int currHealthIndex = 3;
    void Start(){
        player = FindObjectOfType<Player>();
        time = player.startTime;
        timerText.text = $"{player.time}";
        coinText.text = $"{player.coins}";
        currHealthIndex = player.health-1;
        StartCoroutine(timer());
    }

    public void OnShopEnter(){
        shopUI.SetActive(true);
    }

    public void OnShopExit(){
        shopUI.SetActive(false);
    }

    public void OnCoinAdjustment(){
        coinText.text = $"{player.coins}";
    }

    public void OnHealthLoss(){
        Debug.Log("health loss UI");
        inOrderHealth[currHealthIndex].SetActive(false);
        currHealthIndex = player.health-1;
    }

    public void OnHeal(){
        if(currHealthIndex+1 != inOrderHealth.Count){
            inOrderHealth[currHealthIndex+1].SetActive(true);
            currHealthIndex = player.health-1;
        }
    }

    public void OnTimeIncrease(){
        timerText.text = $"{player.time}";

    }

    // Update is called once per frame
    IEnumerator timer(){
        while(true){
            if(player.time == 0){
                Destroy(player.gameObject);
                EndGame();
            }
            player.time--;
            timerText.text = $"{player.time}";
            yield return new WaitForSeconds(1);
        }
    }

    public void EndGame(){
        lossUI.SetActive(true);
    }

    public void WinGame(){
        winUI.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene(1);
    }

    public void Quit(){
        Application.Quit();
    }
}
