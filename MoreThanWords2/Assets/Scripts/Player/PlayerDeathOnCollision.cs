
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerDeathOnCollision : MonoBehaviour
{
    GameObject collided;


    private void OnCollisionEnter2D (Collision2D collision)
    {
        collided = collision.gameObject;

        if (collided.tag == "Player")
        {
            collided.GetComponent<AttackCalculate>().health.CurrentValue = 0;
            collided.GetComponent<CharacterController2D>().bouncing = false;
        }
    }
}