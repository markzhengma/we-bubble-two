using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public int playerNum = 0;
    public bool isNearPickUp = false;
    public bool hasPickUp = false;
    public bool isNearConsole = false;
    public Transform objectPosition;
    public GameObject pickUp;
    public GameObject collidedPickUp;
    public GameObject AudioController;
    public GameObject collidedConsole;
    Animator anim;

	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	void Update () {
        if(playerNum == 1){
            if(Input.GetKeyDown(KeyCode.C)){
                InteractAndPickUp();
            }
            if(Input.GetKeyDown(KeyCode.X)){
                DropDownObject();
            }
            if(pickUp == null){
                collidedPickUp = null;
            }
            // --------- p2 control -------------
        }else if(playerNum == 2){
            if(Input.GetKeyDown(KeyCode.N)){
                InteractAndPickUp();
            }
            if(Input.GetKeyDown(KeyCode.M)){
                DropDownObject();
            }
            if(pickUp == null){
                collidedPickUp = null;
            }
        // ---------- p2 --------------
        }

        if(isNearConsole && pickUp != null && collidedConsole != null){
            string content = "";
            if(pickUp.GetComponent<PickUpIdentifier>().type == "flavor" || pickUp.GetComponent<PickUpIdentifier>().type == "topping"){
                content = pickUp.GetComponent<PickUpIdentifier>().subType;
                collidedConsole.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().paramTry = content;
            }else if(pickUp.GetComponent<PickUpIdentifier>().type == "ice" || pickUp.GetComponent<PickUpIdentifier>().type == "milk"){
                content = pickUp.GetComponent<PickUpIdentifier>().type;
                collidedConsole.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().paramTry = content;
            }
        }
        
    }

    public void InteractAndPickUp(){
        if(isNearPickUp && hasPickUp == false){
            PickUpObject();
        }else if(isNearPickUp && hasPickUp == true){
            InteractWithObject();
        }
        if(isNearConsole && pickUp != null && collidedConsole != null){
            collidedConsole.GetComponent<Animator>().SetTrigger("shake");
            GameObject.Find("Environment").GetComponent<AudioController>().ConsoleChangeSound.Play();
            if(collidedConsole.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().paramTry != ""){
                collidedConsole.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().param = collidedConsole.transform.parent.gameObject.GetComponent<ConsoleIdentifier>().paramTry;
            }
        }
    }

    void PickUpObject(){
        if(pickUp.GetComponent<PickUpIdentifier>().type == "cup" || pickUp.GetComponent<PickUpIdentifier>().type == "milk" || pickUp.GetComponent<PickUpIdentifier>().type == "flavor"){
            pickUp.GetComponent<PickUpIdentifier>().isDropped = false;
        }
        pickUp.GetComponent<Rigidbody>().isKinematic = true;
        pickUp.GetComponent<Rigidbody>().useGravity = false;
        BoxCollider[] childCol = pickUp.GetComponentsInChildren<BoxCollider>();
        for(var i = 0; i < childCol.Length; i ++){
            childCol[i].enabled = false;
        }
        pickUp.transform.position = objectPosition.position;
        pickUp.transform.rotation = objectPosition.rotation;
        pickUp.transform.parent = objectPosition;
        hasPickUp = true;
        anim.SetBool("isHolding", true);
    }

    public void DropDownObject(){
        if(hasPickUp == true){
            if(pickUp.GetComponent<PickUpIdentifier>().isDelivered){
                pickUp = null;
            }else{
                pickUp.transform.parent = null;
                pickUp.GetComponent<Rigidbody>().isKinematic = false;
                pickUp.GetComponent<Rigidbody>().useGravity = true;
                BoxCollider[] childCol = pickUp.GetComponentsInChildren<BoxCollider>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = true;
                }
                if(pickUp.GetComponent<PickUpIdentifier>().type == "spoon" && pickUp.GetComponent<SpoonContent>().content != ""){
                    if(pickUp.GetComponent<PickUpIdentifier>().playerNum == 1){
                        GameObject.Find("Environment").GetComponent<ConsoleController>().objectsOnP1Floor.Add(pickUp.GetComponent<SpoonContent>().content);
                    }
                    if(pickUp.GetComponent<PickUpIdentifier>().playerNum == 2){
                        GameObject.Find("Environment").GetComponent<ConsoleController>().objectsOnP2Floor.Add(pickUp.GetComponent<SpoonContent>().content);
                    }
                    pickUp.GetComponent<SpoonContent>().content = "";
                }
            }
            hasPickUp = false;
            anim.SetBool("isHolding", false);
        }
    }

    void ChangeCollidedPickUpRigidbody(){
        collidedPickUp.GetComponent<Rigidbody>().isKinematic = !collidedPickUp.GetComponent<Rigidbody>().isKinematic;
        collidedPickUp.GetComponent<Rigidbody>().useGravity = !collidedPickUp.GetComponent<Rigidbody>().useGravity;
    }

    void StartToFill(GameObject go, GameObject goOther){
        go.GetComponent<Animator>().SetBool("isFill", true);
        go.GetComponent<Animator>().SetBool("isEmpty", false);
        if(goOther.GetComponent<PickUpIdentifier>().type == "flavor"){
            go.GetComponent<Animator>().SetTrigger("startFill");
        }else if(goOther.GetComponent<PickUpIdentifier>().type == "milk"){
            go.GetComponent<Animator>().SetTrigger("startMilk");
        }
    }

    public void DisableMovement(){
        gameObject.GetComponent<PlayerMovement>().enabled = false;
    }

    IEnumerator DisableAndEnableMovement(){
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        anim.SetBool("isRunning", false);
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    void InteractWithObject(){
        if(collidedPickUp != null && pickUp != null){
            if(pickUp.GetComponent<PickUpIdentifier>().type == "cup" && pickUp.GetComponent<PickUpIdentifier>().isDelivered == false){
                if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "flavor"){
                    if(pickUp.GetComponent<CupContent>().flavor == "" && !collidedPickUp.GetComponent<PickUpIdentifier>().isDropped){
                        pickUp.GetComponent<CupContent>().flavor = collidedPickUp.GetComponent<PickUpIdentifier>().subType;
                        StartToFill(pickUp, collidedPickUp);
                        ChangeCollidedPickUpRigidbody();
                        collidedPickUp.GetComponent<Animator>().SetTrigger("Pour");
                        GameObject.Find("PourLiquidSound").GetComponent<AudioSource>().Play();
                        StartCoroutine(DisableAndEnableMovement());
                    }
                }else if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "milk"){
                    if(pickUp.GetComponent<CupContent>().hasMilk == false && !collidedPickUp.GetComponent<PickUpIdentifier>().isDropped){
                        pickUp.GetComponent<CupContent>().hasMilk = true;
                        ChangeCollidedPickUpRigidbody();
                        collidedPickUp.GetComponent<Animator>().SetTrigger("Pour");
                        GameObject.Find("PourLiquidSound").GetComponent<AudioSource>().Play();
                        StartToFill(pickUp, collidedPickUp);
                        StartCoroutine(DisableAndEnableMovement());
                    }
                }
            }
            if(pickUp.GetComponent<PickUpIdentifier>().type == "spoon"){
                if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "topping"){
                    if(pickUp.GetComponent<SpoonContent>().content == "" && pickUp.GetComponent<SpoonContent>().hasIce == false){
                        pickUp.GetComponent<SpoonContent>().content = collidedPickUp.GetComponent<PickUpIdentifier>().subType;
                        pickUp.GetComponent<Animator>().SetTrigger("get");
                        collidedPickUp.GetComponent<Animator>().SetTrigger("open");
                        GameObject.Find("OpenToppingSound").GetComponent<AudioSource>().Play();
                        GameObject.Find("GetToppingSound").GetComponent<AudioSource>().Play();
                        StartCoroutine(DisableAndEnableMovement());
                    }
                }
                if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "cup"){
                    if(!collidedPickUp.GetComponent<PickUpIdentifier>().isDropped){
                        if(pickUp.GetComponent<SpoonContent>().content != ""){
                            if(collidedPickUp.GetComponent<CupContent>().topping == "" && pickUp.GetComponent<SpoonContent>().content != "ice"){
                                collidedPickUp.GetComponent<CupContent>().topping = pickUp.GetComponent<SpoonContent>().content;
                                pickUp.GetComponent<Animator>().SetTrigger("mix");
                                GameObject.Find("PourToppingSound").GetComponent<AudioSource>().Play();
                                StartCoroutine(DisableAndEnableMovement());
                            }
                            if(!collidedPickUp.GetComponent<CupContent>().hasIce && pickUp.GetComponent<SpoonContent>().content == "ice"){
                                collidedPickUp.GetComponent<CupContent>().hasIce = true;
                                pickUp.GetComponent<Animator>().SetTrigger("mix");
                                GameObject.Find("PourIceSound").GetComponent<AudioSource>().Play();
                                StartCoroutine(DisableAndEnableMovement());
                            }
                        }
                    }
                }
                if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "ice"){
                    if(pickUp.GetComponent<SpoonContent>().content == "" && pickUp.GetComponent<SpoonContent>().hasIce == false){
                        pickUp.GetComponent<SpoonContent>().content = "ice";
                        pickUp.GetComponent<Animator>().SetTrigger("get");
                        collidedPickUp.GetComponent<Animator>().SetTrigger("open");
                        GameObject.Find("GetIceSound").GetComponent<AudioSource>().Play();
                        StartCoroutine(DisableAndEnableMovement());
                    }
                }
            }
            if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "cup"){
                if(!collidedPickUp.GetComponent<PickUpIdentifier>().isDropped){
                    if(pickUp.GetComponent<PickUpIdentifier>().type == "flavor"){
                        if(collidedPickUp.GetComponent<CupContent>().flavor == ""){
                            collidedPickUp.GetComponent<CupContent>().flavor = pickUp.GetComponent<PickUpIdentifier>().subType;
                            StartToFill(collidedPickUp, pickUp);
                            pickUp.GetComponent<Animator>().SetTrigger("HoldingPour");
                            GameObject.Find("PourLiquidSound").GetComponent<AudioSource>().Play();
                            StartCoroutine(DisableAndEnableMovement());
                        }
                    }else if(pickUp.GetComponent<PickUpIdentifier>().type == "milk"){
                        if(collidedPickUp.GetComponent<CupContent>().hasMilk == false){
                            collidedPickUp.GetComponent<CupContent>().hasMilk = true;
                            pickUp.GetComponent<Animator>().SetTrigger("HoldingPour");
                            GameObject.Find("PourLiquidSound").GetComponent<AudioSource>().Play();
                            StartToFill(collidedPickUp, pickUp);
                            StartCoroutine(DisableAndEnableMovement());
                        }
                    }
                }
            }
            if(collidedPickUp.GetComponent<PickUpIdentifier>().type == "trash"){
                if(pickUp.GetComponent<PickUpIdentifier>().type == "cup"){
                    pickUp.GetComponent<CupContent>().flavor = "";
                    pickUp.GetComponent<CupContent>().hasIce = false;
                    pickUp.GetComponent<CupContent>().topping = "";
                    pickUp.GetComponent<CupContent>().hasMilk = false;
                    pickUp.GetComponent<Animator>().SetBool("isFill", false);
                    pickUp.GetComponent<Animator>().SetBool("isEmpty", true);
                    pickUp.GetComponent<Animator>().SetTrigger("empty");
                    GameObject.Find("DumpSound").GetComponent<AudioSource>().Play();
                    StartCoroutine(DisableAndEnableMovement());
                }
            }
        }
    }
}
