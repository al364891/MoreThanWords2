using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int enemyHealth = 100;
    public int points = 10;
	public enum enemyType {NEUTRAL, FIRE, ICE};
	public enemyType enemyMagic;
    private Rigidbody2D rb;
    private Player player;
    /*private NPCPatrolController2D controller;
    private int direction;
    public float giantAttackRange;
    public LayerMask playerLayer;
    [SerializeField] private Player player;
    private AttackCalculate calculations;*/

    private GameObject pointManager;

    void Start()
    {
        pointManager = GameObject.FindGameObjectWithTag("Manager");
        rb = this.GetComponent<Rigidbody2D> ();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player> ();

        /*calculations = player.GetComponent<AttackCalculate>();
        controller = this.GetComponent<NPCPatrolController2D>();
        playerLayer = LayerMask.GetMask ("Player");*/
    }

    /*
    void Update ()
    {
        if (controller.m_FacingRight) //direction of the ray
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        hitInfo = Physics2D.Raycast(transform.position, Vector2.right * direction, giantAttackRange, playerLayer);
    }*/

    public void TakeDamage(int damage)
	{
        if (enemyHealth > 0)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                pointManager.GetComponent<ScoreManagerScript>().AddKillPoints(points);
                this.GetComponent<NPCPatrolMovement>().Death();
                //print(damage + " damage taken!");
            }

            if (player.transform.position.x < this.transform.position.x)
            {
                rb.AddForce (new Vector2 (50, 50));
            }
            else
            {
                rb.AddForce (new Vector2 (-50, 50));
            }
        }
	}


   /*public void CalculateImpact()
    {
        if (controller.m_FacingRight) //direction of the ray
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.right * direction, giantAttackRange, playerLayer);
        Debug.DrawRay(transform.position, Vector2.right, Color.green);
        if(hitInfo.collider.gameObject.CompareTag ("Player"))
        {
            calculations.RecieveDamage (this);
            print("hit");
        }
    }*/
}