using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupContent : MonoBehaviour
{
    public bool hasIce = false;
    public string flavor = "";
    public string topping = "";
    public bool hasMilk = false;
    public GameObject Content;
    public GameObject Milk;
    public GameObject Ice;
    public GameObject Toppings;
    public Material Purple;
    public Material Yellow;
    public Material Red;
    public Material White;
    public Material Black;
    public Material JellyWhite;
    public Material Transparent;
    public Material IceColor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (flavor){
            case "taro":
                Content.GetComponent<Renderer>().material = Purple;
                break;
            case "banana":
                Content.GetComponent<Renderer>().material = Yellow;
                break;
            case "sb":
                Content.GetComponent<Renderer>().material = Red;
                break;
            default:
                Content.GetComponent<Renderer>().material = Transparent;
                break;
        }

        if(hasMilk){
            Milk.GetComponent<Renderer>().material = White;
        }else{
            Milk.GetComponent<Renderer>().material = Transparent;
        }

        if(hasIce){
            Renderer[] iceChildRend = Ice.GetComponentsInChildren<Renderer>();
            for(var i = 0; i < iceChildRend.Length; i ++){
                iceChildRend[i].material = IceColor;
            }
        }else{
            Renderer[] iceChildRend = Ice.GetComponentsInChildren<Renderer>();
            for(var i = 0; i < iceChildRend.Length; i ++){
                iceChildRend[i].material = Transparent;
            }
        }

        Renderer[] childRend = Toppings.GetComponentsInChildren<Renderer>();
        switch (topping){
            case "tp":
                for(var i = 0; i < childRend.Length; i ++){
                    childRend[i].material = Black;
                }
                break;
            case "jl":
                for(var i = 0; i < childRend.Length; i ++){
                    childRend[i].material = JellyWhite;
                }
                break;
            case "pd":
                for(var i = 0; i < childRend.Length; i ++){
                    childRend[i].material = Yellow;
                }
                break;
            default:
                for(var i = 0; i < childRend.Length; i ++){
                    childRend[i].material = Transparent;
                }
                break;
        }
    }

    public void resetCupContent(){
        Content.GetComponent<Renderer>().material = Transparent;
        Milk.GetComponent<Renderer>().material = Transparent;
        Renderer[] iceChildRend = Ice.GetComponentsInChildren<Renderer>();
        for(var i = 0; i < iceChildRend.Length; i ++){
            iceChildRend[i].material = Transparent;
        }
    }
}
