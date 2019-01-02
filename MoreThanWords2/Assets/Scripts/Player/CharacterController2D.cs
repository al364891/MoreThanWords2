﻿using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // Amount of movement smooth
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.   
    [SerializeField] private bool m_AirControl;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.    

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is touching the ground.

    private Rigidbody2D m_Rigidbody2D; // Player physics
    public bool m_FacingRight = true; // For determining which way the player is currently facing.
    private Vector3 velocity = Vector3.zero;

    [HideInInspector] public bool bouncing;
    private float bounceVelocityX;

    [HideInInspector] public bool finished;

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		m_Grounded = false;
        m_AirControl = true;
        bouncing = false;
        finished = false;
        /* //FOR THE EVENT
		if (eventName == null)
			eventName = new UnityEvent();
		*/
    }


    private void FixedUpdate ()
    {
        m_Grounded = false;        

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// We should do this using layers, but this way Sample Assets will not overwrite your project settings
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;

                if (colliders[i].gameObject.tag != "Italics")
                {
                    bouncing = false;
                }
            }
        }
    }

	public void Move(float move, bool jump)
    {
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector3 targetVelocity;
            float movement = move * 10f;
            if (bouncing == true)
            {
                if (Mathf.Sign(movement) != Mathf.Sign(bounceVelocityX) && Mathf.Abs(movement) >= Mathf.Abs(bounceVelocityX))
                {
                    bounceVelocityX = 0f;
                }
                targetVelocity = new Vector2(movement + bounceVelocityX, m_Rigidbody2D.velocity.y);
            }
            else
            {
                targetVelocity = new Vector2(movement, m_Rigidbody2D.velocity.y);
            }
            // And then smoothing it out and applying it to the character
            if (finished == true)
            {
                m_Rigidbody2D.velocity = new Vector2 (0, m_Rigidbody2D.velocity.y);
            }
            else
            {
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
            }

			// If the input is moving the player right and the player is facing left or if the input is moving the player left and the player is facing right...
			if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
            {
                // ... flip the player.
                Flip();
            }
        }
        // If the player should jump...
        if (m_Grounded && jump && finished == false)
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));      //crear fuera cuando tengas ganas de vivir      
        }			
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0,-180,0);
    }


    // Avanzará siempre hacia el lado en el que se movía el jugador cuando comenzó el rebote, pero se puede ajustar la velocidad a la que se mueve horizontalmente con las teclas de movimiento.
    public void Bounce (float verticalForce)
    {
        bouncing = true;

        bounceVelocityX = m_Rigidbody2D.velocity.x;
        m_Rigidbody2D.velocity = new Vector2 (bounceVelocityX, verticalForce);
    }
}