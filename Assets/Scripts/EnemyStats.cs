using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] GameObject enemyLaserPrefab = default;
    [SerializeField] float health = 100f;
    [SerializeField] float shotCounter;
    [SerializeField] float minCooldownbetweenShots = 0.2f;
    [SerializeField] float maxCooldownbetweenShots = 3f;
    [SerializeField] float enemyLaserSpeed = 10f;
    [SerializeField] AudioClip DeathSound = default;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.5f;
    [SerializeField] AudioClip laserFireSound = default;
    [SerializeField] [Range(0, 1)] float laserFireSoundVolume = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minCooldownbetweenShots, maxCooldownbetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountThenShoot();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer dmgDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!dmgDealer) { return; }

        health = health - dmgDealer.GetDmg();
        dmgDealer.Hit();

        if (health <= 0)
        {
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(DeathSound, Camera.main.transform.position, deathSoundVolume);
    }

    private void CountThenShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minCooldownbetweenShots, maxCooldownbetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaserPrefab,
                transform.position,
                quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -enemyLaserSpeed);
        AudioSource.PlayClipAtPoint(laserFireSound, Camera.main.transform.position, laserFireSoundVolume);
    }

}
