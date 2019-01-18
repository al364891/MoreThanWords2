using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCalculate : MonoBehaviour {


	public LayerMask enemyLayer;
	private CharacterController2D controller;
	private Player player; 
	public Health health;
	public float kallumAtttackRange = 2f;
	public Animator animator;
	int direction;
	public bool isDeathCheck = false;
	Rigidbody2D Rb;

	private float comboCDStart = 0.3f; //attackCombo
	private float comboCD;

	private bool hittedEnemy = false;
	private bool hitParry = false;
	private bool playerIsStunned = false;

    private bool flash;
    private float flashCounter;
    [SerializeField] private float flashLength;
    [SerializeField] private int parts;
    private Transform[] children;


    void Start()
    {
        controller = GetComponent<CharacterController2D> ();
        player = GetComponent<Player> ();
        comboCD = comboCDStart;
        Rb = GetComponent<Rigidbody2D> ();
        flash = false;
        flashLength = 1f;
        children = new Transform[parts];
        for (int i = 0; i < parts; i += 1)
        {
            if (i < parts - 1)
            {
                children[i] = this.transform.GetChild(i);
            }
            else
            {
                children[i] = this.transform.Find ("Bones");
                //print (children[i].parent);
            }
        }
        /*foreach (Transform child in children)
        {
            print (child.name);
        }*/
    }


	void Update()
	{
		if (!isDeathCheck) //if we are alive
		{
			if (health.CurrentValue <= 0) //if we die we activate the boolean (to die only 1 time)
			{
				SetDeathState();
				isDeathCheck = true;
			}

			if (player.GetPlayerIsAttacking()) //combo calcules
			{
				if (comboCD > 0)
					comboCD -= Time.deltaTime;
				else //finished
				{
					if (animator.GetBool("AttackCombo"))
					{						
						comboCD = comboCDStart;
						animator.SetBool("AttackCombo", false);
						//sound
						if (hittedEnemy) {FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(0);}  //play impact enemy sound with a random pitch	
						else {FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(1);} //play air sound with a random pitch	
					}
					else
					{						
						animator.SetBool("Attacking", false);
						player.SetPlayerIsAttacking(false);
						hittedEnemy = false;
					}
				}
			}

			if (!player.GetPlayerIsCovering())
			{
				bool attackButton = Input.GetButtonDown("Attack");

				//check if we can play sound
				//if we can, play it randomly

				if (attackButton && player.horizontalMove == 0 && Rb.velocity.y == 0  && !playerIsStunned) //if press attack 
				{					
					CalculateImpact(); //raycast and hit handler
					if (!player.GetPlayerIsAttacking()) //and we were on idle
					{			
						player.SetPlayerIsAttacking(true);
						animator.SetBool("Attacking", true);
						comboCD = comboCDStart;
						//sound
						if (hittedEnemy) 
						{
							if (hitParry)
							{
								FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(2); //parry
							}
								
							else
							{
								FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(0); //impact
							}								
						}  
						else 
						{
							FindObjectOfType<AudioManager>().PlaySoundWithRandomPitch(1);
						} //play air sound with a random pitch	
					} else //if we press in the middle of an attack
					{						
						animator.SetBool("AttackCombo", true);
					}
				}

			}

            // Make Kallum flash if he's been hit.
            if (flash == true)
            {
                flashCounter = this.GetComponent<Flash>().PlayerFlash (flashCounter, flashLength, children);
                /*if (flashCounter > flashLength * 0.66f)
                {
                    for (int i = 0; i < sprites.Length; i += 1)
                    {
                        sprites[i].enabled = false;
                    }
                }
                else if (flashCounter > flashLength * 0.33f)
                {
                    for (int i = 0; i < sprites.Length; i += 1)
                    {
                        sprites[i].enabled = false;
                    }
                }*/

                if (flashCounter < 0)
                {
                    flash = false;
                }
            }
		}


		//Test health

		if (Input.GetKeyDown(KeyCode.O))
		{
			health.CurrentValue -= 20;
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			health.CurrentValue += 20;
		}

		//Test audio
		if (Input.GetKeyDown(KeyCode.T))
		{
			FindObjectOfType<AudioManager>().ReduceMusicVolume(3f);
		}
		hitParry = false;
	}

	public void CalculateImpact ()
	{
		if (controller.m_FacingRight) //direction of the ray
			direction = 1;
		else
			direction = -1;
		
		RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.right * direction, kallumAtttackRange, enemyLayer);
		//Debug.DrawRay(transform.position, Vector2.right, Color.red);
		if (hitInfo.collider != null)
		{			
			GiveDamage(hitInfo.collider.gameObject.GetComponent<Enemy>());
			hittedEnemy = true;
		} else
			hittedEnemy = false;
	}
		
	//FindObjectOfType<AudioManager>().PlayAttackSound(0); //play impact enemy sound with a random pitch	
	void GiveDamage (Enemy enemy) //calls an enemy function to rest life depending on both character magic stats
	{
		int damageGiven;
		switch (enemy.enemyMagic)
		{
		case (Enemy.enemyType.ICE):
			{
				if (player.activeMagic.ToString() == "ICE") //worst combination
				{
					damageGiven = 0;
					//stun
					hitParry = true;
					StartCoroutine("GetStunnedByEnemy");
					StartCoroutine(player.ShortAnimationPlay("AttackFail")); //set parry animation
				}					
				else if (player.activeMagic.ToString() == "FIRE") //best combination
					damageGiven = 20; 
				else //player is neutral
					damageGiven = 5;
			}
			break;
		case (Enemy.enemyType.FIRE):
			{
				if (player.activeMagic.ToString() == "FIRE") //worst combination
				{
					damageGiven = 0;
					//stun
					hitParry = true;
					StartCoroutine("GetStunnedByEnemy");
					StartCoroutine(player.ShortAnimationPlay("AttackFail")); //set parry animation
				}					
				else if (player.activeMagic.ToString() == "ICE") //best combination
					damageGiven = 20;
				else //player is neutral
					damageGiven = 5;			
			}
			break;
		default: //NEUTRAL
			{
				if (player.activeMagic.ToString() == "FIRE" || player.activeMagic.ToString() == "ICE") 
					damageGiven = 15;
				else //player is neutral
					damageGiven = 10;				
			}
			break;
		}
		enemy.TakeDamage(damageGiven);
	}


	public void RecieveDamage (Enemy enemy) //not already in use, this is for the enemies in a future
	{
		int damage;
		switch (enemy.enemyMagic)
		{
		case (Enemy.enemyType.NEUTRAL):
			{
				damage = 10;
			}
			break;
		case (Enemy.enemyType.ICE):
			{
				if (player.activeMagic.ToString() == "ICE")
					damage = 20;
				else
					damage = 15;								
			}
			break;
		default: //FIRE
			{
				if (player.activeMagic.ToString() == "FIRE")
					damage = 20;
				else
					damage = 15;					
			}
			break;
		}

		//animator
		StartCoroutine(player.ShortAnimationPlay("Hit"));
		health.CurrentValue -= damage;

        flash = true;
        flashCounter = flashLength;
	}

	void SetDeathState()
	{
		//sound
		FindObjectOfType<AudioManager>().deathSound(); //mute music for 4.5f, stop all other sounds and play death sound
		animator.SetBool("Death", true);
		this.gameObject.layer = 0;
		this.gameObject.tag = "Untagged";
	}

	private IEnumerator GetStunnedByEnemy() //set a player state that avoid attacking while is stunned
	{
		playerIsStunned = true;
		yield return new WaitForSeconds (0.6f);
		playerIsStunned = false;
	}
}