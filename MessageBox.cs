using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    public float showTime = 2f;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator  SendSpellWarning() {
        AudioManager.Instance.PlaySound("MessageWarning");
        animator.SetTrigger("Message");
        this.GetComponent<TMPro.TMP_Text>().text = "Enemy Spell Detected";
        this.gameObject.SetActive(true);
        yield return new WaitForSeconds(showTime);
        this.gameObject.SetActive(false);
    }
}
