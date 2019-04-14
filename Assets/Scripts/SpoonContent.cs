using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoonContent : MonoBehaviour
{
    public string content = "";
    public bool hasIce = false;
    public bool hasTopping = false;
    public GameObject topping;
    public GameObject ice;
    public Material black;
    public Material yellow;
    public Material white;
    public GameObject contentLocation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasTopping && !hasIce){
            if(content != "" && content != "ice"){
                GameObject toppingClone = Instantiate(topping, new Vector3(contentLocation.transform.position.x, contentLocation.transform.position.y, contentLocation.transform.position.z), Quaternion.identity) as GameObject;
                Collider[] childCol = toppingClone.GetComponentsInChildren<Collider>();
                Rigidbody[] childRb = toppingClone.GetComponentsInChildren<Rigidbody>();
                Renderer[] childRend = toppingClone.GetComponentsInChildren<Renderer>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = false;
                    childRb[i].isKinematic = true;
                    childRb[i].useGravity = false;
                }
                toppingClone.transform.parent = contentLocation.transform;
                hasTopping = true;
                if(content == "tp"){
                    for(var i = 0; i < childCol.Length; i ++){
                        childRend[i].material = black;
                    }
                }else if(content == "pd"){
                    for(var i = 0; i < childCol.Length; i ++){
                        childRend[i].material = yellow;
                    }
                }else if(content == "jl"){
                    for(var i = 0; i < childCol.Length; i ++){
                        childRend[i].material = white;
                    }
                }
            }else if(content == "ice"){
                GameObject iceClone = Instantiate(ice, new Vector3(contentLocation.transform.position.x, contentLocation.transform.position.y, contentLocation.transform.position.z), Quaternion.identity) as GameObject;
                Collider[] childCol = iceClone.GetComponentsInChildren<Collider>();
                Rigidbody[] childRb = iceClone.GetComponentsInChildren<Rigidbody>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = false;
                    childRb[i].isKinematic = true;
                    childRb[i].useGravity = false;
                }
                iceClone.transform.parent = contentLocation.transform;
                hasIce = true;
            }
        }else{
            if(content == ""){
                Collider[] childCol = contentLocation.GetComponentsInChildren<Collider>();
                Rigidbody[] childRb = contentLocation.GetComponentsInChildren<Rigidbody>();
                Renderer[] childRend = contentLocation.GetComponentsInChildren<Renderer>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = true;
                    childRb[i].isKinematic = false;
                    childRb[i].useGravity = true;
                }
                contentLocation.transform.GetChild(0).parent = null;
                hasTopping = false;
                hasIce = false;
            }
        }
    }

    public void destroyContent(){
        Destroy(contentLocation.transform.GetChild(0).gameObject);
        content = "";
        hasTopping = false;
        hasIce = false;
    }
}
