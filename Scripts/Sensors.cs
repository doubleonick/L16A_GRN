using UnityEngine;
using System.Collections;

public class Sensors : MonoBehaviour {
	//Vector3 eyePos = new Vector3(0.0f, 0.0f, 0.0f);
	//Vector3 eyeScale = new Vector3(1.0f, 1.0f, 1.0f);
	//float increment;
	float sensorIncRad = 0.3927f;
	float velocityX;
	float velocityY;
	static int numParts = 18;
	Transform[] parts = new Transform[numParts];
	public Vector2[] velocities = new Vector2[numParts];
	Vector3[] positions = new Vector3[numParts];
	
	// Use this for initialization
	void Start () {
		
		//increment = 0.0f;
		//this.transform.position = eyePos;
		for(int i = 0; i < numParts - 2; i++){
			velocityX = Mathf.Cos(sensorIncRad * i);
			velocityY = Mathf.Sin(sensorIncRad * i);
			velocities[i] = new Vector2(velocityX, velocityY) * Random.Range(0.0f, 1.0f);
			parts[i] = this.transform.Find("Part" + i.ToString());
			positions[i] = new Vector3(0.0f, 0.0f, 1.0f);
		}
		velocities[16] = new Vector2(0, 1) * Random.Range(0.0f, 1.0f);
		velocities[17] = new Vector2(0, -1) * Random.Range(0.0f, 1.0f);
		parts[16] = this.transform.Find("Part" + 16.ToString());
		positions[16] = new Vector3(0.0f, 0.0f, 1.0f);
		parts[17] = this.transform.Find("Part" + 17.ToString());
		positions[17] = new Vector3(0.0f, 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
		//this.transform.localScale = eyeScale * increment;
		for(int i = 0; i < numParts; i++){
			parts[i].GetComponent<Rigidbody2D>().velocity = velocities[i];
//			sensorX = Mathf.Cos(sensorIncRad * i);
//			sensorY = Mathf.Sin(sensorIncRad * i);
//			velocities[i].x = sensorX * Time.deltaTime;
//			velocities[i].y = sensorY * Time.deltaTime;
////			eyePos.x = sensorX * 2.0f * Time.deltaTime;
////			eyePos.y = sensorY * 2.0f * Time.deltaTime;
//			parts[i].transform.Translate(velocities[i]);
			//this.transform.position = eyePos;
		}
	}
}
