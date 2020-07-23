using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    [SerializeField] GameObject redLaser;
    [SerializeField] GameObject greenLaser;
    [SerializeField] GameObject blueLaser;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (collision.gameObject.name == "laserRed01 (Standard Projectile)(Clone)")
        {
            Destroy(collision.gameObject);
        }
        */
        Destroy(collision.gameObject);
    }
}
