using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class PartCollision : MonoBehaviour {
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
	public DevelopmentManager dm;
	string[] growthScales;
	string[] hashScales;


	void Awake(){
		dm = GameObject.FindObjectOfType<DevelopmentManager>();
	}

	// Use this for initialization
	void Start () {
		
		string[] hashInfo;
		currPhase = Application.loadedLevelName;
		if(currPhase == "PhaseOne"){
			selected = false;
			connected = false;
		}
		//If the Divinity Manager populates growthScales and hashScales (or
		//some equivalent), then that information can be used to determine
		//what connects to what.  In some way, it will be necessary to have
		//genetic information attached to seeds.
		//Possibly connect simple scripts to the brefabs.  The scripts would
		//simply have the genetic properties recorded.  Then you should be able
		//to just query the seed object.  Part Collision may be a perfectly
		//adequate place to do this since it gets attached to all seeds already.
		//The question would be how to assign values to Part Collision variables
		//from Divinity Manager.  That should just be a matter of having a
		//variable of type PartCollision within Divinity Manager.
		if(File.Exists(pathGrowthScales)){
			growthScales = File.ReadAllLines(pathGrowthScales);
		}
		if(File.Exists(pathHashtable)){
			hashScales = File.ReadAllLines(pathHashtable);
		}
	}
	
	// Update is called once per frame
	void Update () {
		

	}
	
	void OnTriggerEnter2D(Collider2D col){
		int cloneIndex;
		int thisIndex, colIndex;

		string colMsg;
		float timeElapsed;
		string thisName = this.name;
		string colName  = col.name;
		timeElapsed 	= Time.realtimeSinceStartup;
		//colMsg 			= thisName + " collided with " + colName + Environment.NewLine;
		//print(colMsg);
		//And now for the $64 thousand question, how should collisions be interpreted?
		//Deal with collisions with the shadow ring.
		if(thisName.Contains("Sensor") ){ 
			if(colName.Contains("irSeed")){
				//Change sensor color to indicate that this sensor may be connected to the ANN.
				if(col.GetComponent<SpriteRenderer>().color != Color.green){
					col.GetComponent<SpriteRenderer>().color = Color.green;
					cloneIndex = FindClone(colName);
					if(cloneIndex != - 1){
					    dm.cloneArmy[cloneIndex].name = "IR " + thisName;
					}
				}
				print(colName + " may develop into " + thisName);
			}else if(colName.Contains("ldrSeed")){
				//Change sensor color to indicate that this sensor may be connected to the ANN.
				if(col.GetComponent<SpriteRenderer>().color != Color.green){
					col.GetComponent<SpriteRenderer>().color = Color.green;
					cloneIndex = FindClone(colName);
					if(cloneIndex != -1){
					    dm.cloneArmy[cloneIndex].name = "LDR " + thisName;
					}
				}
				print(colName + " may develop into " + thisName);
			}
		}else if(colName.Contains("Sensor")){
			if(thisName.Contains("irSeed")){
				//Change sensor color to indicate that this sensor may be connected to the ANN.
				if(this.GetComponent<SpriteRenderer>().color != Color.green){
					this.GetComponent<SpriteRenderer>().color = Color.green;
					cloneIndex = FindClone(thisName);
					if(cloneIndex != -1){
						dm.cloneArmy[cloneIndex].name = "IR " + colName;
					}
				}
				print(thisName + " may develop into " + colName);
			}else if(thisName.Contains("ldrSeed")){
				//Change sensor color to indicate that this sensor may be connected to the ANN.
				if(this.GetComponent<SpriteRenderer>().color != Color.green){
					this.GetComponent<SpriteRenderer>().color = Color.green;
					cloneIndex = FindClone(thisName);
					if(cloneIndex != -1){
						dm.cloneArmy[cloneIndex].name = "LDR " + colName;
					}
				}
				print(thisName + " may develop into " + colName);
			}
		}
		//Deal with stem cells colliding with stem cells
		//Sensors can go directly to motors, or to neurons.
		//1. Cull sensor stem cells that do not collide with a specific shadow sensor.
		//2. Looking at which object INITIATES the connection, determine allowable connections.
		//3. If the connection is allowable, look at angle between parts to determine connection weight.
		//4. If parts get connected, record this to a file, or data structure.
		//Which (growth rate * growth duration) is bigger?  That component initiates connection, if
		//connection is to be made.
		thisIndex = FindClone(thisName);
		colIndex  = FindClone(colName);
		if(thisIndex == -1){
			print("Problem finding thisName: " + thisName);
		}
		if(colIndex == -1){
			print("Problem finding colName: " + colName);
		}
		if(thisIndex < dm.cloneArmy.Count && colIndex < dm.cloneArmy.Count){
			print("DetermineConnections for " + thisName + " at " + thisIndex.ToString() + " and " + colName + " at " + colIndex.ToString());
			DetermineConnection(thisIndex, colIndex);
		}
		/*
		if(timeElapsed >= 5.0f){
			colMsg = this.name + "Collided with " + col.name + Environment.NewLine;
			print(colMsg);
		}*/
	}

	int FindClone(string name){
		//print("Scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
		//print("Looking for " + name);
		int index = -1;
		if(dm != null){
			if(dm.cloneArmy == null){
				print("FindClone()... DM exists, null cloneArmy.");
				return index;
			}
		}
		else{
			print("FindClone().... null DevelopmentManager");
		}
		//Having null reference errors here still....(08 June 2017)
		int numClones = dm.cloneArmy.Count;
		//print("There are " + numClones.ToString() + " clones through which to search.");

		for(index = 0; index < numClones; index++){
			//print(".... " + dm.cloneArmy[index].name);
			if(dm.cloneArmy[index].name == name){
				//print("Looking for " + name + " and found " + dm.cloneArmy[index].name + " at index = "  + index.ToString());
				break;
			}
		}
		//print("Returning index " + index.ToString());
		return index;
	}

	void DetermineConnection(int thisIndex, int colIndex){
		float thisSize, colSize;
		thisSize  = dm.cloneGrowthRate[thisIndex][0] * dm.cloneGrowthDuration[thisIndex];
		colSize   = dm.cloneGrowthRate[colIndex][0] * dm.cloneGrowthDuration[colIndex];
		if(thisSize > colSize){
			if(dm.cloneArmy[thisIndex].name.Contains("Sensor") || (dm.cloneArmy[thisIndex].name.Contains("neuron") && !dm.cloneArmy[colIndex].name.Contains("Sensor"))){
				print(dm.cloneArmy[thisIndex] + " of size " + thisSize + " connecting(?) to " + dm.cloneArmy[colIndex] + " of size " + colSize);
			}
		}else if(colSize > thisSize){
			if(dm.cloneArmy[colIndex].name.Contains("Sensor") || (dm.cloneArmy[colIndex].name.Contains("neuron") && !dm.cloneArmy[thisIndex].name.Contains("Sensor"))){
				print(dm.cloneArmy[colIndex] + " of size " + colSize + " connecting(?) to " + dm.cloneArmy[thisIndex] + " of size " + thisSize);
			}
		}else{
			print("Bi-directional connection(?) between " + dm.cloneArmy[thisIndex] + " and " + dm.cloneArmy[colIndex]);
		}
	}
}
