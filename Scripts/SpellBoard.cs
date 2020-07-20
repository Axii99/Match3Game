using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBoard : MonoBehaviour
{
    public int SpellNum = 6;
    public Spell[] allSpells;


    // Start is called before the first frame update
    void Start()
    {
        CreateAllSpellButton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateAllSpellButton() {
        foreach(Spell sp in allSpells) {
            Spell temp = Instantiate(sp);
            temp.transform.SetParent(this.gameObject.transform);
            temp.transform.localScale = new Vector3(1, 1, 1);
           
        }
    }
}
