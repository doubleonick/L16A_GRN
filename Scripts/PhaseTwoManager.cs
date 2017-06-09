using UnityEngine;
using System.Collections;

public class PhaseTwoManager : MonoBehaviour {
	
	float timeElapsed;
	float timePhaseTwo;
	
	
	
	// Use this for initialization
	void Start () {
		timePhaseTwo = 20.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed = Time.realtimeSinceStartup;
		//print("Time: " + timeElapsed);
		//increment = increment + 0.25f;
		if(timeElapsed >= timePhaseTwo){
			print ("END PHASE TWO!!!");
			//System.IO.File.OpenWrite("selectedParts.txt");
			//System.IO.File.WriteAllText ("selectedParts.txt", "These are the parts.");
			
			Application.LoadLevel("PhaseEnd");
		}
		
	}
}
