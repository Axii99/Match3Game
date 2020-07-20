using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
  
    //Attributes
    public float maxHealth;
    [SerializeField]
    private float curretHealth;
    public float speed;
    public float attackDamage;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;
    public float attackRange = 0.6f;
    public bool isEnemy;
    public LayerMask EnemyMask;

    /// <summary>
    /// /////////////
    /// </summary>
    public GameObject StartPoint;
    public GameObject EndPoint;
    public GameObject target;

    public Animator animator;

    // Start is called before the first frame update

    private void Awake() {
        curretHealth = maxHealth;
    }

    void Start()
    {
        curretHealth = maxHealth;
        transform.position = StartPoint.transform.position;
        
        target = null;
        Debug.Log("done");
    }

    // Update is called once per frame
    void Update()
    {
        if (curretHealth <= 0) {
            return;
        }

        SearchEnemy();
        if (target != null) {
            if (Time.time >= nextAttackTime) {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
        else {
            if (Vector2.Distance(EndPoint.transform.position, this.transform.position) > 0.1f) {
                Walk();
            }
            else {
                animator.SetFloat("Speed", 0);
                //Game_Manager.Instance.TakeDamage(isEnemy, curretHealth);
                //Destroy(this.gameObject);
            }
        }
    }


    public void Walk() {
        animator.SetFloat("Speed", speed);
        transform.position = Vector2.MoveTowards(transform.position, EndPoint.transform.position, Time.deltaTime * speed);
    }

    public void SearchEnemy() {
        Collider2D[] inRangeEnemies = Physics2D.OverlapCircleAll(this.transform.position, attackRange, EnemyMask);

        float dist = -1;
        GameObject temp_target = null;
        foreach(Collider2D enemy in inRangeEnemies) {
            //Debug.Log("Find: " + enemy.name);
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
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

    public void Attack() {
        animator.SetTrigger("Attack");
        if (target != null) {
            target.GetComponent<Knight>().TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(float damage) {
        Debug.Log(curretHealth);
        curretHealth -= damage;
        if (curretHealth <= 0) {
            Die();
        }
    }

    void Die() {
        animator.SetTrigger("Die");
        Destroy(this.gameObject, 1f);
        Debug.Log("Die!");
        
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }


}
