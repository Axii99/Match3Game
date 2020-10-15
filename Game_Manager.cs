using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour {

    public static Game_Manager Instance { get; private set; }

    // UI Variables
    public Text[] RuneNumText;
    [SerializeField]
    private int[] RuneNumArr;
    public Slider PlayerHealthBar;
    public Slider EnemyHealthBar;
    public PauseMenu pm;
    public int nextLevel;
    //GameObjects
    public float PlayerMaxHealth = 1000;
    public float EnemyMaxHealth = 1000;
    public float PlayerCurrentHealth;
    public float EnemyCurrentHealth;
    public Knight[] allPlayerKnights;
    public Knight[] allEnemyKnights;
    public FightPlatform[] allPlatforms;

    public GameObject SelectedStart;
    public GameObject SelectedEnd;

    //Knights and Enemies
    [SerializeField]
    private List<Knight> alivePlayerKnights;
    [SerializeField]
    private List<Knight> aliveEnemyKnights;

    //endless mode
    public bool endless = false;

    /// <summary>
    /// ///////////////////////YVO
    /// </summary>
    private bool[] yvo = { false, false, false };
    //////

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else {

            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        PlayerCurrentHealth = PlayerMaxHealth;
        EnemyCurrentHealth = EnemyMaxHealth;
        SetPlayerHealth(100f);
        SetEnemyHealth(100f);
        RuneNumArr = new int[6];
        SelectPlatform(allPlatforms[0]);
        foreach (Text rText in RuneNumText) {
            rText.text = "0";
        }

        alivePlayerKnights = new List<Knight>();
        aliveEnemyKnights = new List<Knight>();
    }

    private void OnLevelWasLoaded(int level) {
        Time.timeScale = 1f;
        PlayerCurrentHealth = PlayerMaxHealth;
        EnemyCurrentHealth = EnemyMaxHealth;
        SetPlayerHealth(100f);
        SetEnemyHealth(100f);
        RuneNumArr = new int[6];
        foreach (Text rText in RuneNumText) {
            rText.text = "0";
        }
    }

    // Update is called once per frame
    void Update() {
        if (PauseMenu.GamePause) {
 
            return;
        }
        if (PlayerCurrentHealth <= 0) {
            //Debug.Log("lose");
            if (endless) {
                Debug.Log("game over");
                EndGame(false);
            }
            else {
                EndGame(false);
            }
        }
        else if (EnemyCurrentHealth <= 0) {
            //Debug.Log("win");
            if (endless) {
                EnemyCurrentHealth = EnemyMaxHealth;
                SetEnemyHealth(1);
                SetPlayerHealth(1);
                Game_Manager.Instance.GetComponent<EndlessLevel>().AddScore((int) EnemyMaxHealth/10);
                Game_Manager.Instance.GetComponent<EndlessLevel>().StageUp();
            }
            else {
                EndGame(true);
            }
            
        }

        Yvo();
        //RandomSpawnEnemy();
        for (int i = 0; i < RuneNumArr.Length; i++) {
            RuneNumText[i].text = RuneNumArr[i].ToString();
        }
    }


    public void RandomSpawnEnemy() {

            int randomKnight = Random.Range(0, allEnemyKnights.Length);
            int randomPlatform = Random.Range(0, allPlatforms.Length);
            Knight temp_knight = Instantiate(allEnemyKnights[randomKnight], allPlatforms[randomPlatform].transform.position, Quaternion.identity);
            temp_knight.StartPoint = allPlatforms[randomPlatform].EndPoint;
            temp_knight.EndPoint = allPlatforms[randomPlatform].StartPoint;


    }

    public Knight SpawnEnemy(int KnightIndex, int PlatformIndex) {
       
        Knight temp_knight = Instantiate(allEnemyKnights[KnightIndex], allPlatforms[PlatformIndex].transform.position, Quaternion.identity);
        temp_knight.StartPoint = allPlatforms[PlatformIndex].EndPoint;
        temp_knight.EndPoint = allPlatforms[PlatformIndex].StartPoint;
        aliveEnemyKnights.Add(temp_knight);
        return temp_knight;
    }

    public void CreatePlayerKnight(int index) {
       
        Knight temp =  Instantiate(allPlayerKnights[index], SelectedStart.transform.position, Quaternion.identity);
        temp.StartPoint = SelectedStart;
        temp.EndPoint = SelectedEnd;
        alivePlayerKnights.Add(temp);
    }

    public void Hello() {
        Debug.Log("aaa");
    }

    public void AddRuneNum(int index, int num) {
        RuneNumArr[index] += num;
    }

    public int[] getRuneNum() {
        return RuneNumArr;
    }

    public void ConsumeRune(int index, int num) {
        RuneNumArr[index] -= num;
    }


    public void SelectPlatform(FightPlatform fp) {
        if (fp != null) {
            fp.Select();
            SelectedStart = fp.GetComponent<FightPlatform>().StartPoint;
            SelectedEnd = fp.GetComponent<FightPlatform>().EndPoint;

            for (int i = 0; i < allPlatforms.Length; i++) {
                if (allPlatforms[i].name != fp.name) {
                    allPlatforms[i].Unselect();
                }
            }
        }
    }

    public void SetPlayerHealth(float v) {
        PlayerHealthBar.value = v * PlayerHealthBar.maxValue;
    }

    public void SetEnemyHealth(float v) {
        EnemyHealthBar.value = v * EnemyHealthBar.maxValue;
    }


    public void TakeDamage(bool isPlayer, float damage) {
        if (isPlayer) {
            PlayerCurrentHealth -= damage;
            SetPlayerHealth(PlayerCurrentHealth / PlayerMaxHealth);
        }
        else {

            EnemyCurrentHealth -= damage;
            SetEnemyHealth(EnemyCurrentHealth / EnemyMaxHealth);
            if (endless) {
                Game_Manager.Instance.GetComponent<EndlessLevel>().AddScore((int)damage/10);
            }
        }
    }

    public List<Knight> getAliveEnemyKnights() {
        return aliveEnemyKnights;
    }

    public List<Knight> getAlivePlayerKnights() {
        return alivePlayerKnights;
    }

    public void RemovePlayerKnight(Knight k) {
        alivePlayerKnights.Remove(k);
    }

    public void RemoveEnemyKnight(Knight k) {
        aliveEnemyKnights.Remove(k);
        if (endless && k.currentHealth <= 0) {
           Game_Manager.Instance.GetComponent<EndlessLevel>().AddScore(10);
        }
    }

    ///YVO
    private void Yvo() {
        if (yvo[0] && yvo[1] && yvo[2]) {
            Knight temp = Instantiate(allPlayerKnights[0], SelectedStart.transform.position, Quaternion.identity);
            temp.StartPoint = SelectedStart;
            temp.EndPoint = SelectedEnd;
            temp.transform.localScale *= 2;
            temp.speed = 20;
            temp.attackDamage = 100;
            temp.attackRange = 1;
            temp.attackRate = 3;
            temp.maxHealth = 2000;
            yvo[0] = false;
            yvo[1] = false;
            yvo[2] = false;
            for (int i = 0; i < RuneNumArr.Length; i++) {
               RuneNumArr[i] = 500;
            }
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            yvo[0] = true;
        }

        if (Input.GetKeyDown(KeyCode.V)) {
            yvo[1] = true;
        }

        if (Input.GetKeyDown(KeyCode.O)) {
            yvo[2] = true;
        }
    }

    void EndGame(bool win) {
        if (win) {
            AudioManager.Instance.PlaySound("Win");
            AudioManager.Instance.PauseSound("Theme");
            pm.EndGameManu(win);
           
            PlayerPrefs.SetInt("levelReached", nextLevel);
            PlayerPrefs.Save();
        }
        else {
            AudioManager.Instance.PlaySound("Lose");
            AudioManager.Instance.PauseSound("Theme");
            pm.EndGameManu(win);
        }
        //SceneManager.LoadScene(0);
        //return;
    }



}
