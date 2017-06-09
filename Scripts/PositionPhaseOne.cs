using UnityEngine;
using System.Collections;

public class PositionPhaseOne : MonoBehaviour {

	float sensorIncRad = 0.3927f;
	float positionX;
	float positionY;
	float posMagnitude;
	static int numParts = 16;
	Transform[] parts = new Transform[numParts];
	Vector3[] positions = new Vector3[numParts];
	public string currPhase;
	// Use this for initialization
	void Start () {
		string partType;
		posMagnitude = 3;
		for(int i = 0; i < numParts; i++){
			//positionX = Mathf.Cos(sensorIncRad * i);
			//positionY = Mathf.Sin(sensorIncRad * i);
			//positions[i] = new Vector3(positionX * posMagnitude, positionY * posMagnitude, -1.0f);
			if(i%2 == 0){
				partType = "IR" + i.ToString();
			}
			else{
				partType = "LDR" + i.ToString();
			}
			parts[i] = this.transform.Find(partType);
			parts[i].transform.position = positions[i];
		}
		
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		currPhase = Application.loadedLevelName;
		if(currPhase == "PhaseOne"){
			for(int i = 0; i < numParts; i++){
				parts[i].transform.position = positions[i];
			}

		}
	}
}
