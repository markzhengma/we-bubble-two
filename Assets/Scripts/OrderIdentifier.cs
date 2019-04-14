using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderIdentifier : MonoBehaviour
{
    public string flavor;
    public string topping;
    public bool hasMilk;
    public bool hasIce;
    public Renderer flavorRenderer;
    public Renderer toppingRenderer;
    public Renderer milkRenderer;
    public Renderer iceRenderer;

    public Material taro;
    public Material banana;
    public Material sb;
    public Material pd;
    public Material jl;
    public Material tp;
    public Material ice;
    public Material noIce;
    public Material milk;
    public Material noMilk;
    public Material nothing;
    private GameObject ScoreCounter;
    
    void Start(){
        ScoreCounter = GameObject.Find("Environment");
    }
    void Update()
    {
        switch(flavor){
            case "taro":
                flavorRenderer.material = taro;
                break;
            case "banana":
                flavorRenderer.material = banana;
                break;
            case "sb":
                flavorRenderer.material = sb;
                break;
            default:
                flavorRenderer.material = nothing;
                break;
        };
        switch(topping){
            case "tp":
                toppingRenderer.material = tp;
                break;
            case "jl":
                toppingRenderer.material = jl;
                break;
            case "pd":
                toppingRenderer.material = pd;
                break;
            case "":
                toppingRenderer.material = nothing;
                break;
            default:
                toppingRenderer.material = nothing;
                break;
        };
        if(hasMilk){
            milkRenderer.material = milk;
        }else{
            milkRenderer.material = noMilk;
        };
        if(hasIce){
            iceRenderer.material = ice;
        }else{
            iceRenderer.material = noIce;
        }
    }

    public void DestroyOrder(){
        if(gameObject.tag == "OrderP1"){
            ScoreCounter.GetComponent<ScoreCounter>().isP1Missed = true;
        }else if(gameObject.tag == "OrderP2"){
            ScoreCounter.GetComponent<ScoreCounter>().isP2Missed = true;
        }
        GameObject.Find("OrderErrorSound").GetComponent<AudioSource>().Play();
        Destroy(gameObject);
    }
}
