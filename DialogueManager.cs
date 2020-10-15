using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{


    private Queue<string> sentences;

    public bool available;
    public GameObject dialogueCanvas;

    public Image avatar1;
    public Image avatar2;
    public TMPro.TMP_Text Name1;
    public TMPro.TMP_Text Name2;
    public TMPro.TMP_Text dialogueContent;
    public Button nextBtn;
    // Start is called before the first frame update
    void Start()
    {

        sentences = new Queue<string>();
        available = true;
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue(Dialogue dial) {
        if (dial == null) {
            return;
        }
        dialogueCanvas.SetActive(true);
        available = false;

        sentences.Clear();
        foreach(string sentence in dial.sentences) {
            sentences.Enqueue(sentence);
        }
        avatar1.sprite = dial.avatar1;
        avatar2.sprite = dial.avatar2;
        Name1.text = "";
        Name2.text = "";
        if (dial.left) {
            Name1.text = dial.name;
        }
        else {
            Name2.text = dial.name;
        }
        DisplayNextSentence();

    }

    public void DisplayNextSentence() {
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }
        string current = sentences.Dequeue();
        dialogueContent.text = current;
    }

    public void EndDialogue() {
        available = true;
    }

    public void closeDialogueCanvas() {
        dialogueCanvas.SetActive(false);
    }
}
