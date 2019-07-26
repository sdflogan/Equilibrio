using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour {

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	public Rigidbody _rb;

	void Update() {

		if (_rb.velocity.y < 0) 
			_rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		else if (_rb.velocity.y > 0)// && !isJumping) 
			_rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
	}
}
