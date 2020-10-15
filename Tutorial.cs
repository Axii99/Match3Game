using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public PauseMenu pm;
    public DialogueManager dm;
    private int DialogueIndex;
    private int nextIndex;
    private int step;
    private void Awake() {
        StartCoroutine(pasueGame());
    }
    // Start is called before the first frame update
    void Start()
    {
        DialogueIndex = 0;
        nextIndex = 3;
        step = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (dm.available) {
            if (step == 0) {
                if (DialogueIndex == nextIndex) {
                    dm.closeDialogueCanvas();
                    pm.Resume();
  
                    step++;
                    nextIndex = 5;
                    Debug.Log("1");
                }
                else {
                    FindObjectOfType<DialogueTrigger>().TriggerDialogue(DialogueIndex);

                    DialogueIndex++;
                }
            }
            else if (step == 1) {
                if (DialogueIndex == nextIndex) {
                    dm.closeDialogueCanvas();
                    pm.Resume();
 
                    step++;
                    nextIndex = 7;
                }
                else if (DialogueIndex == 3) {
                    int[] temp_arr = Game_Manager.Instance.getRuneNum();
                    if (temp_arr[0] >= 9 || temp_arr[2] >= 9 || temp_arr[4] >= 9) {
                            Game_Manager.Instance.SpawnEnemy(0, 0);
                            pm.setPause();
                            FindObjectOfType<DialogueTrigger>().TriggerDialogue(DialogueIndex);
                            DialogueIndex++;
                    }
                    
                }
                else {
                    FindObjectOfType<DialogueTrigger>().TriggerDialogue(DialogueIndex);
                    DialogueIndex++;
                }
            }
            else if (step == 2){
                if (!Game_Manager.Instance.GetComponent<TutorialLevel>().ready) {
                    Game_Manager.Instance.GetComponent<TutorialLevel>().ready = true;
                    Game_Manager.Instance.GetComponent<TutorialLevel>().nextWaveTime = Time.time + Game_Manager.Instance.GetComponent<TutorialLevel>().FirstWaveTime;
                }
            }
            else if (step == 3) {
                if (DialogueIndex == nextIndex) {
                    dm.closeDialogueCanvas();
                    pm.Resume();
                    step++;
                    PlayerPrefs.SetInt("levelReached", Game_Manager.Instance.GetComponent<Game_Manager>().nextLevel);
                    PlayerPrefs.Save();
                    pm.NextLevel();
                }
                else {
                    FindObjectOfType<DialogueTrigger>().TriggerDialogue(DialogueIndex);

                    DialogueIndex++;
                }
            }
        }




    }

    public void EndGameStory()
    {
        step++;

    }
    public void skip() {
        DialogueIndex = nextIndex;
        dm.EndDialogue();
        dm.closeDialogueCanvas();

    }

    IEnumerator pasueGame() {
        
        yield return new WaitForSeconds(0.8f);
        //AudioManager.Instance.PauseSound("Theme");
        pm.setPause();
        pm.enabled = false;
    }
}
