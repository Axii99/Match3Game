using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TriggerDialogue(int index) {
        if (index >= dialogue.Length) {
            return;
        }
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue[index]);
    }
}
