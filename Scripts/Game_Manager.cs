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

    //GameLevel
    public float spawnInterval = 4f;
    private float nextSpawnTime = 0;

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
        foreach (Text rText in RuneNumText) {
            rText.text = "0";
        }
    }

    private void OnLevelWasLoaded(int level) {
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

        if (PlayerCurrentHealth <= 0) {
            EndGame(false);
        }
        else if (EnemyCurrentHealth <= 0) {
            EndGame(true);
        }

        Yvo();
        RandomSpawnEnemy();
        for (int i = 0; i < RuneNumArr.Length; i++) {
            RuneNumText[i].text = RuneNumArr[i].ToString();
        }
    }


    public void RandomSpawnEnemy() {
        if (Time.time >= nextSpawnTime) {
            int randomKnight = Random.Range(0, allEnemyKnights.Length);
            int randomPlatform = Random.Range(0, allPlatforms.Length);
            Knight temp_knight = Instantiate(allEnemyKnights[randomKnight], allPlatforms[randomPlatform].transform.position, Quaternion.identity);
            temp_knight.StartPoint = allPlatforms[randomPlatform].EndPoint;
            temp_knight.EndPoint = allPlatforms[randomPlatform].StartPoint;
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    public void CreatePlayerKnight(int index) {
       
        Knight temp =  Instantiate(allPlayerKnights[index], SelectedStart.transform.position, Quaternion.identity);
        temp.StartPoint = SelectedStart;
        temp.EndPoint = SelectedEnd;
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


    public void TakeDamage(bool isEnemy, float damage) {
        if (isEnemy) {
            PlayerCurrentHealth -= damage;
            SetPlayerHealth(PlayerCurrentHealth / PlayerMaxHealth);
        }
        else {

            EnemyCurrentHealth -= damage;
            SetEnemyHealth(EnemyCurrentHealth / EnemyMaxHealth);
        }
    }



    ///YVO
    private void Yvo() {
        if (yvo[0] && yvo[1] && yvo[2]) {
            Knight temp = Instantiate(allPlayerKnights[0], SelectedStart.transform.position, Quaternion.identity);
            temp.StartPoint = SelectedStart;
            temp.EndPoint = SelectedEnd;
            temp.transform.localScale *= 2;
            temp.speed = 2;
            temp.attackDamage = 80;
            temp.attackRange = 1;
            temp.attackRate = 3;
            temp.maxHealth = 1000;
            yvo[0] = false;
            yvo[1] = false;
            yvo[2] = false;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        return;
    }



}
