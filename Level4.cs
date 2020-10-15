using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    public Wave[] waves;
    public float FirstWaveTime;
    public float WaveInterval;
    private int spawnedWaves;
    public float nextWaveTime;

    public GameObject ShieldEffect;
    public MessageBox mb;
    public bool ready;
    // Start is called before the first frame update
    void Start() {
        spawnedWaves = 0;
        nextWaveTime = Time.time + FirstWaveTime;
    }

    // Update is called once per frame
    void Update() {
        if (!ready || PauseMenu.GamePause) {
            return;
        }
        if (Time.time >= nextWaveTime) {
            if (spawnedWaves < waves.Length) {
                StartCoroutine(SpawnWave(spawnedWaves));
                int PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);

            }
            else if (Game_Manager.Instance.EnemyCurrentHealth / Game_Manager.Instance.EnemyCurrentHealth > 0.5) {
                DynamicDifficulty();
                StartCoroutine(RandomSpawn(2, 2f));
                int PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);
                StartCoroutine(DelaySpawn(4, PlatformIndex, 3));

                if (Random.Range(0, 5) == 0) {
                    StartCoroutine(ArmorBuff(5f));
                }
            }
            else {
                WaveInterval = 22;
                DynamicDifficulty();
                StartCoroutine(RandomSpawn(3, 2f));
                int PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);
                StartCoroutine(DelaySpawn(4, PlatformIndex, 3));
                if (Random.Range(0, 4) == 0) {
                    StartCoroutine(ArmorBuff(5f));
                }
            }
        }
        else {
            if (Time.time - nextWaveTime == 10f && Random.Range(0, 2) == 0) {
                int PlatformIndex = Random.Range(0, Game_Manager.Instance.allPlatforms.Length);
                StartCoroutine(DelaySpawn(3, PlatformIndex, 2));
            }
        }

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
            Game_Manager.Instance.SpawnEnemy(EnemyIndex, PlatformIndex);
            yield return new WaitForSeconds(interval);
        }

    }

    IEnumerator DelaySpawn(int knight, int platform, float interval) {
        yield return new WaitForSeconds(interval);
        Game_Manager.Instance.SpawnEnemy(knight, platform);
    }

    IEnumerator ArmorBuff(float time) {
        if (time > 0) {
            if (mb != null) {
                StartCoroutine(mb.SendSpellWarning());
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
                GameObject heal = Instantiate(ShieldEffect, temp_pos, Quaternion.identity);
                heal.transform.parent = temp[i].transform;
                temp[i].transform.localScale *= 1.2f;

            }

        }

    }


    void DynamicDifficulty() {
        int[] RunePool = Game_Manager.Instance.getRuneNum();
        int temp = (int)(RunePool[0] / 12 + RunePool[1] / 12 + RunePool[2] / 12 + RunePool[4] / 12 + RunePool[5] / 12);
        temp = temp / 9;
        StartCoroutine(RandomSpawn(temp, 1.5f));
    }
}
