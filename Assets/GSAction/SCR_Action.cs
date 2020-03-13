using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_Action : MonoBehaviour {
    // =============================================================
	// =============================================================
	// Instance
	public static SCR_Action 	instance 		= null;
	
	// =============================================================
	// =============================================================
	// Const
	public static float 		SCREEN_RATIO 	= 0;
	public static float 		SCREEN_W 		= 0;
	public static float 		SCREEN_H 		= 0;
	public static float 		SCREEN_SCALE	= 0;
	
	public static float			START_HEIGHT	= 18.0f;
	public static int			RESERVE_TERRAIN = 2;
	
	// =============================================================
	// =============================================================
	// Prefab
	public GameObject 			PFB_Terrain;
	public GameObject 			PFB_Character;
	
	// =============================================================
	// =============================================================
	// Public
	public GameObject			character		= null;
	
	// =============================================================
	// =============================================================
	// Private
	private GameObject			lastTerrain		= null;
	
	// =============================================================
	// =============================================================
	
	
	
    private void Awake() {
        // Must go through loading first
		//if (SCR_Loading.loaded == false) {
		//	SceneManager.LoadScene("GSLoading/SCN_Loading");
		//	return;
		//}
		
		// Singleton
		instance = this;
		
		// Calculate screen resolution
		SCREEN_RATIO = Screen.width * 1.0f / Screen.height;
		SCREEN_H = 10.8f;
		SCREEN_W = 10.8f * SCREEN_RATIO;
		SCREEN_SCALE = Screen.width / SCREEN_W;
		
		// Set camera
		Camera.main.orthographicSize = SCREEN_H * 0.5f;
		//Camera.main.transform.position = new Vector3 (SCREEN_W * 0.5f, SCREEN_H * 0.5f, -10);
		
		SCR_Pool.Flush();
    }
	
	private void Start() {
		character = SCR_Pool.GetFreeObject(PFB_Character);
		character.GetComponent<SCR_Character>().Init();
		
		lastTerrain = SCR_Pool.GetFreeObject(PFB_Terrain);
		lastTerrain.GetComponent<SCR_Terrain>().Init (START_HEIGHT, -SCR_Action.SCREEN_W * 0.5f);
		for (int i=0; i<RESERVE_TERRAIN; i++) {
			SpawnNextTerrain(false);
		}
	}

    private void Update() {
        
    }
	
	public void SpawnNextTerrain(bool forceUpdate = false) {
		GameObject temp = SCR_Pool.GetFreeObject(PFB_Terrain);
		temp.GetComponent<SCR_Terrain>().Init (lastTerrain.GetComponent<SCR_Terrain>().GetLastHeight(), lastTerrain.GetComponent<SCR_Terrain>().GetLastX(), forceUpdate);
		lastTerrain = temp;
	}
	
	public float GetTerrainHeightAtX (float x) {
		List<GameObject> terrainList = SCR_Pool.GetObjectList(PFB_Terrain);
		for (int i=0; i<terrainList.Count; i++) {
			SCR_Terrain script = terrainList[i].GetComponent<SCR_Terrain>();
			if (script.GetHeightAt(x) != -1) {
				return script.GetHeightAt(x);
			}
		}
		return -1;
	}
}
