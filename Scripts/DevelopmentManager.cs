using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopmentManager : MonoBehaviour {

	private static int numSensors = 16;
	private static int numMotorLs = 1;
	private static int numMotorRs = 1;
	private static int numNeurons = 20;
	public GameObject neuron;
	public GameObject sensor;
	public GameObject motorLeft;
	public GameObject motorRight;
	private Vector3 cloneScale;
	//private Rigidbody2D[] cloneArmy;
	public List<Rigidbody2D> cloneArmy     = new List<Rigidbody2D>();
	public List<float> cloneVectorRate	   = new List<float>();
	public List<float> cloneVectorStart	   = new List<float>();
	public List<float> cloneVectorDuration = new List<float>();
	public List<Vector3> cloneGrowthRate   = new List<Vector3>();
	public List<float> cloneGrowthStart    = new List<float>();
	public List<float> cloneGrowthDuration = new List<float>();

	public string genomePath = "genome.txt";
	private string[] genome;
	private float timeElapsed;
	private float timeDevelop;

	// Use this for initialization
	void Start () {
		timeDevelop = 30.0f;
		prepDevelopment();
	}
	
	// Update is called once per frame
	void Update () {
		int i;

		timeElapsed = Time.timeSinceLevelLoad;
		//All of this seems to work great.  Now, need to get collisions right.
		for(i = 0; i < numSensors + numMotorLs + numMotorRs + numNeurons; i++){
			if(timeElapsed >= cloneVectorDuration[i]){
				cloneArmy[i].velocity = Vector3.zero;
			}
			//print("time: " + timeElapsed);
			//print("start growing: " + cloneGrowthStart[i]);
			//print("stop growing: " + (cloneGrowthStart[i] + cloneGrowthDuration[i]));
			if((timeElapsed >= (cloneGrowthStart[i] + cloneGrowthDuration[i]))){
				//print("stop growing....");
				cloneArmy[i].transform.localScale = cloneArmy[i].transform.localScale;
			}else if(timeElapsed >= cloneGrowthStart[i]){
				//print("growing.....");
				cloneArmy[i].transform.localScale += cloneGrowthRate[i];	
			}
			//This isn't right.  The key is to give development enough time to finish before
			//reloading ARoboGenesis.
			if(timeElapsed >= timeDevelop){
				UnityEngine.SceneManagement.SceneManager.LoadScene("ARoboGenesis");
			}
		}

	}

	void prepDevelopment(){
		int i, p;
		float vectorStart = 0.0f;
		float vectorAngle;
		float xVelocity;
		float yVelocity;
		float lineDuration;
		Vector2 cloneVelocity;
		//Instead of using partPool like this, use something akin to growthScales.txt and
		//hashScales.txt to determine velocity, growth rate, etc. for nuerons and parts.
		//Clone according to number of neurons found.
		Rigidbody2D clone;

		char[] genomeSeparator = new char[1];
		genomeSeparator[0] = ' ';
		p = 0;
		numSensors = 0;
		numMotorLs = 0;
		numMotorRs = 0;
		numNeurons = 0;
		//Parse genome into smallest pieces of data.  Each piece of data represents:
		//A part type
		//A fraction of 2* PI (direction of velocity)
		//Magnitude of velocity
		//Duration of ballistics
		//Growth start
		//Growth rate
		//Growth duration
		if(System.IO.File.Exists(genomePath)){
			genome = System.IO.File.ReadAllLines(genomePath);
			foreach(string gene in genome){
				vectorAngle = 0;
				xVelocity   = 0;
				yVelocity   = 0;
				string[] genePieces = gene.Split(genomeSeparator, 8);
				i = 0;
				foreach(string factor in genePieces){
					switch (i){
						//Don't start with type.  Start with spawn time.  Then, only move on to the next part of the
						//genome after spawn time has passed!
						case 0:
							//start
							//print("factor: " + factor);
							vectorStart = float.Parse(factor);
							timeElapsed = Time.timeSinceLevelLoad;
							i++;
							/*
							while(timeElapsed < vectorStart){
								timeElapsed = Time.timeSinceLevelLoad;
								print("case 0 timeElapsed: " + timeElapsed.ToString() + " vectorStart: " + vectorStart.ToString());
							}*/
							//print("Duration = " + factor);
							break;
						case 1:
							//print("number of clones = " + cloneArmy.Count.ToString() + " i = " + i.ToString());
							if(factor == "neuronSeed"){
								clone = Instantiate(neuron.transform, neuron.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
								clone.gameObject.name = "neuronSeed" + numNeurons.ToString(); 
								cloneArmy.Add(clone);
								numNeurons++;

							}else if(factor == "irSeed"){
								clone = Instantiate(sensor.transform, sensor.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
								clone.gameObject.name = "irSeed" + numSensors.ToString();
								cloneArmy.Add(clone);
								numSensors++;

							}else if(factor == "ldrSeed"){
								clone = Instantiate(sensor.transform, sensor.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
								clone.gameObject.name = "ldrSeed" + numSensors.ToString();
								cloneArmy.Add(clone);
								numSensors++;

							}else if(factor == "motorLeftSeed"){
								clone = Instantiate(motorLeft.transform, motorLeft.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
								clone.gameObject.name = "motorLeftSeed" + numMotorLs.ToString();
								cloneArmy.Add(clone);
								numMotorLs++;
							
							}else if(factor == "motorRightSeed"){
								clone = Instantiate(motorRight.transform, motorRight.transform.position, Quaternion.identity).GetComponent<Rigidbody2D>();
								clone.gameObject.name = "motorRightSeed" + numMotorRs.ToString();
								cloneArmy.Add(clone);
								numMotorRs++;

							}
							cloneVectorStart.Add(vectorStart);
							i++;
							break;
						case 2:
							vectorAngle = float.Parse(factor);
							xVelocity = Mathf.Cos(vectorAngle);
							yVelocity = Mathf.Sin(vectorAngle);
							//print("Angle = " + ((Mathf.PI * 2) * float.Parse(factor)));
							i++;
							break;
						case 3:
							//print("Rate = " + factor);
							//print("xVelocity = " + xVelocity);
							//print("yVelocity = " + yVelocity);

							xVelocity *= float.Parse(factor);
							yVelocity *= float.Parse(factor);
							i++;
							break;
						case 4:
							//duration
							//print("Growth Rate = " + factor);
							lineDuration = float.Parse(factor);
							cloneVectorDuration.Add(lineDuration);
							i++;
							break;
						case 5:
							//start
							//print("Growth Start = " + factor);
							cloneGrowthRate.Add(new Vector3(1.0f, 1.0f, 0.0f) * float.Parse(factor));
							i++;
							break;
						case 6:
							//rate
							//print("Growth Duration = " + factor);
							cloneGrowthStart.Add(float.Parse(factor));
							i++;
							break;
						case 7:
							//growthDruation
							cloneGrowthDuration.Add(float.Parse(factor));
							i++;
							break;
						default:
							print("default for gene factor...");
							i++;
							break;

					}
					//print(factor);
				}
				//print("cloneArmy length: " + cloneArmy.Count.ToString() + " index (p): " + p.ToString());
				cloneVelocity = new Vector2(xVelocity,yVelocity);
				cloneArmy[p].velocity = cloneVelocity;
				p++;
			}
		}
	}
}
