using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] TutorialList;
    public GameObject BGImage;
    public GameObject LastBtn;
    public GameObject NextBtn;
    public GameObject SkipBtn;
    public GameObject StartBtn;
    public bool skipped = false;
    public int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(TutorialList.Length > 0){
            TutorialList[currentIndex].SetActive(true);
        }
        BGImage.SetActive(true);
        LastBtn.SetActive(true);
        NextBtn.SetActive(true);
        StartBtn.SetActive(false);
        SkipBtn.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentIndex == TutorialList.Length - 1){
            StartBtn.SetActive(true);
            SkipBtn.SetActive(false);
        }
        if(currentIndex == 0){
            StartBtn.SetActive(false);
            SkipBtn.SetActive(true);
        }
        if(skipped){
            LastBtn.SetActive(false);
            NextBtn.SetActive(false);
            StartBtn.SetActive(false);
            SkipBtn.SetActive(false);
            BGImage.SetActive(false);
        }
    }

    public void NextTutorial(){
        if(currentIndex < TutorialList.Length - 1){
            currentIndex += 1;
            TutorialList[currentIndex - 1].SetActive(false);
            TutorialList[currentIndex].SetActive(true);
        }
    }

    public void LastTutorial(){
        if(currentIndex > 0){
            currentIndex -= 1;
            TutorialList[currentIndex + 1].SetActive(false);
            TutorialList[currentIndex].SetActive(true);
        }
    }

    public void SkipTutorial(){
        TutorialList[currentIndex].SetActive(false);
        skipped = true;
    }
}
