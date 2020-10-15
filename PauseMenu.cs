using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{

    public static bool GamePause = false;
    public int nextlevel;
    public GameObject PauseMenuUI;
    public GameObject WinMenuUI;
    public GameObject LoseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        GamePause = false;
    }


    private void OnLevelWasLoaded(int level) {
        GamePause = false;
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GamePause) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void setPause() {
        GamePause = true;
        //Time.timeScale = 0f;
    }

    public void Resume() {
        GameObject dialogueCanvas = GameObject.Find("DialogueCanvas");
        if (dialogueCanvas != null && dialogueCanvas.activeSelf == true) {
            return;
        }
        Debug.Log("Resume");
        Time.timeScale = 1f;
        GamePause = false;
        PauseMenuUI.SetActive(false);
        WinMenuUI.SetActive(false);
        LoseMenuUI.SetActive(false);
    }

    public void WinResume() {
        Time.timeScale = 1f;
        GamePause = false;
        WinMenuUI.SetActive(false);
    }

    public void LoseResume() {
        Time.timeScale = 1f;
        GamePause = false;
        LoseMenuUI.SetActive(false);
    }

    public void Pause() {
        //Time.timeScale = 0f;
        PauseMenuUI.SetActive(true);
        GamePause = true;
    }

    public void WinPause() {
        //Time.timeScale = 0f;
        WinMenuUI.SetActive(true);
        GamePause = true;
    }

    public void LosePause() {
        //Time.timeScale = 0f;
        LoseMenuUI.SetActive(true);
        GamePause = true;
    }

    public void Menu() {
        Resume();
        LevelLoader.Instance.LoadLevelName("Menu");
    }

    public void WinMenu() {
        Resume();       
        LevelLoader.Instance.LoadLevelName("Menu");
    }

    public void LoseMenu() {
        Resume();
        LevelLoader.Instance.LoadLevelName("Menu");
    }

    public void EndGameManu(bool win) {
        if (win) {
            WinPause();
        }
        else {
            LosePause();
        }
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PauseButton(){
        if (GamePause) {
            Resume();
        }
        else {
            Pause();
        }
    }

    public void Restart() {
        Time.timeScale = 1f;
        GamePause = false;
        Resume();
        Debug.Log("Restart");
        LevelLoader.Instance.LoadLevelIndex(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel() {
        Debug.Log("Next Level");
        Resume();
        LevelLoader.Instance.LoadLevelIndex(nextlevel);
    }

    public void OpenHandBook() {
        GameObject hb = this.transform.Find("HandBook").gameObject;
        if (hb.activeSelf) {
            hb.SetActive(false);
        }
        else {
            hb.SetActive(true);
        }
    
    }
}
