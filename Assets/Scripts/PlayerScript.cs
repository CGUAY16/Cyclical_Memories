using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // config Params
    [Header("Player Stats")] 
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] int health = 1000;
    [SerializeField] AudioClip laserFireSound = default;
    [SerializeField] [Range(0, 1)] float laserFireSoundVolume = 0.25f;
    [SerializeField] AudioClip DeathSound = default;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.5f;

    [Header("Projectile Stats")]
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float laserSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;

    Coroutine firingCoroutine;

    bool isFiring = false;
    float xMin;
    float xMax;
    float yMin;
    float yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMovementBounds();
    }

    private void SetUpMovementBounds()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    
    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    // Player Movement
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;

        var newRestrictedXPos = Mathf.Clamp(newXPos, xMin, xMax);
        var newRestrictedYPos = Mathf.Clamp(newYPos, yMin, yMax);

        transform.position = new Vector2(newRestrictedXPos, newRestrictedYPos);
    }

    //Player Shooting
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isFiring)
            {
                firingCoroutine = StartCoroutine(FireAgainAndAgain());
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
            isFiring = false;
        }
    }

    IEnumerator FireAgainAndAgain()
    {
        isFiring = true;
        while (true){ 
            GameObject laser = Instantiate(laserPrefab,
                transform.position,
                quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserFireSound, Camera.main.transform.position, laserFireSoundVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }   
    }

    private void OnTriggerEnter2D(Collider2D playerContact)
    {
        DamageDealer dmgDealer = playerContact.gameObject.GetComponent<DamageDealer>();
        if (!dmgDealer) { return; } // protection against null reference exception.

        health -= dmgDealer.GetDmg();
        dmgDealer.Hit();
        if(health <= 0)
        {
            PlayerDeath();
        }

    }

    private void PlayerDeath()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, deathSoundVolume);
    }

}
