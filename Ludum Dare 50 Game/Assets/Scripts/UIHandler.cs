using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public int StartTime = 30;
    public Text timerText {get; set; }
    public int time;
    public Player player {get; set; }

    public List<GameObject> inOrderHealth;

    int currHealthIndex = 3;
    void Start(){
        timerText = GetComponentInChildren<Text>();
        player = FindObjectOfType<Player>();
        time = StartTime;
        timerText.text = $"{StartTime}";
        currHealthIndex = player.health-1;
        StartCoroutine(timer());
    }



    public void OnHealthLoss(){
        Debug.Log("health loss UI");
        inOrderHealth[currHealthIndex].SetActive(false);
        currHealthIndex = player.health-1;
    }

    // Update is called once per frame
    IEnumerator timer(){
        while(true){
            time--;
            timerText.text = $"{time}";
            yield return new WaitForSeconds(1);
        }
    }
}
