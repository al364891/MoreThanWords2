using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events; //FOR EVENTS

public class Player : MonoBehaviour
{
	private CameraEffects ppEffects; //post processing
    [SerializeField]
    private Health health;

	private int startHealth;

	private AttackCalculate AttackCalc; //script that makes attack calcules

    public CharacterController2D controller;

	private Animator animator;

	private Rigidbody2D Rb;

	[HideInInspector]public float runSpeed = 40f;
	private float maxSpeed = 30f;

	[HideInInspector] public float horizontalMove = 0f; //from -1 to 1
	bool jump = false;
	private bool playerIsCovering = false;
	private bool isAttacking;

	[HideInInspector] public enum playerMagic {NEUTRAL, FIRE, ICE};
	[HideInInspector] public playerMagic activeMagic;
	[HideInInspector] public playerMagic storedMagic;

    //Time how long magic is active
	[HideInInspector] public float magicTime = 20f;

    //Flag to safe storedmagic
    private string storedMagicFlag = null;

    //important for refreshing Magic when active magic and storedmagic is the same
    //is use in ElementIconEffect
    private bool refreshMagic = false;

    //How many keys are in the map,and how many you have (default 10/10) 
    [SerializeField]
    private Text keyNumber;

	[HideInInspector]public GameObject powerUp;

    [SerializeField]
    private Image elementIcon;

    //numbers of key in on map, has to put manuell
    [SerializeField] private int keyOnMap;

    //numbers of the collected key
	[HideInInspector] public int collectedKey;

	GameObject fireParticles;
	GameObject iceParticles;
	GameObject emberIceParticles;
	GameObject emberFireParticles;

	[HideInInspector]public bool ElementFlag = false;

	private float previousVelocity;

    //private Transition transition;
    private PauseMenu menu;
    private bool restarted;

	/*
	[Header("Events")]
	[Space]

	public UnityEvent cameraShake;

	//invoke it with eventName.Invoke();*/

	void Awake()
	{
		isAttacking = false;
		collectedKey = 0;
		startHealth = 100;
	}

	void Start()
	{
		ppEffects = GetComponent<CameraEffects>();
        iceParticles = GameObject.Find("IceParticles");
		emberIceParticles = GameObject.Find("EmberIceParticles");
		fireParticles = GameObject.Find("FireParticles");
		emberFireParticles = GameObject.Find("EmberFireParticles");
        menu = GameObject.Find("PauseMenuCan").GetComponent<PauseMenu> ();

		animator = GetComponent<Animator>();
		AttackCalc = GetComponent<AttackCalculate>();

        health.setHealth(startHealth, startHealth);
		Rb = GetComponent<Rigidbody2D>();

        restarted = false;

		IceParticles(false);
		FireParticles(false);
    }

	// Update is called once per frame
	void Update ()
	{
		if(previousVelocity != 0 && Rb.velocity.y == 0)
			FindObjectOfType<AudioManager>().Play("floorImpact");
        if (!animator.GetBool("Death"))
        {
            keyNumber.text = collectedKey.ToString() + "/" + keyOnMap.ToString();

            // set element into storedMagic, depends on OntriggerEnter2D and Exit
            if (Input.GetButtonDown("PickUpMagic")) //PICKUPMAGIC
            {
				if (storedMagicFlag == "Fire")
				{
					FindObjectOfType<AudioManager>().Play("fireElementPickUp");
					StartCoroutine(DisablePowerUp(powerUp));
					storedMagic = playerMagic.FIRE;
				} else if (storedMagicFlag == "Ice")
				{
					FindObjectOfType<AudioManager>().Play("iceElementPickUp");
					StartCoroutine(DisablePowerUp(powerUp));
					storedMagic = playerMagic.ICE;
				}
            }

            //if !isAttacking to avoid moving while attacking
            if (controller.finished == true)
            {
                horizontalMove = 0;
            }
            else
            {
                horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed; //outside the if !cover to avoid keep moving when run and cover bug
            }
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            animator.SetFloat("VelocityY", Rb.velocity.y); //detects the Y speed for jump animation
            bool jumpButton = Input.GetButtonDown("Jump");

            bool useMagicButton = Input.GetButtonDown("UseMagic"); //e

	        //When activate magic, check storadMagic and use that magic, if same element is in use set refreshMagic as true (important for ElementIconEffect). After activating set storadmagig to Neutral
            if (useMagicButton)
            {
				if (storedMagic.ToString() == "FIRE")//MAGIC USE
                {
					FindObjectOfType<AudioManager>().Play("fireUse"); 
					FindObjectOfType<AudioManager>().Play("usingMagic");
					StartCoroutine(ShortAnimationPlay("UseMagic"));
					ppEffects.activateChromaticAberration(); //postProcessing effect

                    if (activeMagic.ToString() == "FIRE")
                    {
                        refreshMagic = true;
                    }
                    else
                    {
                        activeMagic = playerMagic.FIRE;
                    }
                }
				else if (storedMagic.ToString() == "ICE")//MAGIC USE
                {
					FindObjectOfType<AudioManager>().Play("iceUse"); 
					FindObjectOfType<AudioManager>().Play("usingMagic");
					StartCoroutine(ShortAnimationPlay("UseMagic"));
					ppEffects.activateChromaticAberration(); //postProcessing effect

                    if (activeMagic.ToString() == "ICE")
                    {
                        refreshMagic = true;
                    }
                    else
                    {
                        activeMagic = playerMagic.ICE;
                    }
                }
                storedMagic = playerMagic.NEUTRAL;
			}

			if (!playerIsCovering)
			{
				if (jumpButton)
					jump = true;
			}
					
				
                if (Input.GetButtonDown("Cover") && horizontalMove == 0 && Rb.velocity.y == 0) //if we press cover and we are not moving
                {
                    playerIsCovering = true;
                    animator.SetBool("IsCovering", true);
                }
			
            if (Input.GetButtonUp("Cover") && playerIsCovering) //stop covering
            {
                playerIsCovering = false;
                animator.SetBool("IsCovering", false);
            }


		}//if (!animator.GetBool("Death"))
        else
        {
            StartCoroutine("Respawn");
			horizontalMove = 0;
			animator.SetFloat("VelocityY", 0); //cutre
        }
		previousVelocity = Rb.velocity.y;
	}

