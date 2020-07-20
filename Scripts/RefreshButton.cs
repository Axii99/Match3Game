using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RefreshButton : MonoBehaviour
{
    public float RefreshCoolDownTime = 5f;
    private float timeLeft;
    public Text CountText;
    private bool flag;
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = RefreshCoolDownTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag) {

            timeLeft -= Time.deltaTime;
            //timeLeft = Mathf.Round(timeLeft);
            //Debug.Log(timeLeft);
            CountText.text = Mathf.Round(timeLeft).ToString();
        }
    }

    public void RefreshCoolDown() {
        this.GetComponent<Button>().interactable = false;
        flag = true;

        Invoke("RefreshReset", RefreshCoolDownTime);
        
    }



    public void RefreshReset() {
        this.GetComponent<Button>().interactable = true;
        flag = false;
        timeLeft = RefreshCoolDownTime;
        CountText.text = "";
    }

}
