﻿using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject: MonoBehaviour
{

	[Range(1f, 50f)] public float gravityModifier;
	public Vector2 velocity;

	public float minGroundNormalY = .65f;
	public Rigidbody2D rb;

	protected Vector2 targetVelocity;
	protected bool grounded;
	protected Vector2 groundNormal;
	protected const float minMoveDistance = 0.001f;
	protected ContactFilter2D contactFilter;
	protected RaycastHit2D[] hitBuffer =new RaycastHit2D[16];
	protected const float shellRadius = 0.15f;
	protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    // Start is called before the first frame update
    
	void Awake(){
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
    {
		contactFilter.useTriggers = false;
		contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
		contactFilter.useLayerMask = true;
	}

    // Update is called once per frame
    void Update()
    {
		targetVelocity = Vector2.zero;   
		ComputeVelocity();
    }

	protected virtual void ComputeVelocity(){}

	void FixedUpdate(){
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x;

		grounded = false;

		Vector2 deltaPosition = velocity * Time.deltaTime;

		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

		Vector2 move = moveAlongGround * deltaPosition.x;
		
		Movement(move, false);
		
		move = Vector2.up * deltaPosition.y;

		Movement(move, true);

	}


	protected void Movement(Vector2 move, bool yMovement){
		float distance = move.magnitude;

		if(distance > minMoveDistance){
			int count = rb.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
			hitBufferList.Clear();
			for (int i = 0; i < count; i++){
				hitBufferList.Add(hitBuffer[i]);
			}

			for(int i = 0; i < hitBufferList.Count; i++){
				Vector2 currentNormal = hitBufferList[i].normal;
				if(currentNormal.y > minGroundNormalY){
					grounded = true;
					if(yMovement){
						groundNormal = currentNormal;
						currentNormal.x = 0;
					}
				}

				float projection = Vector2.Dot(velocity, currentNormal);
				if(projection < 0){
					velocity = velocity  - projection * currentNormal;
				}

				float modifiedDistance = hitBufferList[i].distance - shellRadius;
				distance = modifiedDistance < distance ? modifiedDistance : distance;
			}
		}

		rb.position = rb.position + move.normalized * distance;
	}



}
