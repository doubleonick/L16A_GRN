using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class PartCollision_Old : MonoBehaviour {
	public string currPhase;
	public bool selected;
	public bool connected;
	string pathGrowthScales = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\growthScales.txt";
	string pathHashtable = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\hashScales.txt";
	string pathConnectionStart = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectionStart.txt";
	string pathConnectionStop = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectionStop.txt";
	string pathConnectedParts = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectedParts.txt";
	string pathConnections = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connections.txt";
	string pathConnectionWeights = @"C:\Users\The Doctor\Documents\INSPIRE 2014-2017\L16A\GRN\connectionWeights.txt";
	string[] growthScales;
	string[] hashScales;

	// Use this for initialization
	void Start () {
		
		string[] hashInfo;
		currPhase = Application.loadedLevelName;
		if(currPhase == "PhaseOne"){
			selected = false;
			connected = false;
		}
		if(File.Exists(pathGrowthScales)){
			growthScales = File.ReadAllLines(pathGrowthScales);
		}
		if(File.Exists(pathHashtable)){
			hashScales = File.ReadAllLines(pathHashtable);
		}
	}
	
	// Update is called once per frame
	void Update () {
		currPhase = Application.loadedLevelName;
		//print(currPhase);
		if(currPhase != "PhaseOne" && gameObject.name.Contains("Neuron") == true){
			gameObject.SetActive(true);
		}
		else if(currPhase == "PhaseTwo" && selected == false){ 
			gameObject.SetActive(false);
			
		}
		else if(currPhase == "PhaseTwo" && selected == true){
			gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			//gameObject.transform.position = new Vector3(1.0f, 0.0f, 0.0f) * UnityEngine.Random.Range(-3.0f, 3.0f);
			
			//print("Return " + gameObject.name + " to (0, 0, 0)..." + Environment.NewLine);
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
			//colMsg = this.name + ">>" + col.name + Environment.NewLine;//"I, " + this.name + " have collided with " + col.gameObject.name + "...!" + Environment.NewLine;
			//print(colMsg);
			int whichScale = 0;
			float thisScale = 0.0f;
			float colScale = 0.0f;
			float connectionWeight = 0.0f;
			foreach(string scaleKey in hashScales){
				if(scaleKey.Contains(this.name)){
					thisScale = float.Parse(growthScales[whichScale]);
				}
				if(scaleKey.Contains(col.name)){
					colScale = float.Parse(growthScales[whichScale]);
				}
				whichScale++;
			}
			if(thisScale > colScale){
				print (this.name + " is connecting to " + col.name + " based on scales " + thisScale.ToString() + " & " + colScale.ToString() + Environment.NewLine);
				File.AppendAllText(pathConnectionStart, this.name + Environment.NewLine);
				File.AppendAllText(pathConnectionStop, col.name + Environment.NewLine);
				if(thisScale <= 1.5){
					connectionWeight = -1.0f;
				}
				else{
					connectionWeight = 1.0f;
				}
				File.AppendAllText(pathConnectionWeights, connectionWeight.ToString() + Environment.NewLine);
			}
			else if(colScale > thisScale){
				print (col.name + " is connecting to " + this.name + " based on scales " + colScale.ToString() + " & " + thisScale.ToString() + Environment.NewLine);
				File.AppendAllText(pathConnectionStart, col.name + Environment.NewLine);
				File.AppendAllText(pathConnectionStop, this.name + Environment.NewLine);	
				if(thisScale <= 1.5){
					connectionWeight = -1.0f;
				}
				else{
					connectionWeight = 1.0f;
				}
				File.AppendAllText(pathConnectionWeights, connectionWeight.ToString() + Environment.NewLine);	
			}
			else if(colScale == thisScale){
				print (this.name + " has a bidirectional conenction to " + col.name + " because scales = " + thisScale + " & " + colScale + Environment.NewLine);
				File.AppendAllText(pathConnectionStart, this.name + Environment.NewLine);
				File.AppendAllText(pathConnectionStop, col.name + Environment.NewLine);
				File.AppendAllText(pathConnectionStart, col.name + Environment.NewLine);
				File.AppendAllText(pathConnectionStop, this.name + Environment.NewLine);
				if(thisScale <= 1.5){
					connectionWeight = -1.0f;
				}
				else{
					connectionWeight = 1.0f;
				}
				File.AppendAllText(pathConnectionWeights, connectionWeight.ToString() + Environment.NewLine);
				File.AppendAllText(pathConnectionWeights, connectionWeight.ToString() + Environment.NewLine);
			}
			if(connected == false){
				connected = true;
				string appendText = this.name + Environment.NewLine;//"This is extra text" + Environment.NewLine;
				File.AppendAllText(pathConnectedParts, appendText);
			}	
			/*if(this.name.Contains("Neuron") && col.name.Contains("Neuron")){
				print("Neuron to Neuron connection: " + this.name + " --> " + col.name + Environment.NewLine);
			}*/
		}
	}
}
