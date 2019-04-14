using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
public class PlayerMovement : MonoBehaviour
{
    public bool isMobile = false;
    public float speed = 3.0F;
    public int playerNum = 0;
    public GameObject ScoreCounter;
    Vector3 movement;
    Animator anim;
    void Start()
    {
        movement = new Vector3(0f, 0f, 0f);
        anim = this.GetComponent<Animator>();
        ScoreCounter = GameObject.Find( "Environment" );
    }

    // Update is called once per frame
    void Update()
    {
        if(!ScoreCounter.GetComponent<ScoreCounter>().isEnded){
            float h = 0;
            float v = 0;
            if(playerNum == 1){
                if(isMobile){
                    h = CrossPlatformInputManager.GetAxis ("Horizontal_p1");
                    v = CrossPlatformInputManager.GetAxis ("Vertical_p1");
                }else{
                    h = Input.GetAxis("Horizontal_p1");
                    v = Input.GetAxis("Vertical_p1");
                }
                movement = new Vector3(h, 0f, v) * speed * Time.deltaTime;
                transform.Translate(movement, Space.World);
                if(movement != Vector3.zero){
                    transform.rotation = Quaternion.LookRotation(movement);
                }
            }else if(playerNum == 2){
                if(isMobile){
                    h = CrossPlatformInputManager.GetAxis ("Horizontal_p2");
                    v = CrossPlatformInputManager.GetAxis ("Vertical_p2");
                }else{
                    h = Input.GetAxis("Horizontal_p2");
                    v = Input.GetAxis("Vertical_p2");
                }
                movement = new Vector3(h, 0f, v) * speed * Time.deltaTime;
                transform.Translate(movement, Space.World);
                if(movement != Vector3.zero){
                    transform.rotation = Quaternion.LookRotation(movement);
                }
            }
            if(h != 0 || v != 0){
                anim.SetBool("isRunning",true);
            }else{
                anim.SetBool("isRunning", false);
            }
        }else{
            if(Input.GetKeyDown(KeyCode.C)){
                P1Ready();
            }
            if(Input.GetKeyDown(KeyCode.N)){
                P2Ready();
            }
        }
    }

    public void P1Ready(){
        if(ScoreCounter.GetComponent<ScoreCounter>().isEnded){
            ScoreCounter.GetComponent<ScoreCounter>().isP1Ready = true;
        }
    }

    public void P2Ready(){
        if(ScoreCounter.GetComponent<ScoreCounter>().isEnded){
            ScoreCounter.GetComponent<ScoreCounter>().isP2Ready = true;
        }
    }

    void OnTriggerStay(Collider col){
        if(col.gameObject.tag == "SpilledLiquid"){
            if(col.gameObject.GetComponent<Renderer>().material.name == "white (Instance)"){
                speed = 5.0f;
            }else{
                speed = 1.5f;   
            }
        }
    }
    void OnTriggerExit(Collider col){
        if(col.gameObject.tag == "SpilledLiquid"){
            speed = 3.0f;
        }
    }
}
