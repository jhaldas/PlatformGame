﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{	

	public Health enemyHealth;
	public float maxHP = 100;

	private Rigidbody2D rb;
	//public HealthBar bar;

    // Start is called before the first frame update
    void Start()
    {
		rb  = gameObject.GetComponent<Rigidbody2D>();
		enemyHealth = new Health(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
		if(enemyHealth.GetCurrentHP() <= 0){
			Die();
		}
    }

	void Die(){
		Destroy(gameObject);
	}

	public void TakeDamage(float damage){
		enemyHealth.TakeDamage(damage);
	}

	public void Knockback(Vector3 force){
		rb.AddForce(force);
	}



}
