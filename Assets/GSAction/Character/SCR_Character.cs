using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Character : MonoBehaviour {
	public const float OFFSET_X = 1.5f;
	public const float OFFSET_Y = 1;
	public const float START_SPEED = 10;
	
	public float x = 0;
	public float y = 0;
	public float angle = 0;
	public float speedX = 0;
	public float speedY = 0;
	
	
    private void Start() {
        
    }
	
	public void Init() {
		x = -SCR_Action.SCREEN_W * 0.5f + OFFSET_X;
		y = OFFSET_Y;
		transform.position = new Vector3(x, y, 0);
		
		speedX = START_SPEED;
	}

    private void Update() {
        float dt = Time.deltaTime;
		float terrainY = SCR_Action.instance.GetTerrainHeightAtX(transform.position.x);
		
		
		
		
		
		transform.position = new Vector3(transform.position.x, terrainY + 0.5f, 0);
    }
}