using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour {
    public int width;
    public int height;
    public GameObject tilePrefab;
    public float tileSize = 1;
    private GameObject[,] allTiles;
    public GameObject[] runes;
    public GameObject[,] allRunes;
    public GameObject BoomEffect;
    private bool NeedRefill = false;
    
    //Match Check
    public List<GameObject> SameRuneList = new List<GameObject>();
    public List<GameObject> BoomList = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {
        allTiles = new GameObject[width, height];
        allRunes = new GameObject[width, height];
        Setup();
    }

    private void Update() {
        StartCoroutine(Match_Check());
       
    }


    private void Setup() {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2(i * tileSize, j * tileSize);
                GameObject tile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                tile.transform.parent = this.transform;
                tile.name = "(" + i + "," + j + ")";
                int RuneToUse = Random.Range(0, runes.Length);
                while (isInline(runes[RuneToUse], i, j)) {
                    RuneToUse = Random.Range(0, runes.Length);
                }
                GameObject rune = Instantiate(runes[RuneToUse], tempPosition, Quaternion.identity);
                rune.transform.parent = tile.transform;
                rune.name = "(" + i + "," + j + ")";
                rune.GetComponent<Rune>().column = i;
                rune.GetComponent<Rune>().row = j;
                allRunes[i, j] = rune;
                allTiles[i, j] = tile;
            }
        }
    }

    public void Refresh() {
        foreach (GameObject rune in allRunes) {
            if (rune != null) {
                Destroy(rune);
            }
        }
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                Vector2 tempPosition = new Vector2(i * tileSize, j * tileSize);
                GameObject tile = allTiles[i, j];
                int RuneToUse = Random.Range(0, runes.Length);
                while (isInline(runes[RuneToUse], i, j)) {
                    RuneToUse = Random.Range(0, runes.Length);
                }
                GameObject rune = Instantiate(runes[RuneToUse], tempPosition, Quaternion.identity);
                rune.transform.parent = tile.transform;
                rune.name = "(" + i + "," + j + ")";
                rune.GetComponent<Rune>().column = i;
                rune.GetComponent<Rune>().row = j;
                allRunes[i, j] = rune;
            }
        }

    }

    private bool isInline(GameObject rune, int i, int j) {

        if (i < 2 && j < 2) {
            return false;
        }

        GameObject leftRune1 = null;
        GameObject leftRune2 = null;
        GameObject downRune1 = null;
        GameObject downRune2 = null;

        if (i >= 2) {
            leftRune1 = allRunes[i - 1, j];
            leftRune2 = allRunes[i - 2, j];
        }
        else if (i == 1) {
            leftRune1 = allRunes[i - 1, j];
        }

        if (j >= 2) {
            downRune1 = allRunes[i, j - 1];
            downRune2 = allRunes[i, j - 2];
        }
        else if (j == 1) {
            downRune1 = allRunes[i, j - 1];
        }

        if (leftRune1 == null && downRune1 == null) {
            return false;
        }
        else if ((leftRune1 == null || leftRune2 == null) && downRune2 != null) {
            //check horizontal

            if (downRune1.tag == rune.tag && downRune2.tag == rune.tag) {
                return true;
            }
            return false;
        }
        else if ((downRune1 == null || downRune2 == null) && leftRune2 != null) {
            //check parallel
            if (leftRune1.tag == rune.tag && leftRune2.tag == rune.tag) {
                return true;
            }
            return false;
        }

        //check parallel
        if (leftRune1.tag == rune.tag && leftRune2.tag == rune.tag) {
            return true;
        }


        //check horizontal

        if (downRune1.tag == rune.tag && downRune2.tag == rune.tag) {
            return true;
        }


        return false;
    }

    public GameObject GetUpRune(GameObject rune) {

        int tempColumn = rune.GetComponent<Rune>().column;
        int tempRow = rune.GetComponent<Rune>().row;

        if (tempRow < height - 1) {
            return allRunes[tempColumn, tempRow + 1];
        }

        return null;
    }

    public GameObject GetDownRune(GameObject rune) {

        int tempColumn = rune.GetComponent<Rune>().column;
        int tempRow = rune.GetComponent<Rune>().row;

        if (tempRow > 0) {
            return allRunes[tempColumn, tempRow - 1];
        }

        return null;
    }

    public GameObject GetLeftRune(GameObject rune) {

        int tempColumn = rune.GetComponent<Rune>().column;
        int tempRow = rune.GetComponent<Rune>().row;

        if (tempColumn > 0) {
            return allRunes[tempColumn - 1, tempRow];
        }

        return null;
    }

    public GameObject GetRightRune(GameObject rune) {

        int tempColumn = rune.GetComponent<Rune>().column;
        int tempRow = rune.GetComponent<Rune>().row;

        if (tempColumn < width - 1) {
            return allRunes[tempColumn + 1, tempRow];
        }

        return null;
    }

    public void FillSameRuneList(GameObject rune) {
        if (SameRuneList.Contains(rune) || rune == null) {
            return;
        }

        int tempColumn = rune.GetComponent<Rune>().column;
        int tempRow = rune.GetComponent<Rune>().row;

        //if (tempColumn < 1 || tempColumn > width - 2 || tempRow < 1 || tempRow > height - 2) {
        //    return;
        //}

        GameObject[] templist = new GameObject[] {GetDownRune(rune),
                                                    GetUpRune(rune),
                                                    GetLeftRune(rune),
                                                    GetRightRune(rune)};
        for (int i = 0; i < templist.Length; i++) {
            if (templist[i] == null) {
                continue;
            }

            if (templist[i].tag == rune.tag) {
                if (!SameRuneList.Contains(rune)) {
                    SameRuneList.Add(rune);
                    //Debug.Log("add: " + rune.name);
                }
                FillSameRuneList(templist[i]);
            }
        }
    }


    public void FillBoomList(GameObject current) {
        //计数器
        int rowCount = 0;
        int columnCount = 0;
        //临时列表
        List<GameObject> rowTempList = new List<GameObject>();
        List<GameObject> columnTempList = new List<GameObject>();
        //横向纵向检测
        foreach (GameObject item in SameRuneList) {
            if (current.tag != item.tag) {
                continue;
            }
            //如果在同一行
            if (item.GetComponent<Rune>().row == current.GetComponent<Rune>().row) {
                //判断该点与Curren中间有无间隙
                bool rowCanBoom = CheckItemsInterval(true, current, item);
                if (rowCanBoom) {
                    //计数
                    rowCount++;
                    //添加到行临时列表
                    rowTempList.Add(item);
                }
            }
            //如果在同一列
            if (item.GetComponent<Rune>().column == current.GetComponent<Rune>().column) {
                //判断该点与Curren中间有无间隙
                bool columnCanBoom = CheckItemsInterval(false, current, item);
                if (columnCanBoom) {
                    //计数
                    columnCount++;
                    //添加到列临时列表
                    columnTempList.Add(item);
                }
            }
        }
        //横向消除
        bool horizontalBoom = false;
        //如果横向三个以上
        //Debug.Log(current.name + "rowCount: " + rowCount + " columnCount: " + columnCount);
        if (rowCount > 2) {
            //将临时列表中的Item全部放入BoomList

            //BoomList.AddRange(rowTempList);
            BoomList.Add(current);
            //横向消除
            horizontalBoom = true;
        }
        //如果纵向三个以上
        if (columnCount > 2) {
            if (!horizontalBoom) {
                //剔除自己
                //BoomList.Remove(current);
                BoomList.Add(current);
            }
            //将临时列表中的Item全部放入BoomList


            //BoomList.AddRange(columnTempList);
        }
        //如果没有消除对象，返回
        if (BoomList.Count == 0) {
            //Debug.Log("NO BOOM!!!");
            return;
        }
        //BoomList = new HashSet<GameObject>(BoomList);
    }

    private bool CheckItemsInterval(bool isHorizontal, GameObject begin, GameObject end) {
        //获取图案
        string temp_tag = begin.tag; //如果是横向
        if (isHorizontal) {
            //起点终点列号
            int beginIndex = begin.GetComponent<Rune>().column;
            int endIndex = end.GetComponent<Rune>().column;
            //如果起点在右，交换起点终点列号
            if (beginIndex > endIndex) {
                beginIndex = end.GetComponent<Rune>().column;
                endIndex = begin.GetComponent<Rune>().column;
            }
            //遍历中间的Item
            for (int i = beginIndex + 1; i < endIndex; i++) {
                //异常处理(中间未生成，标识为不合法)
                if (allRunes[i, begin.GetComponent<Rune>().row] == null) {
                    return false;
                }
                //如果中间有间隙(有图案不一致的)
                if (allRunes[i, begin.GetComponent<Rune>().row].tag != temp_tag) {
                    return false;
                }
            }
            //Debug.Log("CKI: " + begin.name + " " + end.name +"--"+ beginIndex +"," +endIndex);
            return true;
        }
        else {
            //起点终点行号
            int beginIndex = begin.GetComponent<Rune>().row;
            int endIndex = end.GetComponent<Rune>().row;
            //如果起点在上，交换起点终点列号
            if (beginIndex > endIndex) {
                beginIndex = end.GetComponent<Rune>().row;
                endIndex = begin.GetComponent<Rune>().row;
            }
            //遍历中间的Item
            for (int i = beginIndex + 1; i < endIndex; i++) {
                //如果中间有间隙(有图案不一致的)
                if (allRunes[begin.GetComponent<Rune>().column, i] == null || allRunes[begin.GetComponent<Rune>().column, i].tag != temp_tag) {
                    return false;
                }
            }
            //Debug.Log("CKI: " + begin.name + " " + end.name);
            return true;
        }
    }

    public void Boom() {
        
        if (BoomList.Count == 0) {
            return;
        }

        foreach (GameObject go in BoomList) {
            int temp_col = go.GetComponent<Rune>().column;
            int temp_row = go.GetComponent<Rune>().row;
            GameObject ps = Instantiate(BoomEffect, new Vector2(temp_col * tileSize, temp_row * tileSize), Quaternion.identity);
            Destroy(go);
            Destroy(ps,0.5f);
            //Debug.Log("(" + temp_col + "," + temp_row + ")");
            
            allRunes[temp_col, temp_row] = null;
            NeedRefill = true;
        }
        //Debug.Log("-----------------");
    }


    //Match Check
    public IEnumerator Match_Check() {
        yield return new WaitForSeconds(0.1f);
        foreach (GameObject tempR in allRunes) {
            FillSameRuneList(tempR);
        }


        foreach (GameObject go in SameRuneList) {
            FillBoomList(go);
            //SpriteRenderer mysprite = go.GetComponent<SpriteRenderer>();
            //mysprite.color = new Color(1f, 1f, 1f, .2f);

        }

        foreach (GameObject go in BoomList) {
            SpriteRenderer mysprite = go.GetComponent<SpriteRenderer>();
            mysprite.color = new Color(1f, 1f, 1f, .2f);
            go.GetComponent<Rune>().isMatched = true;
        }

        AddRuneToPlayer();
        Boom();

        SameRuneList.Clear();
        BoomList.Clear();
        if (NeedRefill) {
            DropAll();
            NeedRefill = false;

        }

    }

    public void AddRuneToPlayer() {
        if (BoomList.Count == 0) {
            return;
        }
        int[] temp_arr = { 0, 0, 0, 0, 0, 0 };

        foreach(GameObject rune in BoomList) {
            switch ((string) rune.tag) {
                case "Rune1":
                    temp_arr[0] += 1;
                    break;
                case "Rune2":
                    temp_arr[1] += 1;
                    break;
                case "Rune3":
                    temp_arr[2] += 1;
                    break;
                case "Rune4":
                    temp_arr[3] += 1;
                    break;
                case "Rune5":
                    temp_arr[4] += 1;
                    break;
                case "Rune6":
                    temp_arr[5] += 1;
                    break;
                default:
                    Debug.Log("error");
                    break;
            }
        }

        for (int i = 0; i < temp_arr.Length; i++) {
            Game_Manager.Instance.GetComponent<Game_Manager>().AddRuneNum(i, temp_arr[i]);
        }

    }


    public void DropAll() {
        //逐列检测
        for (int i = 0; i < width; i++) {
            //计数器
            int count = 0;
            //下落队列
            Queue<GameObject> dropQueue = new Queue<GameObject>();
            //逐行检测
            for (int j = 0; j < height; j++) {
                if (allRunes[i, j] != null) {
                    //计数
                    count++;
                    //放入队列
                    dropQueue.Enqueue(allRunes[i, j]);
                }

            }
            //下落
            for (int k = 0; k < count; k++) {
                //获取要下落的Item
                GameObject current = dropQueue.Dequeue();
                //修改全局数组(原位置情况)
                allRunes[current.GetComponent<Rune>().column, current.GetComponent<Rune>().row] = null;
                //修改Item的行数
                current.GetComponent<Rune>().row = k;
                current.GetComponent<Rune>().prevRow = k;
                //修改全局数组(填充新位置)
                allRunes[current.GetComponent<Rune>().column, current.GetComponent<Rune>().row] = current;
                //下落
            }
        }
        
        StartCoroutine(Refill());
       
    }

    public IEnumerator Refill() {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                if (allRunes[i, j] == null) {
                    Vector2 tempPosition = new Vector2(i * tileSize, height * tileSize);
                    GameObject tile = allTiles[i, j];
                    int RuneToUse = Random.Range(0, runes.Length);
                    GameObject rune = Instantiate(runes[RuneToUse], tempPosition, Quaternion.identity);
                    rune.transform.parent = tile.transform;
                    rune.name = "(" + i + "," + j + ")";
                    rune.GetComponent<Rune>().column = i;
                    rune.GetComponent<Rune>().row = j;
                    allRunes[i, j] = rune;
                }
            }
        }
       
    }

}
