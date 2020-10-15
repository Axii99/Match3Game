using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandBookMenu : MonoBehaviour
{

    public HandBook[] allentries;
    public Image display_icon;
    public TMPro.TMP_Text display_name;
    public TMPro.TMP_Text display_descrip;

    private GameObject display_panel;
    private GameObject option_panel;
    // Start is called before the first frame update
    void Start()
    {
        display_panel = this.transform.Find("DisplayPanel").gameObject;
        option_panel = this.transform.Find("options").gameObject;

        display_panel.SetActive(false);


        display_icon.sprite = allentries[0].artwork;
        display_name.text = allentries[0].name;
        display_descrip.text = allentries[0].description;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void display(int index) {
        display_icon.sprite = allentries[index].artwork;
        display_name.text = allentries[index].name;
        display_descrip.text = allentries[index].description;
        option_panel.SetActive(false);
        display_panel.SetActive(true);
    }

    public void BacktoOption() {
        display_panel.SetActive(false);
        option_panel.SetActive(true);
    }

    public void CloseBook() {
        this.gameObject.SetActive(false);
    }
}
