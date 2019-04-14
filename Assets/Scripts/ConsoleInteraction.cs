using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleInteraction : MonoBehaviour
{
    public GameObject Console;
    public GameObject NotBlock;
    public Transform BlockLocation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "PickUp"){
            if(col.gameObject.GetComponent<PickUpIdentifier>().type == "notBlock"){
                col.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                col.gameObject.GetComponent<Rigidbody>().useGravity = false;
                Collider[] childCol = col.gameObject.GetComponentsInChildren<Collider>();
                for(var i = 0; i < childCol.Length; i ++){
                    childCol[i].enabled = false;
                }
                col.gameObject.transform.position = BlockLocation.position;
                col.gameObject.transform.rotation = BlockLocation.rotation;
                col.gameObject.transform.parent = BlockLocation;

                col.gameObject.GetComponent<Animator>().enabled = true;
                gameObject.GetComponent<Animator>().SetTrigger("change");
                GameObject.Find("Environment").GetComponent<AudioController>().ConsoleChangeSound.Play();
                
                GameObject BlockP1Location = GameObject.Find("BlockP1Location");
                GameObject BlockP2Location = GameObject.Find("BlockP2Location");
                if(col.gameObject.GetComponent<PickUpIdentifier>().playerNum == 1){
                    GameObject NotBlock1Clone = Instantiate(NotBlock, new Vector3(BlockP1Location.transform.position.x, BlockP1Location.transform.position.y, BlockP1Location.transform.position.z), Quaternion.identity) as GameObject;
                    NotBlock1Clone.GetComponent<PickUpIdentifier>().playerNum = 1;
                }else if(col.gameObject.GetComponent<PickUpIdentifier>().playerNum == 2){
                    GameObject NotBlock2Clone = Instantiate(NotBlock, new Vector3(BlockP2Location.transform.position.x, BlockP2Location.transform.position.y, BlockP2Location.transform.position.z), Quaternion.identity) as GameObject;
                    NotBlock2Clone.GetComponent<PickUpIdentifier>().playerNum = 2;
                }

                StartCoroutine(DestroyAfter2Second(col.gameObject));
            }
        }
    }
    IEnumerator DestroyAfter2Second(GameObject obj){
        yield return new WaitForSeconds(1);
        Console.GetComponent<ConsoleIdentifier>().nots += "! ";
        if(Console.GetComponent<ConsoleIdentifier>().nots.Length == 3){
            Console.GetComponent<ConsoleIdentifier>().nots += "/n";
        }
        Destroy(obj);
    }
}
