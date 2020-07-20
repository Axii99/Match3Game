using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightPlatform : MonoBehaviour
{
    [SerializeField]
    private Renderer arrowRenderer;
    public GameObject StartPoint;
    public GameObject EndPoint;


    // Start is called before the first frame update
    void Start() {
        arrowRenderer = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        StartPoint = this.transform.GetChild(1).gameObject;
        EndPoint = this.transform.GetChild(2).gameObject;
        arrowRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update() {


    }

    private void OnMouseUpAsButton() {
        Game_Manager.Instance.SelectPlatform(this);
    }

    public void Select() {
        arrowRenderer.enabled = true;
        Debug.Log(this.name + "Selected");
    }

    public void Unselect() {
        arrowRenderer.enabled = false;
        Debug.Log(this.name + "UnSelected");
    }
}
