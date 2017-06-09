using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class PartCollision_PlusScale : MonoBehaviour {
	public string currPhase;
	public bool selected;
	public bool connected;
	
	public bool initialized;
	
	public string phaseOnePath = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\selectedParts.txt";
	public string[] phaseOneInfo;
	public int numParts;
	GameObject partPool;
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
		currPhase = Application.loadedLevelName;
		if(currPhase == "PhaseOne"){
			selected = false;
			connected = false;
		}
		initialized = false;
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		int n = 0;
		currPhase = Application.loadedLevelName;
		//print(currPhase);
		if(currPhase != "PhaseOne" && gameObject.name.Contains("Neuron") == true){
			gameObject.SetActive(true);
		}
		else if(currPhase == "PhaseTwo"){
			if(selected == false){ 
				gameObject.SetActive(false);
				
			}
			else if(selected == true){
				gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
				//gameObject.transform.position = new Vector3(1.0f, 0.0f, 0.0f) * UnityEngine.Random.Range(-3.0f, 3.0f);
				
				//print("Return " + gameObject.name + " to (0, 0, 0)..." + Environment.NewLine);
			}
			
			if(initialized == false){
				PhaseTwoInitialize();
			}
			else{
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
		//Use which scene you're in to determine what developmental behavior to enact.
		else if(currPhase == "PhaseEnd"){
			//print ("PhaseEnd Collision.........");
			//gameObject.SetActive(false);
		}
		
		
		
		
	}
	
	void OnTriggerEnter2D(Collider2D col){
		string colMsg;
		if(col.gameObject.name.Contains("Part") == true){
			colMsg = this.name + Environment.NewLine;//"I, " + this.name + " have collided with " + col.gameObject.name + "...!" + Environment.NewLine;
			//print(colMsg);
			string path = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\selectedParts.txt";
			
			// This text is added only once to the file.
			/*if (!File.Exists(path))
			{
				// Create a file to write to.
				string createText = "The following parts have been selected" + Environment.NewLine;
				File.WriteAllText(path, createText);
			}*/
			
			// This text is always added, making the file longer over time
			// if it is not deleted.
			string appendText = colMsg;//"This is extra text" + Environment.NewLine;
			File.AppendAllText(path, appendText);
			
			// Open the file to read from.
			//string readText = File.ReadAllText(path);
			//Console.WriteLine(readText);
			selected = true;
		}
		//This was here to try to record PhaseTwo collisions, which are meant to determine what gets connected to what.
		//However, there seem to be some issues.  Resolve these before moving onto PhaseThree, or the end phase.
		
		if(currPhase == "PhaseTwo"){
			string path = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectedParts.txt";
			//colMsg = this.name + ">>" + col.name + Environment.NewLine;//"I, " + this.name + " have collided with " + col.gameObject.name + "...!" + Environment.NewLine;
			//print(colMsg);
			
			if(connected == false){
				connected = true;
				string appendText = this.name + Environment.NewLine;//"This is extra text" + Environment.NewLine;
				File.AppendAllText(path, appendText);
			}	
			/*if(this.name.Contains("Neuron") && col.name.Contains("Neuron")){
				print("Neuron to Neuron connection: " + this.name + " --> " + col.name + Environment.NewLine);
			}*/
		}
	}
	
	void PhaseTwoInitialize() {
		//numParts = 0;
		float velocityX;
		float velocityY;
		float sensorIncRad = 0.3927f;
		float motorX;
		float motorY;
		Vector3 motorPositionDelta;
		if(File.Exists(phaseOnePath))
		{
			int i = 0;	
			int n = 0;
			
			//Get a representation of the PartPool, which contains all of the 
			//sensors and motors.  Find out how many there are.
			partPool = GameObject.Find("PartPool");
			numParts = partPool.transform.childCount;
			//Get a representation of the NeuronPool, which contains all of the 
			//intra neurons.  Find out how many there are.
			neuronPool = GameObject.Find("NeuronPool");
			numNeurons = neuronPool.transform.childCount;
			//Make sure the NeuronPool persists after loading the next scene.  It will be
			//needed to draw the final network/connectome/etc.
			DontDestroyOnLoad(neuronPool);
			neurons = new Transform[numNeurons];
			neuronPositions = new Vector3[numNeurons];
			neuronPosMagnitude = 3.8f;
			neuronIncRad = (Mathf.PI * 2.0f)/numNeurons;
			for(i = 0; i < numNeurons; i++){
				neurons[i] = neuronPool.transform.Find("Neuron"+i.ToString());
				neuronPositions[i].x = Mathf.Sin(neuronIncRad * i) * neuronPosMagnitude;
				neuronPositions[i].y = Mathf.Cos(neuronIncRad * i) * neuronPosMagnitude;
				neurons[i].position = neuronPositions[i];
				DontDestroyOnLoad(neurons[i].gameObject);
			}
			
			phaseOneInfo = File.ReadAllLines(phaseOnePath);
			
			scales = new Vector3[numParts + numNeurons];
			parts = new Transform[numParts];
			velocities = new Vector2[numParts];
			i = 0;
			foreach (string line in phaseOneInfo){
				velocityX = Mathf.Cos(sensorIncRad * -i);
				velocityY = Mathf.Sin(sensorIncRad * i);
				velocities[i] = new Vector2(velocityX, velocityY) * UnityEngine.Random.Range(0.0f, 1.0f);
				scales[i] = new Vector3(0.05f, 0.05f, 0.0f) * UnityEngine.Random.Range(1.0f, 2.0f);
				//print ("line in phaseOneInfo... " + line + Environment.NewLine);
				parts[i] = partPool.transform.Find(line);
				print("scale for " + parts[i].name + " " + scales[i] + Environment.NewLine);
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
			
			for(i = numParts; i < numParts + numNeurons; i++){
				n = i - numParts;
				//clones[i] = GameObject.Instantiate(this.gameObject.transform);
				scales[i] = new Vector3(0.05f, 0.05f, 0.0f) * UnityEngine.Random.Range(1.0f, 3.0f);		
				print("scale for " + neurons[n].name + " " + scales[i] + Environment.NewLine);		
			}
			
		}
		initialized = true;
		print ("PhaseTwo initialization complete!" + Environment.NewLine);
	}
}
