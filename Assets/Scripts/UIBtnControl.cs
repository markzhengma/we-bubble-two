using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBtnControl : MonoBehaviour
{
    public Button P1InteractBtn;
    public Button P2InteractBtn;
    public Button P1DropBtn;
    public Button P2DropBtn;
    public GameObject P1;
    public GameObject P2;
    GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        for(var i = 0; i < players.Length; i ++){
            if(players[i].GetComponent<PlayerInteraction>().playerNum == 1){
                P1 = players[i];
            }
            if(players[i].GetComponent<PlayerInteraction>().playerNum == 2){
                P2 = players[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(P1 != null && P2 != null){
            if(GameObject.Find( "Environment").GetComponent<ScoreCounter>().isEnded){
                P1InteractBtn.onClick.AddListener(P1.GetComponent<PlayerMovement>().P1Ready);
                P2InteractBtn.onClick.AddListener(P2.GetComponent<PlayerMovement>().P2Ready);
            }else{
                P1InteractBtn.onClick.AddListener(P1.GetComponent<PlayerInteraction>().InteractAndPickUp);
                P2InteractBtn.onClick.AddListener(P2.GetComponent<PlayerInteraction>().InteractAndPickUp);
                P1DropBtn.onClick.AddListener(P1.GetComponent<PlayerInteraction>().DropDownObject);
                P2DropBtn.onClick.AddListener(P2.GetComponent<PlayerInteraction>().DropDownObject);
            }
        }
    }
}
