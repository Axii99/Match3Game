using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    public int[] RuneCosts;
    public Sprite[] allRuneSprites;
    public int KnightIndex;
 


    public int Rune1Index;
    public int Rune2Index;
    public Text Rune1Text;
    public Text Rune2Text;
    public Image Rune1Image;
    public Image Rune2Image;

    // Start is called before the first frame update
    void Start()
    {
        Rune1Text.text = "x " + RuneCosts[Rune1Index].ToString();
        Rune2Text.text = "x " + RuneCosts[Rune2Index].ToString();
        Rune1Image.sprite = allRuneSprites[Rune1Index];
        Rune2Image.sprite = allRuneSprites[Rune2Index];
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CastSpell() {
        int[] RuneNumArr = Game_Manager.Instance.getRuneNum();
        bool isEnough = true;
        for (int i = 0; i < RuneNumArr.Length; i++) {
            if ((RuneNumArr[i] - RuneCosts[i]) < 0) {
                isEnough = false;
                break;
            }
        }

        if (isEnough) {
            for (int i = 0; i < RuneNumArr.Length; i++) {
                Game_Manager.Instance.ConsumeRune(i, RuneCosts[i]);
            }

            Game_Manager.Instance.CreatePlayerKnight(KnightIndex);
        }
    }
}
