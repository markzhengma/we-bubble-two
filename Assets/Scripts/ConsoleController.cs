using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    public GameObject Milk;
    public GameObject Tapioca;
    public GameObject Console;
    public GameObject ConsoleClone;
    public Transform ConsoleLocation;
    public Material White;
    public Material Black;
    public List<string> objectsOnP1Floor = new List<string>();
    public List<string> objectsOnP2Floor = new List<string>();
    public string[] conditions;
    public string[] functions;
    public string[] paramList;
    public string currentCond;
    public string currentFunc;
    public string nots;
    public string param;
    public bool isGeneratingConsoles = false;
    public bool isDestroyedConsole = false;
    public int conIndex;
    public int funIndex;

    GameObject P1Cup;
    GameObject P2Cup;
    // Start is called before the first frame update
    void Start()
    {
        conditions = new string[3] {"Score < 40", "OnFloor", "OnFloor"};
        functions = new string[4] {"FasterScore()", "SlowerScore()", "TapiocaIntrude()", "Milkify()"};
        paramList = new string[8] {"milk", "ice", "taro", "banana", "sb", "tp", "jl", "pd"};
        // functions = new string[4] {"One", "Two", "Three", "Four"};
    }

    void Update(){
        currentCond = conditions[conIndex];
        currentFunc = functions[funIndex];
        if(isGeneratingConsoles){
            isGeneratingConsoles = false;
            StartCoroutine(GenerateConsoles());
        }
        if(isDestroyedConsole){
            // nots = ConsoleClone.GetComponent<ConsoleIdentifier>().nots;
            // param = ConsoleClone.GetComponent<ConsoleIdentifier>().param;
            isDestroyedConsole = false;
            CheckConditions(nots, param);
        }
    }
    void CheckConditions(string nots, string param){
        Debug.Log("param: " + param);
        int numOfNots = 0;
        for(var i = 0; i < nots.Length; i ++){
            if(nots[i].ToString() == "!"){
                numOfNots += 1;
            }
        }

        if(numOfNots % 2 == 0){
            switch (conIndex){
                case 0:
                    if(gameObject.GetComponent<ScoreCounter>().ScoreP1 < 40){
                        SelectedFunction(1);
                    }
                    if(gameObject.GetComponent<ScoreCounter>().ScoreP2 < 40){
                        SelectedFunction(2);
                    }
                    break;
                case 1:
                    if(objectsOnP1Floor.Contains(param)){
                        SelectedFunction(1);
                    }
                    if(objectsOnP2Floor.Contains(param)){
                        SelectedFunction(2);
                    }
                    break;
                case 2:
                    if(objectsOnP1Floor.Contains(param)){
                        SelectedFunction(1);
                    }
                    if(objectsOnP2Floor.Contains(param)){
                        SelectedFunction(2);
                    }
                    break;
                case 3:
                    for(var i = 0; i < GameObject.FindGameObjectsWithTag("PickUp").Length; i ++){
                        if(GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().type == "cup" && GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().playerNum == 1){
                            P1Cup = GameObject.FindGameObjectsWithTag("PickUp")[i];
                        }
                        if(GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().type == "cup" && GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().playerNum == 2){
                            P2Cup = GameObject.FindGameObjectsWithTag("PickUp")[i];
                        }
                    }
                    if(P1Cup.GetComponent<CupContent>().flavor == param || P1Cup.GetComponent<CupContent>().topping == param){
                        SelectedFunction(1);
                    }
                    if(P1Cup.GetComponent<CupContent>().hasMilk && param == "milk"){
                        SelectedFunction(1);
                    }
                    if(P1Cup.GetComponent<CupContent>().hasIce && param == "ice"){
                        SelectedFunction(1);
                    }

                    if(P2Cup.GetComponent<CupContent>().flavor == param || P2Cup.GetComponent<CupContent>().topping == param){
                        SelectedFunction(2);
                    }

                    if(P2Cup.GetComponent<CupContent>().hasMilk && param == "milk"){
                        SelectedFunction(2);
                    }
                    if(P2Cup.GetComponent<CupContent>().hasIce && param == "ice"){
                        SelectedFunction(2);
                    }

                    break;
                default:
                    break;
            }
        }else{
            switch (conIndex){
                case 0:
                    if(gameObject.GetComponent<ScoreCounter>().ScoreP1 >= 40){
                        SelectedFunction(1);
                    }
                    if(gameObject.GetComponent<ScoreCounter>().ScoreP2 >= 40){
                        SelectedFunction(2);
                    }
                    break;
                case 1:
                    if(!objectsOnP1Floor.Contains(param)){
                        SelectedFunction(1);
                    }
                    if(!objectsOnP2Floor.Contains(param)){
                        SelectedFunction(2);
                    }
                    break;
                case 2:
                    if(!objectsOnP1Floor.Contains(param)){
                        SelectedFunction(1);
                    }
                    if(!objectsOnP2Floor.Contains(param)){
                        SelectedFunction(2);
                    }
                    break;
                case 3:
                    for(var i = 0; i < GameObject.FindGameObjectsWithTag("PickUp").Length; i ++){
                        if(GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().type == "cup" && GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().playerNum == 1){
                            P1Cup = GameObject.FindGameObjectsWithTag("PickUp")[i];
                        }
                        if(GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().type == "cup" && GameObject.FindGameObjectsWithTag("PickUp")[i].GetComponent<PickUpIdentifier>().playerNum == 2){
                            P2Cup = GameObject.FindGameObjectsWithTag("PickUp")[i];
                        }
                    }
                    if(P1Cup.GetComponent<CupContent>().flavor != param && P1Cup.GetComponent<CupContent>().topping != param){
                        if(param == "milk" && !P1Cup.GetComponent<CupContent>().hasMilk){
                            SelectedFunction(1);
                        }
                        if(param == "ice" && !P1Cup.GetComponent<CupContent>().hasIce){
                            SelectedFunction(1);
                        }
                        SelectedFunction(1);
                    }

                    if(P2Cup.GetComponent<CupContent>().flavor != param && P2Cup.GetComponent<CupContent>().topping != param){
                        if(param == "milk" && !P2Cup.GetComponent<CupContent>().hasMilk){
                            SelectedFunction(2);
                        }
                        if(param == "ice" && !P2Cup.GetComponent<CupContent>().hasIce){
                            SelectedFunction(2);
                        }
                        SelectedFunction(2);
                    }
                    break;
                default:
                    break;
            }
        }

    }

    void SelectedFunction(int playerNum){
        switch(funIndex){
            case 0:
                StartCoroutine(FasterScore(playerNum));
                // StartCoroutine(TapiocaIntrusion(playerNum));
                break;
            case 1:
                StartCoroutine(SlowerScore(playerNum));
                // StartCoroutine(TapiocaIntrusion(playerNum));
                break;
            case 2:
                StartCoroutine(TapiocaIntrusion(playerNum));
                break;
            case 3:
                Milkify(playerNum);
                // StartCoroutine(TapiocaIntrusion(playerNum));
                break;
            default:
                break;
        }
    }

    IEnumerator FasterScore(int playerNum){
        if(playerNum == 1){
            gameObject.GetComponent<ScoreCounter>().P1Increment += 20;
            Debug.Log("+20 increment for P " + playerNum);
            yield return new WaitForSeconds(30);
            gameObject.GetComponent<ScoreCounter>().P1Increment -= 20;
        }else if(playerNum == 2){
            gameObject.GetComponent<ScoreCounter>().P2Increment += 20;
            Debug.Log("+20 increment for P " + playerNum);
            yield return new WaitForSeconds(30);
            gameObject.GetComponent<ScoreCounter>().P2Increment -= 20;
        }
    }
    IEnumerator SlowerScore(int playerNum){
        if(playerNum == 1){
            gameObject.GetComponent<ScoreCounter>().P1Increment -= 10;
            Debug.Log("-10 score increment for P " + playerNum);
            yield return new WaitForSeconds(30);
            gameObject.GetComponent<ScoreCounter>().P1Increment += 10;
        }else if(playerNum == 2){
            gameObject.GetComponent<ScoreCounter>().P2Increment -= 10;
            Debug.Log("-10 score increment for P " + playerNum);
            yield return new WaitForSeconds(30);
            gameObject.GetComponent<ScoreCounter>().P2Increment += 10;
        }
    }
    IEnumerator TapiocaIntrusion(int playerNum){
        Debug.Log("Tapioca for P" + playerNum);
        Transform spawnPoint = gameObject.transform.Find("FloorP" + playerNum +"/SpawnPoint");
        if(playerNum == 1){
            objectsOnP1Floor.Add("tp");
        }else if(playerNum == 2){
            objectsOnP2Floor.Add("tp");
        }
        for(var i = 0; i < 10; i ++){
            GameObject TapiocaClone = Instantiate(Tapioca, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity) as GameObject;
            Collider[] childCol = TapiocaClone.GetComponentsInChildren<Collider>();
            Rigidbody[] childRb = TapiocaClone.GetComponentsInChildren<Rigidbody>();
            Renderer[] childRend = TapiocaClone.GetComponentsInChildren<Renderer>();
            for(var j = 0; j < childCol.Length; j ++){
                childCol[j].enabled = true;
                childRb[j].isKinematic = false;
                childRb[j].useGravity = true;
                childRend[j].material = Black;
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
    void Milkify(int playerNum){
        Debug.Log("Milkify with P " + playerNum);
        Transform LiquidPoint = gameObject.transform.Find("FloorP" + playerNum +"/LiquidPoint");
        GameObject MilkClone = Instantiate(Milk, new Vector3(LiquidPoint.position.x, LiquidPoint.position.y, LiquidPoint.position.z), Quaternion.identity) as GameObject;
        Renderer[] childRend = MilkClone.GetComponentsInChildren<Renderer>();
        for(var i = 0; i < childRend.Length; i ++){
            childRend[i].material = White;
        }
    }

    IEnumerator GenerateConsoles(){
        if(!gameObject.GetComponent<ScoreCounter>().isEnded){
            yield return new WaitForSeconds(20);
            ConsoleClone = Instantiate(Console, new Vector3(ConsoleLocation.transform.position.x, ConsoleLocation.transform.position.y, ConsoleLocation.transform.position.z), Quaternion.identity) as GameObject;
            yield return new WaitForSeconds(15);
            isGeneratingConsoles = true;
        }
    }
}
