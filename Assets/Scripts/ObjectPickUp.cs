using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickUp : MonoBehaviour
{
    bool isNearPickUp;
    public GameObject player;
    public Material Transparent;
    public Material PickUpMaterial;
    public Material CollidedMaterial;
    // Start is called before the first frame update
    void Start()
    {
        // isNearPickUp = player.GetComponent<PlayerInteraction>().isNearPickUp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnTriggerEnter(Collider col){
    //     if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == false){
    //         player.GetComponent<PlayerInteraction>().isNearPickUp = true;
    //         player.GetComponent<PlayerInteraction>().pickUp = col.gameObject;
    //     }else if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == true){
    //         player.GetComponent<PlayerInteraction>().isNearPickUp = true;
    //         player.GetComponent<PlayerInteraction>().collidedPickUp = col.gameObject;
    //     }
    // }

    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == false){
            player.GetComponent<PlayerInteraction>().isNearPickUp = false;
            player.GetComponent<PlayerInteraction>().pickUp = null;
            EnableAndDisableHighlights(col.gameObject, "PickUpIndicator", Transparent);
        }else if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == true){
            player.GetComponent<PlayerInteraction>().isNearPickUp = false;
            player.GetComponent<PlayerInteraction>().collidedPickUp = null;
            EnableAndDisableHighlights(col.gameObject, "PickUpIndicator", Transparent);
        }
        if(col.gameObject.tag == "Console"){
            player.GetComponent<PlayerInteraction>().isNearConsole = false;
            player.GetComponent<PlayerInteraction>().collidedConsole = null;
            col.gameObject.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().paramTry = "";
        }
    }

    void OnTriggerStay(Collider col){
        // if(col.gameObject.tag == "Delivery" && player.GetComponent<PlayerInteraction>().hasPickUp == false){
        //     player.GetComponent<PlayerInteraction>().isNearPickUp = false;
        //     player.GetComponent<PlayerInteraction>().pickUp = null;
        // }
        // if(col.gameObject.tag == "PickUp" && col.gameObject.GetComponent<PickUpIdentifier>().isDelivered == true){
        //     player.GetComponent<PlayerInteraction>().isNearPickUp = false;
        //     player.GetComponent<PlayerInteraction>().pickUp = null;
        // }
        if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == false){
            if(col.gameObject.GetComponent<PickUpIdentifier>().isDelivered == false){
                player.GetComponent<PlayerInteraction>().isNearPickUp = true;
                player.GetComponent<PlayerInteraction>().pickUp = col.gameObject;
                EnableAndDisableHighlights(col.gameObject, "PickUpIndicator", PickUpMaterial);
            }
        }else if(col.gameObject.tag == "PickUp" && player.GetComponent<PlayerInteraction>().hasPickUp == true){
            if(col.gameObject.GetComponent<PickUpIdentifier>().isDelivered == false){
                player.GetComponent<PlayerInteraction>().isNearPickUp = true;
                player.GetComponent<PlayerInteraction>().collidedPickUp = col.gameObject;
                EnableAndDisableHighlights(col.gameObject, "PickUpIndicator", CollidedMaterial);
            }
        }
        // if(col.gameObject.tag == "Console" && player.GetComponent<PlayerInteraction>().hasPickUp == true){
        //     player.GetComponent<PlayerInteraction>().isNearConsole = true;
        //     player.GetComponent<PlayerInteraction>().collidedConsole = col.gameObject;
        // }
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Console" && player.GetComponent<PlayerInteraction>().hasPickUp == true){
            player.GetComponent<PlayerInteraction>().isNearConsole = true;
            player.GetComponent<PlayerInteraction>().collidedConsole = col.gameObject;
        }
    }

    void EnableAndDisableHighlights(GameObject obj, string indicatorName, Material mat){
        Renderer[] rs = obj.GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs){
            if(r.gameObject.tag == indicatorName){
                Material m = r.material;
                m = mat;
                r.material = m;
            }
        }
    }
}
