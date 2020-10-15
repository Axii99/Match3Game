using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight3 : MonoBehaviour
{
    [SerializeField]
    private Knight knight;
    private float basic_damage;
    // Start is called before the first frame update
    void Start()
    {
        knight = this.GetComponent<Knight>();
        basic_damage = knight.attackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (knight != null) {
            //knight.attackDamage = basic_damage + Mathf.Round((knight.maxHealth - knight.currentHealth)/6);
            knight.attackDamage = Mathf.Min(knight.attackDamage, 25);
            if (knight.armor < -10) {
                knight.armor = -10;
            }
        }
    }
}
