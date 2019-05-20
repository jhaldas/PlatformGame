﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	private float speed = 20f;
	private float damage = 50f;

	public Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Enemy"){
			GameObject enemy = col.gameObject;
			enemy.GetComponent<EnemyHandler>().TakeDamage(damage);
		}
		Destroy(gameObject);
	}
}
