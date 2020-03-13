using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Terrain : MonoBehaviour {
	public const int 	ANCHOR_NUMBER 			= 16;
	public const float 	WIDTH_RATIO 			= 6;
	public const float 	HEIGHT_RATIO 			= 2;
	public const float 	OFFSET_HEIGHT 			= 1.7f;
	public const float 	MIN_HEIGHT 				= 0.7f;
	public const float 	MAX_HEIGHT 				= 0.9f;
	public const int 	INTERPOLATE_NUMBER 		= 100;
	
	public MeshFilter meshFilter;
	public LineRenderer lineRenderer;
	
	private Mesh mesh;
	private float height = 0;
	private float width = 0;
	private SCR_Spline2D spline;
	
	private SCR_Character characterScript;
	private List<float> heightMap = new List<float>();
	
    private void Start() {
		characterScript = SCR_Action.instance.character.GetComponent<SCR_Character>();
    }

    private void Update() {
		if (characterScript == null) {
			characterScript = SCR_Action.instance.character.GetComponent<SCR_Character>();
		}
		
        float dt = Time.deltaTime;
		float x = transform.position.x;
		float y = transform.position.y;
		
		transform.position = new Vector3(x - characterScript.speedX * dt, y, 0);
		
		if (transform.position.x < -width - SCR_Action.SCREEN_W) {
			SCR_Action.instance.SpawnNextTerrain(true);
			gameObject.SetActive (false);
		}
    }
	
	
	public void Init(float firstPoint, float startX, bool forceUpdate = false) {
		if (mesh == null) {
			mesh = new Mesh();
			meshFilter.mesh = mesh;
			width = SCR_Action.SCREEN_H * WIDTH_RATIO;
			height = SCR_Action.SCREEN_H * HEIGHT_RATIO;
		}
		
		spline = new SCR_Spline2D();
		spline.AddPoint (new Vector2(0, firstPoint));
		for (int i = 1; i < ANCHOR_NUMBER; i++) {
			spline.AddPoint (new Vector2(1.0f * i * INTERPOLATE_NUMBER, Random.Range(MIN_HEIGHT, MAX_HEIGHT) * height));
		}
		
		heightMap.Clear();
		heightMap.Add (firstPoint);
		for (int i = 1; i < ANCHOR_NUMBER * INTERPOLATE_NUMBER; i++) {
			Vector2 middlePoint = spline.Interpolate (i * 1.0f / INTERPOLATE_NUMBER / ANCHOR_NUMBER);
			heightMap.Add (middlePoint.y);
		}
		lineRenderer.positionCount = heightMap.Count;
		
		
		List<Vector3> positions = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
        List<int> triangles = new List<int>();
		
		int offset = 0;
		float step = width / heightMap.Count;
        for (int i = 0; i < heightMap.Count; i++) {
            offset = i * 4;
            
            float h = heightMap[i];
            
            //create the 4 vertices we will use to create the 2 triangles below:
            positions.Add(new Vector3((i + 0) * step, 0, 0)); //lower left - at index 0
            positions.Add(new Vector3((i + 1) * step, 0, 0)); //lower right - at index 1
            positions.Add(new Vector3((i + 0) * step, h, 0)); //upper left - at index 2
            positions.Add(new Vector3((i + 1) * step, h, 0)); //upper right - at index 3
			
			uv.Add(new Vector2((i + 0) * 1.0f / heightMap.Count, 0)); //lower left - at index 0
            uv.Add(new Vector2((i + 1) * 1.0f / heightMap.Count, 0)); //lower right - at index 1
            uv.Add(new Vector2((i + 0) * 1.0f / heightMap.Count, 1)); //upper left - at index 2
            uv.Add(new Vector2((i + 1) * 1.0f / heightMap.Count, 1)); //upper right - at index 3
			
			//uv.Add(new Vector2((i + 0) * 1.0f / heightMap.Count, 0)); //lower left - at index 0
            //uv.Add(new Vector2((i + 1) * 1.0f / heightMap.Count, 0)); //lower right - at index 1
            //uv.Add(new Vector2((i + 0) * 1.0f / heightMap.Count, h / height)); //upper left - at index 2
            //uv.Add(new Vector2((i + 1) * 1.0f / heightMap.Count, h / height)); //upper right - at index 3
			
            
            //triangle 1:
            triangles.Add(offset + 0);
            triangles.Add(offset + 2);
            triangles.Add(offset + 1);
            
            //triangle 2:
            triangles.Add(offset + 1);
            triangles.Add(offset + 2);
            triangles.Add(offset + 3);
			
			
			
			lineRenderer.SetPosition(i, new Vector3((i + 0) * step, heightMap[i], 0.0f));
        }
        
		mesh.Clear();
        mesh.vertices = positions.ToArray();
		mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
		
		transform.position = new Vector3(startX, -SCR_Action.SCREEN_H * OFFSET_HEIGHT, 0);
		
		// Force 1 update
		if (forceUpdate) {
			Update();
		}
	}
	
	public float GetLastHeight() {
		return heightMap[heightMap.Count - 1];
	}
	
	public float GetLastX() {
		return transform.position.x + width;
	}
	
	public float GetHeightAt(float x) {
		if (x >= transform.position.x && x <= transform.position.x + width) {
			float ratio = (x - transform.position.x) / width;
			return spline.Interpolate(ratio).y + transform.position.y;
		}
		return -1;
	}
}