using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight4 : MonoBehaviour
{
    [SerializeField]
    private Knight knight;
    private Animator animator;
    private float basic_damage;
    private float basic_speed;
    // Start is called before the first frame update
    void Start() {
        knight = this.GetComponent<Knight>();
        animator = knight.animator;
        basic_damage = knight.attackDamage;
        basic_speed = knight.speed;
        knight.stealth = true;
    }

    // Update is called once per frame
    void Update() {
        if (knight != null) {
            
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Knight4_stealth") && (knight.currentHealth == knight.maxHealth)) {
                //knight.stealth = true;
                //Debug.Log("knight4 walking");
                Color temp = this.GetComponent<SpriteRenderer>().color;
                temp.a = 0.6f;
                this.GetComponent<SpriteRenderer>().color = temp;
                
                if (knight.attackDamage < 2 * basic_damage) {
                    knight.attackDamage = 2 * basic_damage;
                }
            }
            else {
                Color temp = this.GetComponent<SpriteRenderer>().color;
                temp.a = 1f;
                this.GetComponent<SpriteRenderer>().color = temp;
                if (knight.currentHealth < knight.maxHealth) {
                    knight.stealth = false;
                    knight.attackDamage = basic_damage;
                    knight.speed = basic_speed/2.0f;
                }
            }
        }
    }
}
