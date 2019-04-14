using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreCounter : MonoBehaviour
{
    public GameObject DeliveryP1;
    public GameObject DeliveryP2;
    public GameObject OrderP1;
    public GameObject OrderP2;
    public GameObject Cup;
    public GameObject NotBlock;
    public GameObject[] OrderP1List;
    public GameObject[] OrderP2List;
    private GameObject P1;
    private GameObject P2;
    private GameObject ReplayBtn;
    private Text ScoreTextP1;
    private Text ScoreTextP2;
    private Text ScoreIncTextP1;
    private Text ScoreIncTextP2;
    private Text TimerMin;
    private Text TimerSec;
    private Text WinText;
    public Transform OrderP1Location;
    public Transform OrderP2Location;
    public Transform CupP1Location;
    public Transform CupP2Location;
    public Transform BGMLocation;
    public Transform BlockP1Location;
    public Transform BlockP2Location;
    public bool isGeneratingOrders = false;
    public int ScoreP1;
    public int ScoreP2;
    public int P1Increment = 20;
    public int P2Increment = 20;
    public int min;
    public int sec;
    public bool isP1Missed = false;
    public bool isP2Missed = false;
    public bool isP1Ready;
    public bool isP2Ready;
    public bool isEnded;
    public string[] flavors;
    public string[] toppings;
    public bool[] hasMilks;
    public bool[] hasIces;

    // Start is called before the first frame update
    void Start()
    {
        isEnded = true;
        isP1Ready = false;
        isP2Ready = false;
        ScoreP1 = 0;
        ScoreP2 = 0;
        ScoreTextP1 = GameObject.Find( "ScoreTextP1" ).GetComponentInChildren<Text>();
        ScoreTextP2 = GameObject.Find( "ScoreTextP2" ).GetComponentInChildren<Text>();
        ScoreIncTextP1 = GameObject.Find( "ScoreIncTextP1" ).GetComponentInChildren<Text>();
        ScoreIncTextP2 = GameObject.Find( "ScoreIncTextP2" ).GetComponentInChildren<Text>();
        TimerMin = GameObject.Find( "TimerMin" ).GetComponentInChildren<Text>();
        TimerSec = GameObject.Find( "TimerSec" ).GetComponentInChildren<Text>();
        WinText = GameObject.Find( "WinText" ).GetComponentInChildren<Text>();
        ReplayBtn = GameObject.Find("ReplayBtn");
        ReplayBtn.SetActive(false);

        for(var i = 0; i < GameObject.FindGameObjectsWithTag("Player").Length; i ++){
            if(GameObject.FindGameObjectsWithTag("Player")[i].GetComponent<PlayerInteraction>().playerNum == 1){
                P1 = GameObject.FindGameObjectsWithTag("Player")[i];
            }else if(GameObject.FindGameObjectsWithTag("Player")[i].GetComponent<PlayerInteraction>().playerNum == 2){
                P2 = GameObject.FindGameObjectsWithTag("Player")[i];
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        OrderP1List = GameObject.FindGameObjectsWithTag("OrderP1");
        OrderP2List = GameObject.FindGameObjectsWithTag("OrderP2");

        if(isGeneratingOrders){
            gameObject.GetComponent<AudioController>().OrderNewSound.Play();
        }
        if(isP1Ready && isP2Ready && isEnded){
            StartCoroutine(StartGame());
        }

        if(ScoreP1 < 10 && ScoreP1 >= 0){
            ScoreTextP1.text = "0" + ScoreP1.ToString();
        }else{
            ScoreTextP1.text = ScoreP1.ToString();
        }
        if(ScoreP2 < 10 && ScoreP2 >= 0){
            ScoreTextP2.text = "0" + ScoreP2.ToString();
        }else{
            ScoreTextP2.text = ScoreP2.ToString();
        }

        if(P1Increment >= 0){
            ScoreIncTextP1.text = "+" + P1Increment.ToString();
        }else{
            ScoreIncTextP1.text = P1Increment.ToString();
        }

        if(P2Increment >= 0){
            ScoreIncTextP2.text = "+" + P2Increment.ToString();
        }else{
            ScoreIncTextP2.text = P2Increment.ToString();
        }

        if(isGeneratingOrders){
            StartCoroutine(GenerateOrders());
        }
        if(DeliveryP1.GetComponent<MilkTeaSaver>().hasMilkTea){
            StartCoroutine(CheckIfMilkTeaMatches(OrderP1List, DeliveryP1, "P1"));
            StartCoroutine(DeliveryP1.GetComponent<MilkTeaSaver>().DestroyMilkTea());
            GameObject CupClone = Instantiate(Cup, new Vector3(CupP1Location.transform.position.x, CupP1Location.transform.position.y, CupP1Location.transform.position.z), Quaternion.identity) as GameObject;
        }
        if(DeliveryP2.GetComponent<MilkTeaSaver>().hasMilkTea){
            StartCoroutine(CheckIfMilkTeaMatches(OrderP2List, DeliveryP2, "P2"));
            StartCoroutine(DeliveryP2.GetComponent<MilkTeaSaver>().DestroyMilkTea());
            GameObject CupClone = Instantiate(Cup, new Vector3(CupP2Location.transform.position.x, CupP2Location.transform.position.y, CupP2Location.transform.position.z), Quaternion.identity) as GameObject;
        }
        if(isP1Missed){
            ScoreP1 -= 10;
            isP1Missed = false;
        }else if(isP2Missed){
            ScoreP2 -= 10;
            isP2Missed = false;
        }
    }

    IEnumerator GenerateOrders(){
        if(!isEnded){
            GameObject OrderP1Clone = Instantiate(OrderP1, new Vector3(OrderP1Location.transform.position.x, OrderP1Location.transform.position.y, OrderP1Location.transform.position.z), Quaternion.identity) as GameObject;
            GameObject OrderP2Clone = Instantiate(OrderP2, new Vector3(OrderP2Location.transform.position.x, OrderP2Location.transform.position.y, OrderP2Location.transform.position.z), Quaternion.identity) as GameObject;
            RandomizeAndSetAtt(OrderP1Clone, OrderP2Clone);
            isGeneratingOrders = false;
            yield return new WaitForSeconds(40);
            isGeneratingOrders = true;
        }
    }

    void RandomizeAndSetAtt(GameObject obj1, GameObject obj2){
        string flavor = flavors[Random.Range(0, 3)];
        string topping = toppings[Random.Range(0, 4)];
        bool hasMilk = hasMilks[Random.Range(0, 2)];
        bool hasIce = hasIces[Random.Range(0, 2)];

        obj1.GetComponent<OrderIdentifier>().flavor = flavor;
        obj1.GetComponent<OrderIdentifier>().topping = topping;
        obj1.GetComponent<OrderIdentifier>().hasMilk = hasMilk;
        obj1.GetComponent<OrderIdentifier>().hasIce = hasIce;
        obj2.GetComponent<OrderIdentifier>().flavor = flavor;
        obj2.GetComponent<OrderIdentifier>().topping = topping;
        obj2.GetComponent<OrderIdentifier>().hasMilk = hasMilk;
        obj2.GetComponent<OrderIdentifier>().hasIce = hasIce;
    }

    IEnumerator CheckIfMilkTeaMatches(GameObject[] OrderList, GameObject Delivery, string id){
        for(var i = 0; i < OrderList.Length; i ++){
            if(OrderList[i].GetComponent<OrderIdentifier>().flavor == Delivery.GetComponent<MilkTeaSaver>().flavor
                && OrderList[i].GetComponent<OrderIdentifier>().topping == Delivery.GetComponent<MilkTeaSaver>().topping
                && OrderList[i].GetComponent<OrderIdentifier>().hasIce == Delivery.GetComponent<MilkTeaSaver>().hasIce
                && OrderList[i].GetComponent<OrderIdentifier>().hasMilk == Delivery.GetComponent<MilkTeaSaver>().hasMilk
            ){
                if(id == "P1"){
                    ScoreP1 += P1Increment;
                }else if(id == "P2"){
                    ScoreP2 += P2Increment;
                }
                gameObject.GetComponent<AudioController>().OrderDoneSound.Play();
                yield return new WaitForSeconds(1);
                Destroy(OrderList[i]);
            // }else if(i == OrderList.Length - 1
            //     && (OrderList[i].GetComponent<OrderIdentifier>().flavor == Delivery.GetComponent<MilkTeaSaver>().flavor
            //         || OrderList[i].GetComponent<OrderIdentifier>().topping == Delivery.GetComponent<MilkTeaSaver>().topping
            //         || OrderList[i].GetComponent<OrderIdentifier>().hasIce == Delivery.GetComponent<MilkTeaSaver>().hasIce
            //         || OrderList[i].GetComponent<OrderIdentifier>().hasMilk == Delivery.GetComponent<MilkTeaSaver>().hasMilk)
            // ){
            //     gameObject.GetComponent<AudioController>().OrderErrorSound.Play();
            }
        }
    }

    IEnumerator StartCountdown(){
		while(min > -1 && !isEnded)
        {
            TimerMin.text = "0" + min.ToString();
            if(sec >= 10)
            {
                TimerSec.text = sec.ToString();
            }else if (sec >= 0 && sec < 10){
                TimerSec.text = "0" + sec.ToString();
            }else if (sec < 0 && min > 0)
            {
                min--;
                TimerMin.text = "0" + min.ToString();
                sec = 59;
            }else if(sec < 0 && min == 0){
                if(ScoreP1 > ScoreP2){
                    WinText.text = "Player 1 Won!";
                    P1.GetComponent<Animator>().SetTrigger("win");
                }else if(ScoreP1 < ScoreP2){
                    WinText.text = "Player 2 Won!";
                    P2.GetComponent<Animator>().SetTrigger("win");
                }else{
                    P1.GetComponent<Animator>().SetTrigger("win");
                    P2.GetComponent<Animator>().SetTrigger("win");
                    WinText.text = "It's A Tie!";
                }
                gameObject.GetComponent<AudioController>().GameFinishSound.Play();
                ReplayBtn.SetActive(true);
                isEnded = true;
            }
            yield return new WaitForSeconds(1.0f);
            sec--;
        }
    }

    public IEnumerator StartGame(){
        isP1Ready = false;
        isP2Ready = false;
        isEnded = false;

        WinText.text = "3";
        yield return new WaitForSeconds(1f);
        WinText.text = "2";
        yield return new WaitForSeconds(1f);
        WinText.text = "1";
        yield return new WaitForSeconds(1f);
        WinText.text = "GO!";
        yield return new WaitForSeconds(1f);
        WinText.text = "";


        isGeneratingOrders = true;
        gameObject.GetComponent<ConsoleController>().isGeneratingConsoles = true;

        int randomNum = Random.Range(0, gameObject.GetComponent<AudioController>().BGMList.Length);
        GameObject BGM = Instantiate(gameObject.GetComponent<AudioController>().BGMList[randomNum].gameObject, new Vector3(BGMLocation.position.x, BGMLocation.position.y, BGMLocation.position.z), Quaternion.identity) as GameObject;
        BGM.GetComponent<AudioSource>().Play();

        GameObject Cup1Clone = Instantiate(Cup, new Vector3(CupP1Location.transform.position.x, CupP1Location.transform.position.y, CupP1Location.transform.position.z), Quaternion.identity) as GameObject;
        Cup1Clone.GetComponent<PickUpIdentifier>().playerNum = 1;
        GameObject Cup2Clone = Instantiate(Cup, new Vector3(CupP2Location.transform.position.x, CupP2Location.transform.position.y, CupP2Location.transform.position.z), Quaternion.identity) as GameObject;
        Cup2Clone.GetComponent<PickUpIdentifier>().playerNum = 2;

        GameObject NotBlock1Clone = Instantiate(NotBlock, new Vector3(BlockP1Location.transform.position.x, BlockP1Location.transform.position.y, BlockP1Location.transform.position.z), Quaternion.identity) as GameObject;
        NotBlock1Clone.GetComponent<PickUpIdentifier>().playerNum = 1;
        GameObject NotBlock2Clone = Instantiate(NotBlock, new Vector3(BlockP2Location.transform.position.x, BlockP2Location.transform.position.y, BlockP2Location.transform.position.z), Quaternion.identity) as GameObject;
        NotBlock2Clone.GetComponent<PickUpIdentifier>().playerNum = 2;

        StartCoroutine(StartCountdown());
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex ) ;
    }
}