	void FixedUpdate() 
	{
		// Trying to Limit Speed
		if(Rb.velocity.magnitude > maxSpeed){
			Rb.velocity = Vector3.ClampMagnitude(Rb.velocity, maxSpeed);
		}
		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
		////fixedDeltaTime es el tiempo desde la ultima vez que se llamó a la funión (así funciona igual de bien independientemente de cada cuanto tiempo se llame a fixedUpdate (funciona igual a 30fps que a 60fps)
		jump = false;
	}

    
    //set Flag to the element where the player is staying at
	void OnTriggerEnter2D(Collider2D other)
	{
		powerUp = other.gameObject;
		if (other.tag == "PowerUpFire")
		{
			ElementFlag = true;
			storedMagicFlag = "Fire";
			ElementFlag = false;
		}
		if (other.tag == "PowerUpIce")
		{
			ElementFlag = true;
			storedMagicFlag = "Ice";
			ElementFlag = false;
		}
	}


    // set flag to null by exit
    void OnTriggerExit2D() {
		powerUp = null;
        storedMagicFlag = null;
    }

    //actually this is a setter (is in use in ElementIconEffect)
    public void beNeutral() {
        activeMagic = playerMagic.NEUTRAL;
    }

    //getter and setter for refreshmagic
    public bool ReFreshMagic{
        get{ return refreshMagic; }
        set{ refreshMagic = value; }
    }

	public int CollectedKey{
		get{ return collectedKey; }
		set{ collectedKey = value;}
	}

    public bool GetPlayerIsCovering() //information for the enemies
    {
        return playerIsCovering;
    }

	public bool GetPlayerIsAttacking() //information for the enemies
	{
		return isAttacking;
	}

	public void SetPlayerIsAttacking(bool attacking) //information for the enemies
	{
		isAttacking = attacking;
	}


    private IEnumerator Respawn ()
    {
        yield return new WaitForSeconds (3.5f);

        if (restarted == false)
        {
            menu.RestartLevel ();
            restarted = true;
        }
    }

	private IEnumerator DisablePowerUp(GameObject powerUp){
		if (powerUp.tag == "PowerUpIce" && storedMagic.ToString() != "ICE" || powerUp.tag == "PowerUpFire" && storedMagic.ToString() != "FIRE" || storedMagic.ToString() == "NEUTRAL")
		{
			powerUp.SetActive(false);
			storedMagicFlag = null;
			//print("called");
			yield return new WaitForSeconds(5f);
			powerUp.SetActive(true);	
		}

	}

	public IEnumerator ShortAnimationPlay(string name) //activates and deactivates an anim with a little delay to avoid bugs and calcules
	{
		animator.SetBool(name,true);
		yield return new WaitForSeconds (0.3f);
		animator.SetBool(name,false);
	}

	public void IceParticles(bool activated)
	{
		iceParticles.GetComponent<ParticleSystem>().enableEmission = activated;
		emberIceParticles.GetComponent<ParticleSystem>().enableEmission = activated;
	}

	public void FireParticles(bool activated)
	{
		fireParticles.GetComponent<ParticleSystem>().enableEmission = activated;
		emberFireParticles.GetComponent<ParticleSystem>().enableEmission = activated;
	}
}
