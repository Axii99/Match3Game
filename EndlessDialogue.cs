using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessDialogue : MonoBehaviour
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
    void Start() {
        DialogueIndex = 0;
        nextIndex = 2;
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
                if (!Game_Manager.Instance.GetComponent<EndlessLevel>().ready) {
                    Game_Manager.Instance.GetComponent<EndlessLevel>().ready = true;
                    Game_Manager.Instance.GetComponent<EndlessLevel>().nextWaveTime = Time.time + Game_Manager.Instance.GetComponent<EndlessLevel>().FirstWaveTime;
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
