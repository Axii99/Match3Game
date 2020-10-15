using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spell : MonoBehaviour
{
    public int[] RuneCosts;
    public Sprite[] allRuneSprites;
    public int KnightIndex;
    public ParticleSystem SpellEffect;
    public Button btn;

    //cool down
    public float CoolDown = 3f;
    public float nextAvailableTime;
    private float timeLeft;
    public TMPro.TMP_Text CountText;

    public int Rune1Index;
    public int Rune2Index;
    public TMPro.TMP_Text Rune1Text;
    public TMPro.TMP_Text Rune2Text;
    public Image Rune1Image;
    public Image Rune2Image;

    // Start is called before the first frame update
    void Start()
    {
        CountText.text = "";
        btn = this.GetComponent<Button>();
        nextAvailableTime = 0;
        Rune1Text.text = "x" + RuneCosts[Rune1Index].ToString();
        Rune2Text.text = "x" + RuneCosts[Rune2Index].ToString();
        Rune1Image.sprite = allRuneSprites[Rune1Index];
        Rune2Image.sprite = allRuneSprites[Rune2Index];
    }

    // Update is called once per frame
    void Update()
    {
       if (PauseMenu.GamePause) {
            return;
        }
       if (btn.interactable == false) {
            if (Time.time >= nextAvailableTime) {
                btn.interactable = true;
                if (CountText != null) {
                    CountText.text = "";
                }
            }
            else {
                timeLeft -= Time.deltaTime;
                if (CountText != null) {
                    CountText.text = Mathf.Ceil(timeLeft).ToString();
                }
            }
        }

        CheckEnough();

    }

    public void CastSpell() {
        if (PauseMenu.GamePause) {
            return;
        }
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
            //cool down
            CoolDownStart();
        }
    }

    public void Fire(int damage) {
        if (PauseMenu.GamePause) {
            return;
        }
        int[] RuneNumArr = Game_Manager.Instance.getRuneNum();
        bool isEnough = true;
        for (int i = 0; i < RuneNumArr.Length; i++) {
            if ((RuneNumArr[i] - RuneCosts[i]) < 0) {
                isEnough = false;
                break;
            }
        }

        if (isEnough) {
            List<Knight> allEnemies = Game_Manager.Instance.getAliveEnemyKnights();
            Knight[] temp = allEnemies.ToArray();
            if (allEnemies.Count == 0) {
                return;
            }
            for (int i = 0; i < RuneNumArr.Length; i++) {
                Game_Manager.Instance.ConsumeRune(i, RuneCosts[i]);
            }


            AudioManager.Instance.PlaySound("FireSpell");
            for(int i =0; i < temp.Length; i++) {
                //Debug.Log(temp[i].name);
                temp[i].ReduceArmor(10);
                temp[i].TakeDamage(damage);
               
                Vector2 temp_pos = temp[i].transform.position;
                ParticleSystem fire = Instantiate(SpellEffect, temp_pos, Quaternion.identity);
                fire.transform.parent = temp[i].transform;
                Destroy(fire.gameObject, 1.5f);
                Transform temp_tf = temp[i].transform.Find("ShieldEffect(Clone)");
                if (temp_tf != null) {
                    GameObject se = temp_tf.gameObject;
                    Destroy(se);
                }

            }
            //cool down
            CoolDownStart();
            int playerCurrentHealth = (int)Game_Manager.Instance.PlayerCurrentHealth;
            Game_Manager.Instance.TakeDamage(true, playerCurrentHealth * RuneCosts[5] / 100);
        }
    }

    public void Ice(int damage) {
        if (PauseMenu.GamePause) {
            return;
        }
        int[] RuneNumArr = Game_Manager.Instance.getRuneNum();
        bool isEnough = true;
        for (int i = 0; i < RuneNumArr.Length; i++) {
            if ((RuneNumArr[i] - RuneCosts[i]) < 0) {
                isEnough = false;
                break;
            }
        }

        if (isEnough) {
            List<Knight> allEnemies = Game_Manager.Instance.getAliveEnemyKnights();
            Knight[] temp = allEnemies.ToArray();
            if (allEnemies.Count == 0) {
                return;
            }
            for (int i = 0; i < RuneNumArr.Length; i++) {
                Game_Manager.Instance.ConsumeRune(i, RuneCosts[i]);
            }


            AudioManager.Instance.PlaySound("IceSpell");
            for (int i = 0; i < temp.Length; i++) {
                //Debug.Log(temp[i].name);
                temp[i].TakeDamage(damage);
                float temp_speed = temp[i].speed / 2f;
                temp[i].speed = 0;
                StartCoroutine(temp[i].ChangeAttribute("speed", temp_speed, 3f));
                StartCoroutine(temp[i].ChangeAttribute("attackRate", temp[i].attackRate*0.6f, 0f));
                Vector2 temp_pos = temp[i].transform.position;
                ParticleSystem ice = Instantiate(SpellEffect, temp_pos, Quaternion.identity);
                ice.transform.parent = temp[i].transform;
                Destroy(ice.gameObject, 1.5f);

            }
            //cool down
            CoolDownStart();
            int playerCurrentHealth = (int)Game_Manager.Instance.PlayerCurrentHealth;
            Game_Manager.Instance.TakeDamage(true, playerCurrentHealth * RuneCosts[5] / 100);
        }
    }

    public void DarkHealing(int amount) {
        if (PauseMenu.GamePause) {
            return;
        }
        int[] RuneNumArr = Game_Manager.Instance.getRuneNum();
        bool isEnough = true;
        for (int i = 0; i < RuneNumArr.Length; i++) {
            if ((RuneNumArr[i] - RuneCosts[i]) < 0) {
                isEnough = false;
                break;
            }
        }

        if (isEnough) {
            List<Knight> allKnights = Game_Manager.Instance.getAlivePlayerKnights();
            Knight[] temp = allKnights.ToArray();
            if (allKnights.Count == 0) {
                return;
            }
            for (int i = 0; i < RuneNumArr.Length; i++) {
                Game_Manager.Instance.ConsumeRune(i, RuneCosts[i]);
            }


            AudioManager.Instance.PlaySound("DarkHealingSpell");

            for (int i = 0; i < temp.Length; i++) {
                //Debug.Log(temp[i].name);
                temp[i].speed *= 1.2f;
                temp[i].maxHealth += 50;
                temp[i].currentHealth =Mathf.Min(amount + temp[i].currentHealth, temp[i].maxHealth);
                temp[i].armor += 10;
                Vector2 temp_pos = temp[i].transform.position;
                ParticleSystem heal = Instantiate(SpellEffect, temp_pos, Quaternion.identity);
                heal.transform.parent = temp[i].transform;
                //Destroy(heal.gameObject, 1.5f);
               
            }
            //cool down
            CoolDownStart();
            int playerCurrentHealth = (int) Game_Manager.Instance.PlayerCurrentHealth;
            Game_Manager.Instance.TakeDamage(true, playerCurrentHealth * RuneCosts[5] / 100);
        }
    }



    void CoolDownStart() {
        btn.interactable = false;
        nextAvailableTime = Time.time + CoolDown;
        timeLeft = CoolDown;
    }

    private void CheckEnough() {
        int[] RuneNumArr = Game_Manager.Instance.getRuneNum();
        bool isEnough = true;
        for (int i = 0; i < RuneNumArr.Length; i++) {
            if ((RuneNumArr[i] - RuneCosts[i]) < 0) {
                isEnough = false;
                break;
            }
        }
        Button btn = this.GetComponent<Button>();
        ColorBlock colors = btn.colors;
        if (isEnough) {          
            colors.normalColor = Color.white;
        }
        else {
            colors.normalColor = Color.gray;
        }
        btn.colors = colors;
    }

}
