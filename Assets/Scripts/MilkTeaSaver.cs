using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkTeaSaver : MonoBehaviour
{
    public int DeliveryId;
    public bool hasMilkTea = false;
    public bool hasIce;
    public bool hasMilk;
    public string flavor;
    public string topping;
    public GameObject DeliveredMilkTea;
    void Start()
    {
        
    }

    
    void Update()
    {

    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "PickUp"){
            if(col.gameObject.GetComponent<PickUpIdentifier>().type == "cup"){
                DeliveredMilkTea = col.gameObject;
                col.gameObject.GetComponent<PickUpIdentifier>().isDelivered = true;
                hasMilkTea = true;
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                col.gameObject.GetComponent<Rigidbody>().useGravity = false;

                Collider[] childCol = col.gameObject.GetComponentsInChildren<Collider>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = false;
                }
                col.gameObject.transform.position = transform.position;
                col.gameObject.transform.rotation = transform.rotation;
                col.gameObject.transform.parent = transform;

                hasIce = col.gameObject.GetComponent<CupContent>().hasIce;
                hasMilk = col.gameObject.GetComponent<CupContent>().hasMilk;
                flavor = col.gameObject.GetComponent<CupContent>().flavor;
                topping = col.gameObject.GetComponent<CupContent>().topping;
            }
        }
    }

    public IEnumerator DestroyMilkTea(){
        hasMilkTea = false;
        hasIce = false;
        hasMilk = false;
        flavor = "";
        topping = "";
        yield return new WaitForSeconds(2);
        Destroy(DeliveredMilkTea);
    }
}
