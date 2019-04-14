using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpIdentifier : MonoBehaviour
{
    public string type;
    public string subType;
    public bool isDropped;
    public bool isDelivered;
    private bool isHit = false;
    private bool isSpilling;
    private string spilledString;
    public int playerNum;
    public GameObject droppedTopping;
    public GameObject droppedIce;
    public GameObject spilledLiquid;
    public Material bananaMat;
    public Material sbMat;
    public Material taroMat;
    public Material milkMat;

    public void ChangeMyRigidbody(){
        gameObject.GetComponent<Rigidbody>().isKinematic = !gameObject.GetComponent<Rigidbody>().isKinematic;
        gameObject.GetComponent<Rigidbody>().useGravity = !gameObject.GetComponent<Rigidbody>().useGravity;
    }

    void Start(){
        isDropped = false;
    }

    void Update(){
        if(type == "cup" && gameObject.GetComponent<Rigidbody>().isKinematic == false){
            Transform CupTopTransform = gameObject.transform.Find("Outside/CupTop");
            Transform CupBotTransform = gameObject.transform.Find("Outside/CupBot");
            if(!isDropped && CupTopTransform.position.y - CupBotTransform.position.y <= 0.2){
                isDropped = true;

                if(gameObject.GetComponent<CupContent>().topping != ""){
                    GameObject droppedToppingClone = Instantiate(droppedTopping, new Vector3(CupTopTransform.position.x, CupTopTransform.position.y, CupTopTransform.position.z), Quaternion.identity) as GameObject;
                    Collider[] childCol = droppedToppingClone.GetComponentsInChildren<Collider>();
                    Rigidbody[] childRb = droppedToppingClone.GetComponentsInChildren<Rigidbody>();
                    Renderer[] childRend = droppedToppingClone.GetComponentsInChildren<Renderer>();
                    for(var i = 0; i < childCol.Length; i ++){
                        childCol[i].enabled = true;
                        childRb[i].isKinematic = false;
                        childRb[i].useGravity = true;
                        childRend[i].material = gameObject.transform.Find("Outside/Inside/Toppings/Sphere").gameObject.GetComponent<Renderer>().material;
                    }
                }

                if(gameObject.GetComponent<CupContent>().hasIce){
                    GameObject droppedIceClone = Instantiate(droppedIce, new Vector3(CupTopTransform.position.x, CupTopTransform.position.y, CupTopTransform.position.z), Quaternion.identity) as GameObject;
                    Collider[] childCol = droppedIceClone.GetComponentsInChildren<Collider>();
                    Rigidbody[] childRb = droppedIceClone.GetComponentsInChildren<Rigidbody>();
                    for(var i = 0; i < childCol.Length; i ++){
                        childCol[i].enabled = true;
                        childRb[i].isKinematic = false;
                        childRb[i].useGravity = true;
                    }
                }

                if(gameObject.GetComponent<CupContent>().flavor != "" || gameObject.GetComponent<CupContent>().hasMilk){
                    isSpilling = true;
                    if(gameObject.GetComponent<CupContent>().flavor != ""){
                        spilledString = gameObject.GetComponent<CupContent>().flavor;
                    }else if(gameObject.GetComponent<CupContent>().hasMilk){
                        spilledString = "milk";
                    }
                }

                gameObject.GetComponent<CupContent>().flavor = "";
                gameObject.GetComponent<CupContent>().hasIce = false;
                gameObject.GetComponent<CupContent>().topping = "";
                gameObject.GetComponent<CupContent>().hasMilk = false;
                gameObject.GetComponent<Animator>().SetBool("isFill", false);
                gameObject.GetComponent<Animator>().SetBool("isEmpty", true);
                gameObject.GetComponent<Animator>().SetTrigger("drop");
            }else if(isDropped &&CupTopTransform.position.y - CupBotTransform.position.y > 0.2){
                isDropped = false;
                isSpilling = false;
            }
            // Debug.Log("cuptop: " + CupTopTransform.position.y);
            // Debug.Log("cupbot: " + CupBotTransform.position.y);
        }
        if((type == "flavor" || type == "milk") && gameObject.GetComponent<Rigidbody>().isKinematic == false){
            Transform LiquidTopTransform = gameObject.transform.Find("Box/LiquidTop");
            Transform LiquidBotTransform = gameObject.transform.Find("Box/LiquidBot");
            if(!isDropped && LiquidTopTransform.position.y - LiquidBotTransform.position.y <= 0.2){
                isDropped = true;
                isSpilling = true;
                if(type == "flavor"){
                    spilledString = subType;
                }else if(type == "milk"){
                    spilledString = "milk";
                }
            }else if(isDropped &&LiquidTopTransform.position.y - LiquidBotTransform.position.y > 0.2){
                isDropped = false;
                isSpilling = false;
            }
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.gameObject.tag == "Player"){
            if(isHit == false){
                if(type == "cup" || type == "topping"){
                    GameObject.Find("HitGlassSound").GetComponent<AudioSource>().Play();
                    isHit = true;
                    StartCoroutine(ResetHitSound());
                }
                if(type == "ice" || type == "trash"){
                    GameObject.Find("HitBigSound").GetComponent<AudioSource>().Play();
                    isHit = true;
                    StartCoroutine(ResetHitSound());
                }
                if(type == "spoon"){
                    GameObject.Find("HitSmallSound").GetComponent<AudioSource>().Play();
                    isHit = true;
                    StartCoroutine(ResetHitSound());
                }
                if(type == "flavor" || type == "milk"){
                    GameObject.Find("HitBoxSound").GetComponent<AudioSource>().Play();
                    isHit = true;
                    StartCoroutine(ResetHitSound());
                }
            }
        }
    }

    void OnCollisionStay(Collision col){
        if(col.gameObject.tag == "Floor" && isSpilling){
            if(col.gameObject.name == "FloorP1"){
                GameObject.Find("Environment").GetComponent<ConsoleController>().objectsOnP1Floor.Add(spilledString);
            }
            if(col.gameObject.name == "FloorP2"){
                GameObject.Find("Environment").GetComponent<ConsoleController>().objectsOnP2Floor.Add(spilledString);
            }
            if(spilledString == "taro"){
                SetSpilledMaterial(col, taroMat);
            }else if(spilledString == "banana"){
                SetSpilledMaterial(col, bananaMat);
            }else if(spilledString == "sb"){
                SetSpilledMaterial(col, sbMat);
            }else if(spilledString == "milk"){
                SetSpilledMaterial(col, milkMat);
            }
            isSpilling = false;
        }
    }

    IEnumerator ResetHitSound(){
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

    void SetSpilledMaterial(Collision col, Material mat){
        GameObject spilledLiquidClone = Instantiate(spilledLiquid, new Vector3(col.contacts[0].point.x, col.contacts[0].point.y, col.contacts[0].point.z), Quaternion.identity) as GameObject;
        Renderer[] childRend = spilledLiquidClone.GetComponentsInChildren<Renderer>();
        for(var i = 0; i < childRend.Length; i ++){
            childRend[i].material = mat;
        }
    }
}
