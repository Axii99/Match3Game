using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }
    public Animator transition;
    public float transitionTime;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else {

            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LoadLevelName(string name) {
        Debug.Log(name + " load");
        transition.SetTrigger("Start");
        StartCoroutine(loadname(name));
    }

    IEnumerator loadname(string name) {
        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(name);
    }


    public void LoadLevelIndex(int index) {
        transition.SetTrigger("Start");
        StartCoroutine(loadIndex(index));
    }

    IEnumerator loadIndex(int index) {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }

}
