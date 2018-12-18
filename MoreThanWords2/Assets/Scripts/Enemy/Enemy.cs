using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int enemyHealth = 100;
    public int points = 10;
	public enum enemyType {NEUTRAL, FIRE, ICE};
	public enemyType enemyMagic;
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
        if(enemyHealth> 0)
        {
            enemyHealth -= damage;
            if (enemyHealth <= 0)
            {
                pointManager.GetComponent<ScoreManagerScript>().AddKillPoints(points);
                this.GetComponent<NPCPatrolMovement>().Death();
                //print(damage + " damage taken!");
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