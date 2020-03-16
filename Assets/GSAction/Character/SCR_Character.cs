﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Character : MonoBehaviour {
	public const float GRAVITY = 10.0f;
	public const float CHAR_SIZE = 0.6f;
	public const float OFFSET_X = 1.5f;
	public const float OFFSET_Y = 1;
	public const float START_SPEED = 10;
	public const float BASE_ANGLE_DIFFERENCE = 25;
	
	public float oldX = 0;
	public float x = 0;
	public float y = 0;
	public float angle = 0;
	public float speedX = 0;
	public float speedY = 0;
	
	
    private void Start() {
        
    }
	
	public void Init() {
		oldX = -SCR_Action.SCREEN_W * 0.5f;
		x = -SCR_Action.SCREEN_W * 0.5f + OFFSET_X;
		y = OFFSET_Y;
		transform.position = new Vector3(x, y, 0);
		
		speedX = START_SPEED;
	}

    private void Update() {
        float dt = Time.deltaTime;
		float newTerrainY = SCR_Action.instance.GetTerrainHeightAtX(x);
		float oldTerrainY = SCR_Action.instance.GetTerrainHeightAtX(oldX);
		
		angle = CalculateAngle(speedX, speedY);
		float terrainAngle = CalculateAngle(x - oldX, newTerrainY - oldTerrainY);
		
		
		y += speedY * dt;
		speedY -= GRAVITY * dt;
		
		if (y <= newTerrainY + CHAR_SIZE) {
			float angleDifference = Mathf.Abs(angle - terrainAngle);
			float speedBonus = (BASE_ANGLE_DIFFERENCE - angleDifference) / (90 - BASE_ANGLE_DIFFERENCE); // (0.3 -> -1)
			float combineSpeed = Mathf.Sqrt (speedX * speedX + speedY * speedY);
			
			combineSpeed += combineSpeed * (speedBonus / 2) * dt;
			
			speedX = combineSpeed * Mathf.Cos(terrainAngle * Mathf.Deg2Rad);
			speedY = combineSpeed * Mathf.Sin(terrainAngle * Mathf.Deg2Rad);
			
			angle = terrainAngle;
			y = newTerrainY + CHAR_SIZE;
		}
		
		
		
		transform.position = new Vector3(transform.position.x, y, 0);
		transform.localEulerAngles = new Vector3(0, 0, angle);
    }
	
	private float CalculateAngle(float distX, float distY) {
		if (distX == 0) {
			if (distY < 0) {
				return -90;
			}
			return 90;
		}
		return Mathf.Atan(distY / distX) * Mathf.Rad2Deg;
	}
}