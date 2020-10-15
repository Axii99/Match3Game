using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story_lv3 : MonoBehaviour {
    public PauseMenu pm;
    public DialogueManager dm;
    private int DialogueIndex;
    private int nextIndex;
    private int step;
    private void Awake() {
        StartCoroutine(pasueGame());
    }
    // Start is called before the first frame update
    void Start() {
        DialogueIndex = 0;
        nextIndex = 3;
        step = 0;
    }

    // Update is called once per frame
    void Update() {
        if (dm.available) {
            if (step == 0) {
                if (DialogueIndex == nextIndex) {
                    dm.closeDialogueCanvas();
                    pm.Resume();
                    step++;
                    nextIndex = 999;
                }
                else {
                    FindObjectOfType<DialogueTrigger>().TriggerDialogue(DialogueIndex);

                    DialogueIndex++;
                }
            }
            else if (step == 1) {
                if (!Game_Manager.Instance.GetComponent<Level3>().ready) {
                    dm.closeDialogueCanvas();
                    pm.Resume();
                    Game_Manager.Instance.GetComponent<Level3>().ready = true;
                    Game_Manager.Instance.GetComponent<Level3>().nextWaveTime = Time.time + Game_Manager.Instance.GetComponent<Level3>().FirstWaveTime;
                }
            }

        }




    }

    public void EndGameStory() {
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
