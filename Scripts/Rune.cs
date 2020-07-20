using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rune : MonoBehaviour {

    //Board
    private Board board;
    public int column;
    public int row;

    //Movement
    public int prevColumn;
    public int prevRow;
    public int targetX;
    public int targetY;
    private GameObject otherRune;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 tempPosition;
    public float swipeAngle = 0;

    //Attribute

    private static Material origin_material;
    public bool isMatched = false;
    private bool isSelectable = true;

    // Start is called before the first frame update
    void Start() {
        board = FindObjectOfType<Board>();
        Renderer r = this.gameObject.GetComponentInChildren<Renderer>();
        origin_material = r.material;
        targetX = (int) this.transform.position.x;
        targetY = (int)this.transform.position.y;
        //column = targetX;
        //row = targetY;
        prevColumn = column;
        prevRow = row;
    }

    // Update is called once per frame
    void Update() {

        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > 0.1) {
            isSelectable = false;
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
        }
        else {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allRunes[column, row] = this.gameObject;
            isSelectable = true;
        }

        if (Mathf.Abs(targetY- transform.position.y) > 0.1) {
            isSelectable = false;
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
        }
        else {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allRunes[column, row] = this.gameObject;
            isSelectable = true;
        }

        //StartCoroutine(FindMatches());
        


    }

    private void getAngle() {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        //Debug.Log(swipeAngle);
    }

    private void OnMouseDown() {
        if (!isSelectable) {
            return;
        }
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        SelectRune();        
        //Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp() {
        ClearSelection();
        if (!isSelectable) {
            return;
        }
        
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        getAngle();

        float MouseMoveDist = Vector2.Distance(finalTouchPosition, firstTouchPosition);
        if (MouseMoveDist > 0.5 && MouseMoveDist < 5) {
            MoveRune();
        }
        

    }

    private void SelectRune() {
        Renderer r = this.gameObject.GetComponentInChildren<Renderer>();
        Material temp_m = r.material;
        temp_m.color = Color.gray;
        r.material = temp_m;
    }

    private void ClearSelection() {
        Renderer r = this.gameObject.GetComponentInChildren<Renderer>();
        r.material = origin_material;
    }

    private void MoveRune() {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1) {
            //Right Swipe
            otherRune = board.allRunes[column + 1, row];
            otherRune.GetComponent<Rune>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1) {
            //Up Swipe
            otherRune = board.allRunes[column, row + 1];
            otherRune.GetComponent<Rune>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && column > 0) {
            //Left Swipe
            otherRune = board.allRunes[column - 1, row];
            otherRune.GetComponent<Rune>().column += 1;
            column -= 1;
        }
        else if (swipeAngle <= -45 && swipeAngle > -135 && row > 0) {
            //Down Swipe
            otherRune = board.allRunes[column, row - 1];
            otherRune.GetComponent<Rune>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMoveCo());
        otherRune.GetComponent<Rune>().prevColumn = otherRune.GetComponent<Rune>().column;
        otherRune.GetComponent<Rune>().prevRow = otherRune.GetComponent<Rune>().row;
    }

    public IEnumerator CheckMoveCo() {
        yield return new WaitForSeconds(.2f);
        if (otherRune != null) {
            if (!isMatched && !otherRune.GetComponent<Rune>().isMatched) {
                otherRune.GetComponent<Rune>().row = row;
                otherRune.GetComponent<Rune>().column = column;
                otherRune.GetComponent<Rune>().prevColumn = otherRune.GetComponent<Rune>().column;
                otherRune.GetComponent<Rune>().prevRow = otherRune.GetComponent<Rune>().row;
                board.allRunes[column, row] = otherRune;
                column = prevColumn;
                row = prevRow;
                board.allRunes[column, row] = this.gameObject;
            }
            
            otherRune = null;
            
        }
        else {
            Debug.Log("aaaaaaaaaaaaaaaaaaa");
            prevColumn = column;
            prevRow = row;
           
        }

    }

    public IEnumerator FindMatches() {
        yield return new WaitForSeconds(.1f);
        if (column > 0 && column < board.width - 1) {
            GameObject leftRune1 = board.allRunes[column - 1, row];
            GameObject rightRune1 = board.allRunes[column + 1, row];
            if ((leftRune1.tag == this.gameObject.tag) && (rightRune1.tag == this.gameObject.tag)) {
                leftRune1.GetComponent<Rune>().isMatched = true;
                rightRune1.GetComponent<Rune>().isMatched = true;
                this.isMatched = true;
                Debug.Log(name + "1match!");
            }
        }

        if (row > 0 && row < board.height - 1) {
            GameObject upRune1 = board.allRunes[column, row + 1];
            GameObject downRune1 = board.allRunes[column, row - 1];
            if ((upRune1.tag == this.gameObject.tag) && (downRune1.tag == this.gameObject.tag)) {
                upRune1.GetComponent<Rune>().isMatched = true;
                downRune1.GetComponent<Rune>().isMatched = true;
                this.isMatched = true;
                Debug.Log(column +"," +row+ "   2match!:");
            }
        }

    }
}
