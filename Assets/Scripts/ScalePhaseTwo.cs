using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class ScalePhaseTwo : MonoBehaviour {

	public bool initialized;

	public string phaseOnePath = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\selectedParts.txt";
	public string[] phaseOneInfo;
	public string currPhase;
	public int numParts;
	public Vector3[] scales; 
	Transform[] parts;
	Vector2[] velocities;	
	
	Transform[] clones;
	
	public int numNeurons;
	GameObject neuronPool;// = new GameObject;
	Transform[] neurons;
	float neuronPosMagnitude;
	float neuronX;
	float neuronY;
	Vector3[] neuronPositions;
	float neuronIncRad;
	// Use this for initialization
	void Start () {
		initialized = false;
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		int n = 0;
		currPhase = Application.loadedLevelName;
		if(currPhase == "PhaseTwo"){
			if(initialized == false){
				PhaseTwoInitialize();
			}
		
			for(i = 0; i < numParts; i++){
				//print ("Appendage updates..." + Environment.NewLine);
				//print ("scale for part " + i + " named, " + parts[i].name + scales[i] + Environment.NewLine);
				parts[i].localScale += scales[i];
				//parts[i].rigidbody2D.velocity = velocities[i];
			}
			for(i = numParts; i < numParts + numNeurons; i++){
				n = i - numParts;
				//print ("scale for part " + i + " named, " + neurons[n].name + scales[i] + Environment.NewLine);
				neurons[n].localScale += scales[i];
			}
		}
		
	}
	
	
	void PhaseTwoInitialize() {
		numParts = 0;
		float velocityX;
		float velocityY;
		float sensorIncRad = 0.3927f;
		float motorX;
		float motorY;
		Vector3 motorPositionDelta;
		if(File.Exists(phaseOnePath))
		{
			int i = 0;	
			
			neuronPool = GameObject.Find("NeuronPool");
			DontDestroyOnLoad(neuronPool);
			numNeurons = neuronPool.transform.childCount;
			neurons = new Transform[numNeurons];
			neuronPositions = new Vector3[numNeurons];
			neuronPosMagnitude = 3.8f;
			neuronIncRad = (Mathf.PI * 2.0f)/numNeurons;
			for(i = 0; i < numNeurons; i++){
				neurons[i] = neuronPool.transform.FindChild("Neuron"+i.ToString());
				neuronPositions[i].x = Mathf.Sin(neuronIncRad * i) * neuronPosMagnitude;
				neuronPositions[i].y = Mathf.Cos(neuronIncRad * i) * neuronPosMagnitude;
				neurons[i].position = neuronPositions[i];
				DontDestroyOnLoad(neurons[i].gameObject);
			}
			
			phaseOneInfo = File.ReadAllLines(phaseOnePath);
			foreach (string line in phaseOneInfo)
			{
				numParts++;
			}
			scales = new Vector3[numParts + numNeurons];
			parts = new Transform[numParts];
			velocities = new Vector2[numParts];
			i = 0;
			foreach (string line in phaseOneInfo){
				print("i = " + i + Environment.NewLine);
				velocityX = Mathf.Cos(sensorIncRad * -i);
				velocityY = Mathf.Sin(sensorIncRad * i);
				velocities[i] = new Vector2(velocityX, velocityY) * UnityEngine.Random.Range(0.0f, 1.0f);
				scales[i] = new Vector3(0.05f, 0.05f, 0.0f) * UnityEngine.Random.Range(1.0f, 2.0f);
				parts[i] = this.transform.Find(line);
				if(line.Contains("Motor") == true){
					motorX = 0;
					if(line == "MotorL"){
						motorY = 4.6f;
					}
					else{
						motorY = -4.6f;
					}
					motorPositionDelta = new Vector3(motorX, motorY, 0.0f);
					parts[i].position = motorPositionDelta;
				}
				
				i++;
			}
			/*
			for(i = numParts; i < numParts + numNeurons; i++){
				clones[i] = GameObject.Instantiate(this.gameObject.transform);
				scales[i] = new Vector3(0.05f, 0.05f, 0.0f) * UnityEngine.Random.Range(1.0f, 3.0f);				
			}*/
			initialized = true;
		}
	}
}
