
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCGiantAttack : Attacker
{
    private NPCPatrolController2D controller;
    private int direction;
    public float giantAttackRange;
    public LayerMask playerLayer;
    private Player player;
    private AttackCalculate calculations;
    private RaycastHit2D hitInfo;
    private float x;
    private float y;


    void Start ()
    {
        player = this.GetComponent<Enemy>().player;
        calculations = player.GetComponent<AttackCalculate> ();
        controller = this.GetComponent<NPCPatrolController2D> ();
        playerLayer = LayerMask.GetMask ("Player");
    }


    public override void Attack (float x, float y, float direction)
    {
        this.x = x;
        this.y = y + this.gameObject.GetComponent<CircleCollider2D>().radius;

        Invoke ("CalculateImpact", 0.8f);
		FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(4); //giantSound
    }


    public void CalculateImpact ()
    {
        //Debug.Log(x + " " + y);
        if (Vector2.Distance(new Vector2(x, y), new Vector2(player.transform.position.x, player.transform.position.y)) <= 4.5 && ((controller.m_FacingRight && player.transform.position.x >= x) || !controller.m_FacingRight && player.transform.position.x <= x))
        {
            //Debug.Log("get Hit");
            if (controller.m_FacingRight) //direction of the ray
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            Vector3 position = transform.position;
            position.y -= 30;

            if ((((int)Mathf.Abs(player.gameObject.transform.rotation.eulerAngles.y) >= (int)178 && controller.m_FacingRight) || ((int)Mathf.Abs(player.gameObject.transform.rotation.eulerAngles.y) == (int)0 && !controller.m_FacingRight)) && player.GetPlayerIsCovering())
            {
                //BLOCKED
                //print("Blocked: Enemy Facing Right = " + controller.m_FacingRight + ", Player Facing Right: " + (player.gameObject.transform.rotation.eulerAngles.y == -180));
                FindObjectOfType<AudioManager>().Play("arrowShield");
            }
            else
            {
                player.gameObject.GetComponent<AttackCalculate>().RecieveDamage(this.gameObject.GetComponent<Enemy>());
            }

            //calculations.RecieveDamage(this.GetComponent<Enemy>());
            //Invoke ("DoDamage", );
            //print ("hit");
        }
    }
}