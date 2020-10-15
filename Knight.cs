using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
  
    //Attributes
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public float armor;
    public float attackDamage;
    public float attackReduceArmor;
    public float attackRate = 1f;

    public bool stealth = false;
    public bool CrossPlatformAttack = false;
    public float nextAttackTime = 0f;
    public float attackRange = 0.6f;
    public bool isEnemy;
    public LayerMask EnemyMask;

    /// <summary>
    /// /////////////
    /// </summary>
    public GameObject StartPoint;
    public GameObject EndPoint;
    public GameObject target;


    //Anim & Audio
    public Animator animator;
    public string spawnAudio;
    public string attackAudio;
    public string hitAudio;

    // Start is called before the first frame update

    private void Awake() {
        currentHealth = maxHealth;
    }

    void Start()
    {
        currentHealth = maxHealth;
        transform.position = StartPoint.transform.position;
        AudioManager.Instance.PlaySound(spawnAudio);
        target = null;
        //Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GamePause) {
            return;
        }



        SearchEnemy();
        if ((target != null) && (!stealth)) {
            animator.SetFloat("Speed", 0);
            if (Time.time >= nextAttackTime) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;

            }

        }
        else {
            nextAttackTime = Time.time + 1f / attackRate;
            if (Vector2.Distance(EndPoint.transform.position, this.transform.position) > 0.1f) {
                Walk();
            }
            else {
                animator.SetFloat("Speed", 0);
                Game_Manager.Instance.TakeDamage(isEnemy, currentHealth/maxHealth * 150);
                if (isEnemy) {
                    Game_Manager.Instance.RemoveEnemyKnight(this);
                }
                else {
                    Game_Manager.Instance.RemovePlayerKnight(this);
                }
                Destroy(this.gameObject);
            }
        }

        if (currentHealth <= 0) {
            Die();
        }
    }


    public void Walk() {
        if (stealth) {
            animator.SetTrigger("Stealth");
        }
        else {
            //Debug.Log("walking");
            animator.SetFloat("Speed", speed);
        }
        this.GetComponent<SpriteRenderer>().flipX = false;
        transform.position = Vector2.MoveTowards(transform.position, EndPoint.transform.position, Time.deltaTime * speed / 10);
    }

    public void SearchEnemy() {
        Collider2D[] inRangeEnemies = Physics2D.OverlapCircleAll(this.transform.position, attackRange, EnemyMask);

        float dist = -1;
        GameObject temp_target = null;
        foreach(Collider2D enemy in inRangeEnemies) {
            //Debug.Log("Find: " + enemy.name);
            if (enemy.GetComponent<Knight>().EndPoint != StartPoint && !CrossPlatformAttack) {
                continue;
            }
            if (target!= null && target == enemy.gameObject) {
                return;
            }
            float temp_dist = Vector2.Distance(this.transform.position, enemy.transform.position);
            if (dist < 0) {
                dist = temp_dist;
                temp_target = enemy.gameObject;
            }
            else if (temp_dist < dist) {
                dist = temp_dist;
                temp_target = enemy.gameObject;
            }
        }
        target = temp_target;

        if (target != null) {
            //break stealth
            if (stealth && (temp_target.GetComponent<Knight>().target == this.gameObject)) {
                Debug.Log(this.gameObject.name +" XXXX " +temp_target.GetComponent<Knight>().target.name);
                stealth = false;
            }
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

    public void Attack() {
        //Debug.Log(this.name + " is attacking");
        if (currentHealth > 0) {
            if((target.transform.position.x < this.transform.position.x && !isEnemy) || (target.transform.position.x > this.transform.position.x && isEnemy)) {
                this.GetComponent<SpriteRenderer>().flipX = true;
            }
            else {
                this.GetComponent<SpriteRenderer>().flipX = false;
            }
            animator.SetTrigger("Attack");
        }
        animator.SetFloat("Speed", 0);
        if (target != null) {
            AudioManager.Instance.PlaySound(attackAudio);
            if (attackReduceArmor > 0) {
                target.GetComponent<Knight>().ReduceArmor(attackReduceArmor);
            }
            target.GetComponent<Knight>().TakeDamage(attackDamage);

        }
    }

    public void TakeDamage(float damage) {
        //Debug.Log(curretHealth);
        float temp_damage = damage - armor;
        if (temp_damage <= 0) {
            temp_damage = 1;
        }
        AudioManager.Instance.PlaySound(hitAudio);
        currentHealth -= temp_damage;
        if (currentHealth <= 0) {
            //Die();
        }
    }

    public void ReduceArmor(float amount) {
        //Debug.Log(curretHealth);
        armor -= amount;
        if (armor < -20) {
            armor = -20;
        }
    }

    void Die() {
        if (isEnemy) {
            Game_Manager.Instance.RemoveEnemyKnight(this);
        }
        else {
            Game_Manager.Instance.RemovePlayerKnight(this);
        }
        animator.SetTrigger("Die");
        Destroy(this.gameObject, 1f);
       
        //Debug.Log("Die!");
        
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    public IEnumerator ChangeAttribute(string att, float value, float time) {
        if (time > 0) {
            yield return new WaitForSeconds(time);
        }
        switch (att) {
            case "attackDamage":
                attackDamage = value;
                break;
            case "attackRange":
                attackRange = value;
                break;
            case "attackRate":
                attackRate = value;
                break;
            case "speed":
                speed = value;
                break;
            case "armor":
                armor = value;
                break;
            default:
                break;
        }
            
    }

}
