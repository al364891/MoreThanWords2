using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int enemyHealth = 100;
    public int points = 10;
	public enum enemyType {NEUTRAL, FIRE, ICE};
	public enemyType enemyMagic;
    private Rigidbody2D rb;
    [HideInInspector] public Player player;
    /*private NPCPatrolController2D controller;
    private int direction;
    public float giantAttackRange;
    public LayerMask playerLayer;
    [SerializeField] private Player player;
    private AttackCalculate calculations;*/

    private GameObject pointManager;

    private bool flash;
    private float flashCounter;
    [SerializeField] private float flashLength;
    private GameObject sprites;

    void Start ()
    {
        pointManager = GameObject.FindGameObjectWithTag("Manager");
        rb = this.GetComponent<Rigidbody2D> ();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player> ();

        flash = false;
        flashLength = 0.5f;
        Transform test;
        for (int i = 0; i < this.transform.childCount; i += 1)
        {
            test = this.transform.GetChild (i);
            if (test.name == "Sprites")
            {
                sprites = test.gameObject;
                break;
            }
        }
        /*calculations = player.GetComponent<AttackCalculate>();
        controller = this.GetComponent<NPCPatrolController2D>();
        playerLayer = LayerMask.GetMask ("Player");*/
    }

    
    void Update ()
    {
        // Make the enemy flash if he's been hit.
        if (flash == true)
        {
            flashCounter = this.GetComponent<Flash>().EnemyFlash (flashCounter, flashLength, sprites);

            if (flashCounter < 0)
            {
                flash = false;
            }
        }
    }

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
            else
            {
                flash = true;
                flashCounter = flashLength;
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