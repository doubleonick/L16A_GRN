using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class PhaseEndManager : MonoBehaviour {

	private string pathConnectedParts = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectedParts.txt";
    string[] connectedNodesList;
	
	public string currPhase;
	public int numParts;
	public GameObject partPool; 
	Transform[] parts;
	string[] connectedPartsList;
	Vector2[] velocities;	
	
	int numNeurons;
	public GameObject neuronPool;
	//Transform[] neurons;
	string[] connectedNeuronsList;
	float neuronPosMagnitude;
	float neuronX;
	float neuronY;
	//Vector3[] neuronPositions;
	float neuronIncRad;
	
	Transform[] inputs;
	Transform[] neurons;
	Transform[] outputs;
	Vector3 originalScale;
	int numInputs;
	int numOutputs;
	int numConnections;
	Vector3[] inputPositions;
	Vector3[] neuronPositions;
	Vector3[] outputPositions;
	float[] connectionAngles;
	Vector3[] connectionAnchors;
	float inputXRange;
	float neuronRadius;
	float outputXRange;
	
	public Transform connectionPrefab;
	
	// Use this for initialization
	void Start () {
		print("PHASE THREE: Registering parts and neurons...." + Environment.NewLine);
		currPhase = Application.loadedLevel.ToString();
		inputXRange = 5.0f;
		neuronRadius = 2.0f;
		outputXRange = 2.0f;
		originalScale = new Vector3(0.25f, 0.25f, 1.0f);
		connectionPrefab = GameObject.Find("Connection").transform;
		Register();
		//List ();
		//Bin ();
		Assemble();
		Connect();
	}
	
	// Update is called once per frame
	void Update () {
		//int i, o, n;
		//Assemble();
		/*for(i = 0; i < numInputs; i++){
			inputs[i].localScale = originalScale;
		}
		for(o = 0; o < numOutputs; o++){
			outputs[o].localScale = originalScale;
		}
		for(n = 0; n < numNeurons; n++){
			neurons[n].localScale = originalScale;
		}*/
	}
	
	void Register(){
		int i;
		int p = 0;
		int n = 0;
		numParts = 0;
		numNeurons = 0;
		numInputs = 0;
		numOutputs = 0;
		numConnections = 0;
		
		
		if(File.Exists(pathConnectedParts)){
			connectedNodesList = File.ReadAllLines(pathConnectedParts);
		
			partPool = GameObject.Find("PartPool");
			print ("Found a " + partPool.name + Environment.NewLine);
			foreach(string line in connectedNodesList){
				if(line.Contains("Neuron") == false){
					numParts++;
				}
				else{
					numNeurons++;
				}
			}
			connectedPartsList = new string[numParts];
			connectedNeuronsList = new string[numNeurons];
			foreach(string line in connectedNodesList){
				if(line.Contains("Neuron") == false){
					connectedPartsList[p] = line;
					p++;
					print (line + Environment.NewLine);
				}
				else{
					connectedNeuronsList[n] = line;
					n++;
				}
				numConnections++;
			}
		}
		print ("There are " + numParts.ToString() + " parts present." + Environment.NewLine);		
		neuronPool = GameObject.Find("NeuronPool");
		print ("Found a " + neuronPool.name + Environment.NewLine);
		print ("There are " + numNeurons.ToString() + " neurons present." + Environment.NewLine);

		parts = new Transform[numParts];
		neurons = new Transform[numNeurons];

//		neuronPositions = new Vector3[numNeurons];
//		neuronPosMagnitude = 3.8f;
//		neuronIncRad = (Mathf.PI * 2.0f)/numNeurons;
//		for(i = 0; i < numNeurons; i++){
//			neurons[i] = neuronPool.transform.FindChild("Neuron" + i.ToString());
//			neuronPositions[i].x = Mathf.Sin(neuronIncRad * i) * neuronPosMagnitude;
//			neuronPositions[i].y = Mathf.Cos(neuronIncRad * i) * neuronPosMagnitude;
//			neurons[i].position = neuronPositions[i];
//		}
	}
	
	void Assemble(){
		int i, n, o;
		float neuronIncRad;
		i = 0;
		n = 0;
		o = 0;
		//Array Sensors in a line
		//Array Neurons in a circle
		//Array Motors in a line
		print ("Assemble()" + Environment.NewLine);
		foreach(string connPart in connectedPartsList){
			if(connPart.Contains("IR") || connPart.Contains("LDR")){
				numInputs++;
			}
			else{
				numOutputs++;
			}
		}
		inputPositions = new Vector3[numInputs];
		neuronPositions = new Vector3[numNeurons];
		outputPositions = new Vector3[numOutputs];
		connectionAngles = new float[numConnections];
		
		inputs = new Transform[numInputs];
		neurons = new Transform[numNeurons];
		outputs = new Transform[numOutputs];
		print ("Inputs and Outputs have been counted and initialized." + Environment.NewLine);
		print ("From the connectedPartsList, there are " + numInputs.ToString() + " inputs and " + numOutputs.ToString() + " outputs" + Environment.NewLine);
		foreach(string connPart in connectedPartsList){
			print ("Dealing with connected part: " + connPart + Environment.NewLine);
			if(connPart.Contains("IR") || connPart.Contains("LDR")){
				inputs[i] = GameObject.Find(connPart).transform;
				if(i < numInputs/2){
					inputPositions[i].x = inputXRange * (-i/(float)numInputs * 3);
				}
				else{
					inputPositions[i].x = inputXRange * ((i - numInputs/2 + 1)/(float)numInputs * 3);
				}
				inputPositions[i].y = 4.5f;
				print ("Position for " + inputs[i].name + " (" + inputPositions[i].x + ", " + inputPositions[i].y + ")" +  Environment.NewLine);
				inputs[i].position = inputPositions[i];
				inputs[i].localScale = originalScale;
				i++;
			}
			else{
				outputs[o] = GameObject.Find(connPart).transform;
				outputPositions[o].x = outputXRange * (o/(float)numOutputs);
				outputPositions[o].y = -4.5f;
				print ("Position for " + outputs[o].name + " (" + outputPositions[o].x + ", " + outputPositions[o].y + ")" + Environment.NewLine);
				outputs[o].position = outputPositions[o];
				outputs[o].localScale = originalScale;
				o++;
			}
		}
		print ("There are " + numNeurons + " neurons to assemble." + Environment.NewLine);
		neuronIncRad = (Mathf.PI * 2.0f)/(float)numNeurons;
		foreach(string connNeuron in connectedNeuronsList){
			print ("Dealing with " + connNeuron + Environment.NewLine);
			neurons[n] = GameObject.Find(connNeuron).transform;
			neuronPositions[n].x = Mathf.Sin(neuronIncRad*n) * neuronRadius;
			neuronPositions[n].y = Mathf.Cos(neuronIncRad*n) * neuronRadius;
			print ("Position for " + neurons[n].name + " (" + neuronPositions[n].x + ", " + neuronPositions[n].y + ")" + Environment.NewLine);
			neurons[n].position = neuronPositions[n];
			neurons[n].localScale = originalScale;
			n++;
		}
		//Draw everything according to percent of the allotted space per part type.
		//Record x and y coordinates of each node/component as it is placed (Vector2)
	}
	
	void Connect(){
		//There will be a file that holds, not just the nodes that
		//are to be connected, but the connections themselves.
		//Use the lines of this file to create a number of GameObjects that are physical connections.
		//Use x and y coordinates of nodes being connected to a) scale the connection, and b) rotate
		//it, so that it has the slope needed to connect the two points, and c) translate it so that
		//its center sits at the center of the line between the two points.  Or, if the coordinate
		//system is made to sit at the end of the thing, place that end at the node from which the
		//connection originates.
		int i;
		numConnections = (numInputs + numOutputs + numNeurons);
		float connIncRad = (Mathf.PI * 2)/numConnections;
		float radToDeg = 57.2958f;
		float connX;
		float connY;
		Vector3 connLength;
		Vector3 connPos;
		Transform[] connections = new Transform[numConnections];
		//UnityEngine.Rigidbody2D[] connections = new UnityEngine.Rigidbody2D[numConnections];
		for(i = 0; i < numConnections; i++){
			connX = Mathf.Sin(connIncRad * i);
			connY = Mathf.Cos(connIncRad * i);
			connPos = new Vector3(connX, connY, 0);
			connections[i] = (Transform) Instantiate(connectionPrefab, connPos, Quaternion.AngleAxis(connIncRad * i * radToDeg, new Vector3(0, 0, 1)));
			connLength = new Vector3(10f, 1f, 1f);
			//connections[i] = connection.transform;
			connections[i].localScale = connLength; 
		}
		connectionPrefab.gameObject.SetActive(false);
	}
}
