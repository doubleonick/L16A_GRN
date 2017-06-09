using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivinityManager : MonoBehaviour {
	//public GameObject novoPrime;
	private static bool novoPrime = false;
	private static int sizeGenOne  = 1;
	private string undevelopedStr  = "";
	private int undeveloped        = 0;
	private static int numGenes    = 8;
	private string pathUndeveloped = "undeveloped.txt";

	//Function to get random number
	private static readonly System.Random getrandom = new System.Random();
	private static readonly object syncLock = new object();
	// Use this for initialization
	void Start () {
		if(novoPrime == false){
			//print("You enter a small room...");
			System.IO.File.WriteAllText(pathUndeveloped, sizeGenOne.ToString());
			novoPrime = true;
		}
		else{
			//print("Move along...");
		}
		/*if(GameObject.Find("novoPrime") == null)
		{
			print("novoPrime missing...");
			Instantiate(novoPrime);
			DontDestroyOnLoad(novoPrime);
			System.IO.File.WriteAllText(pathUndeveloped, sizeGenOne.ToString());
		}*/
		undevelopedStr = System.IO.File.ReadAllText(pathUndeveloped);
		undeveloped = int.Parse(undevelopedStr);
		if(undeveloped > 0){
			numGenes = GetRandomNumber(4, 12);
			ARoboGenesis();
			undeveloped--;
			System.IO.File.WriteAllText(pathUndeveloped, undeveloped.ToString());
			//print("undeveloped: " + undeveloped.ToString());
			UnityEngine.SceneManagement.SceneManager.LoadScene("Development");
		}
		else{
			//print("And on the seventh day, He rested.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//ARoboGenesis();

	}

	void ARoboGenesis(){
		//Part Types: 0 = sensor, 1 = neuron, 2 = motorLeft, 3 = motorRight
		//Angle: 0:2PI
		//Vrate: 0.0:1.0 (by 0.1)
		//Vstart: 0.0:10 (seconds)
		//Vduration: 0.0:10.0 (seconds)
		//Erate: 0.0:1.0
		//Estart: 0.0:20 (seconds)
		//Eduration: 0.0:1.0 (seconds)
		int   type 	= 0;
		string typeStr = "";
		float angle = 0.0f;  	
		float vRate = 0.0f;
		float vStart = 0.0f;
		float vDuration = 0.0f;
		float eRate = 0.0f;
		float eStart = 0.0f;
		float eDuration = 0.0f;

		string pathGenOne = "genome.txt";
		string[] genomesGenOne = new string[numGenes];
		/*
		print("angle[] = " + angle);
		print("vRate[] = " + vRate);
		print("vStart[] = " + vStart);
		print("vDuration[] = " + vDuration);
		print("eRate[] = " + eRate);
		print("eStart[] = " + eStart);
		print("eDuration[] = " + eDuration);*/
		int i = 0;
		while(i < numGenes){//Not really the size of the first generation.  This is the number of parts.
			type      = GetRandomNumber(0, 4);
			angle  	  = UnityEngine.Random.value * (Mathf.PI * 2);
			vRate  	  = UnityEngine.Random.Range(0.0f, 0.5f);
			vStart    = UnityEngine.Random.Range(0.0f, 10.0f);
			vDuration = UnityEngine.Random.Range(0.0f, 10.0f);
			eRate     = UnityEngine.Random.value;
			eStart 	  = UnityEngine.Random.Range(0.0f, 20.0f);
			eDuration = UnityEngine.Random.Range(0.0f, 0.1f);

			switch(type){
				case 0:
					typeStr = "irSeed";
					break;
				case 1:
					typeStr = "ldrSeed";
					break;
				case 2:
					typeStr = "motorLeftSeed";
					break;
				case 3: 
					typeStr = "motorRightSeed";
					break;
				case 4:
					typeStr = "neuronSeed";
					break;
				default:
					print("invalid type: " + type.ToString());
					break;
			}
			/*print(type.ToString());
			print(angle.ToString());
			print(vRate.ToString());
			print(vStart.ToString());
			print(vDuration.ToString());
			print(eRate.ToString());
			print(eStart.ToString());
			print(eDuration.ToString());
			print("======== " + i.ToString() + " ========");*/
			genomesGenOne[i] = vStart.ToString() + " " + typeStr + " " + angle.ToString() + " " + vRate.ToString() + " " + vDuration.ToString() + " " + eRate.ToString() + " " + eStart.ToString() + " " + eDuration.ToString(); 
			i++;
		}
		System.IO.File.WriteAllLines(pathGenOne, genomesGenOne);
	}

	public static int GetRandomNumber(int min, int max)
	{
	    lock(syncLock) { // synchronize
	        return getrandom .Next(min, max + 1);
	    }
	}

}
