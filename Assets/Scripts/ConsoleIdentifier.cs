using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleIdentifier : MonoBehaviour
{
    public string cond;
    public string func;
    public string nots;
    public string param;
    public string paramTry;
    public string paramFull;
    public string paramTryFull;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Environment").GetComponent<ConsoleController>().conIndex = Random.Range(0, GameObject.Find("Environment").GetComponent<ConsoleController>().conditions.Length);
        GameObject.Find("Environment").GetComponent<ConsoleController>().funIndex = Random.Range(0, GameObject.Find("Environment").GetComponent<ConsoleController>().functions.Length);
        param = GameObject.Find("Environment").GetComponent<ConsoleController>().paramList[Random.Range(0, GameObject.Find("Environment").GetComponent<ConsoleController>().paramList.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        cond = GameObject.Find("Environment").GetComponent<ConsoleController>().currentCond;
        func = GameObject.Find("Environment").GetComponent<ConsoleController>().currentFunc;

        gameObject.transform.Find("Box/ConsolePlane/ParamTry/ParamTryText").gameObject.GetComponent<TextMesh>().text = paramTryFull;

        if(GameObject.Find("Environment").GetComponent<ConsoleController>().conIndex != 0){
            gameObject.transform.Find("Box/ConsolePlane/ConditionText").gameObject.GetComponent<TextMesh>().text = "(" + nots + paramFull + cond + ")";
        }else{
            gameObject.transform.Find("Box/ConsolePlane/ConditionText").gameObject.GetComponent<TextMesh>().text = "(" + nots + cond + ")";
        }
            gameObject.transform.Find("Box/ConsolePlane/FunctionText").gameObject.GetComponent<TextMesh>().text = func;
        
        switch(param){
            case "sb":
                paramFull = "strawberry";
                break;
            case "pd":
                paramFull = "pudding";
                break;
            case "jl":
                paramFull = "jelly";
                break;
            case "tp":
                paramFull = "tapioca";
                break;
            default:
                paramFull = param;
                break;
        }

        switch(paramTry){
            case "sb":
                paramTryFull = "strawberry";
                break;
            case "pd":
                paramTryFull = "pudding";
                break;
            case "jl":
                paramTryFull = "jelly";
                break;
            case "tp":
                paramTryFull = "tapioca";
                break;
            default:
                paramTryFull = paramTry;
                break;
        }
    }

    void RunFunction(){
        GameObject.Find("Environment").GetComponent<ConsoleController>().isDestroyedConsole = true;
        GameObject.Find("Environment").GetComponent<ConsoleController>().nots = nots;
        GameObject.Find("Environment").GetComponent<ConsoleController>().param = param;
        GameObject.Find("Environment").GetComponent<AudioController>().ConsoleRunSound.Play();
        Destroy(gameObject);
    }
    
}
