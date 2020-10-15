using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLevelMenu : MonoBehaviour
{
    public Sprite lockSprite;
    public Button[] levelButtons;

    private void Awake() {
        //PlayerPrefs.DeleteAll();
    }

    // Start is called before the first frame update
    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 3);
        for (int i = 0; i< levelButtons.Length; i++) {
            if (i >= levelReached-1) {
                levelButtons[i].interactable = false;
                levelButtons[i].image.sprite = lockSprite;
                levelButtons[i].transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "";
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void backToMainMenu() {
        LevelLoader.Instance.LoadLevelName("Menu");
    }

    public void loadLevelNo(int index) {
        LevelLoader.Instance.LoadLevelIndex(index);
    }

    public void loadLevelName(string name) {
        LevelLoader.Instance.LoadLevelName(name);
    }

}
