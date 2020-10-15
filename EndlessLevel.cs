using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessLevel : MonoBehaviour
{
    public Wave[] waves;
    public float FirstWaveTime;
    public float WaveInterval;
    private int spawnedWaves;
    public float nextWaveTime;
    public bool ready;
    public int score;
    public int SuperKnightCoolDown = 5;

    //UI
    public Button scoreButton;
    public TMPro.TMP_Text highscore;
    public TMPro.TMP_Text stagetext;

    //effect
    public Effect[] effects;

    public int stage;
    // Start is called before the first frame update
    void Start() {
        spawnedWaves = 0;
        nextWaveTime = Time.time + FirstWaveTime;
        stage = 0;
        score = 0;
        DisplayInfo();
    }

    // Update is called once per frame
    void Update() {
        if (!ready || PauseMenu.GamePause) {
            return;
        }
        if (Time.time >= nextWaveTime) {
            if (spawnedWaves < waves.Length) {
                StartCoroutine(SpawnWave(spawnedWaves));
            }
            else { 
                if (stage < 3) {
                    DynamicDifficulty(12);
                    StartCoroutine(RandomSpawn(2, 3f));
                }
                else if (stage < 6) {
                    WaveInterval = 25;
                    DynamicDifficulty(11);
                    StartCoroutine(RandomSpawn(2, 3f));
                }
                else if (stage < 10){
                    WaveInterval = 20;
                    DynamicDifficulty(10);
                    StartCoroutine(RandomSpawn(3, 3f));
                }
                else {
                    WaveInterval = 18;
                    DynamicDifficulty(9);
                    SuperKnightCoolDown--;
                    StartCoroutine(RandomSpawn(3, 2f));
                }
            }
            if (Random.Range(0, 5) == 0) {
                StartCoroutine(ArmorBuff(5f));
            }
        }
        DisplayInfo();
    }


    IEnumerator SpawnWave(int WaveIndex) {

        Wave current = waves[WaveIndex];
        spawnedWaves++;
        nextWaveTime = Time.time + WaveInterval + current.timeInterval * current.Count;
        for (int i = 0; i < current.Count; i++) {

            while (PauseMenu.GamePause) {
                nextWaveTime = Time.time + WaveInterval + current.timeInterval * (current.Count - i);
                yield return null;
            }

            int EnemyIndex = current.EnemyIndex;
            if (EnemyIndex < 0) {
                EnemyIndex = Random.Range(0, Game_Manager.Instance.allEnemyKnights.Length);
            }
            int PlatformIndex = current.PlatformIndex[i];
            if (PlatformIndex < 0) {
                PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);
            }
            float interval = current.timeInterval;

            Game_Manager.Instance.SpawnEnemy(EnemyIndex, PlatformIndex);
            yield return new WaitForSeconds(interval);
        }


    }


    IEnumerator RandomSpawn(int num, float interval) {

        nextWaveTime = Time.time + WaveInterval + num * interval;
        for (int i = 0; i < num; i++) {

            while (PauseMenu.GamePause) {
                nextWaveTime = Time.time + WaveInterval + interval * (num - i);
                yield return null;
            }

            int EnemyIndex = Random.Range(0, Game_Manager.Instance.allEnemyKnights.Length);
            int PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);
            Knight knight = Game_Manager.Instance.SpawnEnemy(EnemyIndex, PlatformIndex);
            AdjustAttribute(knight);
            yield return new WaitForSeconds(interval);
        }

    }


    void AdjustAttribute(Knight knight) {
        float param = 0;
        if (stage < 6) {
            param = 1 + (stage) * 0.05f;
        }
        else {
            param = 1.3f;
        }
        
        if (stage > 10 && SuperKnightCoolDown == 0){
            knight.transform.localScale *= 1.5f;
            knight.speed *= 0.5f;
            knight.maxHealth = knight.maxHealth * 1.5f;
            knight.currentHealth = knight.currentHealth * 1.5f;
            SuperKnightCoolDown = 1;
        }

        knight.attackDamage = (int) knight.attackDamage * param;
        knight.maxHealth = (int)knight.maxHealth * param;
        knight.currentHealth = (int) knight.currentHealth * param;
        knight.attackRate = knight.attackRate * ((param-1)/2 + 1f);
    }


    void DynamicDifficulty(int param) {
        int[] RunePool = Game_Manager.Instance.getRuneNum();
        int temp = 0;
        for (int i = 0; i < RunePool.Length; i++) {
            if (i == 1 || i == 3 || i==5) {
                temp += RunePool[i] / 20;
            }
            else {
                temp += RunePool[i] / 12;
            }
        }
        temp = temp / param;
        StartCoroutine(RandomSpawn(temp, 1.5f));
    }


    public void StageUp() {
        stage++;
    }

    public void AddScore(int points) {
        float param = ((float)stage) / 10f + 1;
        score += (int) Mathf.Floor(param * (float)points);
    }

    void DisplayInfo() {
        stagetext.text = "level: " + (stage+1).ToString();
        int hs = PlayerPrefs.GetInt("HighScore", 0);
        if (score >= hs) {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            scoreButton.GetComponentInChildren<TMPro.TMP_Text>().text = "High Score: " + score.ToString();
            scoreButton.GetComponentInChildren<TMPro.TMP_Text>().fontStyle = TMPro.FontStyles.Italic;
        }
        else {
            scoreButton.GetComponentInChildren<TMPro.TMP_Text>().text = "Score: " + score.ToString();
        }
        hs = PlayerPrefs.GetInt("HighScore", 0);
        highscore.text = "High Score: " + hs.ToString();
    }

    IEnumerator ArmorBuff(float time) {
        if (time > 0) {
            GameObject messagebox = GameObject.Find("GameCanvas").transform.Find("MessageBox").gameObject;
            if (messagebox != null) {
                StartCoroutine(messagebox.GetComponent<MessageBox>().SendSpellWarning());
            }
            else {
                Debug.Log("MB error");
            }
            yield return new WaitForSeconds(time);
        }
        List<Knight> allKnights = Game_Manager.Instance.getAliveEnemyKnights();
        Knight[] temp = allKnights.ToArray();
        if (allKnights.Count == 0) {
            yield break;
        }
        AudioManager.Instance.PlaySound("ShieldBuff");
        for (int i = 0; i < temp.Length; i++) {
            //Debug.Log(temp[i].name);
            if (temp[i].transform.childCount == 0) {
                temp[i].maxHealth += 50;
                temp[i].currentHealth += 50;
                temp[i].armor += 10;
                Vector2 temp_pos = temp[i].transform.position;
                GameObject ShieldEffect = getEffectName("ShieldEffect");
                if (ShieldEffect != null) {
                    GameObject heal = Instantiate(ShieldEffect, temp_pos, Quaternion.identity);
                    heal.transform.parent = temp[i].transform;
                }
                else {
                    Debug.Log("effect error");
                }

                temp[i].transform.localScale *= 1.2f;

            }

        }

    }

    GameObject getEffectName(string name) {
        foreach (Effect ef in effects) {
            if (ef.name == "ShieldEffect") {
                return ef.effect;
            }
        }
        return null;
    }

}
