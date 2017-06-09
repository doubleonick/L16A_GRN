using UnityEngine;
using System.Collections;

public class PhaseOneManager : MonoBehaviour {

	float timeElapsed;
	float timePhaseOne;

	// Use this for initialization
	void Start () {
		timePhaseOne = 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		timeElapsed = Time.realtimeSinceStartup;
		//print("Time: " + timeElapsed);
		//increment = increment + 0.25f;
		if(timeElapsed >= timePhaseOne){
			print ("END PHASE ONE!!!");
			//System.IO.File.OpenWrite("selectedParts.txt");
			//System.IO.File.WriteAllText ("selectedParts.txt", "These are the parts.");
			
			Application.LoadLevel("PhaseTwo");
		}
	
	}
}
