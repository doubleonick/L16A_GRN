using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
 
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;

	GameObject wheelColliders;
    List<WheelCollider> wheels = new List<WheelCollider>();
    public float timeElapsed;
    public float timeCurrent;
    public int steerMode;
    public Light morningstar;
    public GameObject ir_cone;//Do this the same was as with bricks in block smasher...
    public List<GameObject> ir_sensors;

    public void Start(){
    	Rigidbody ir_sensor	   = new Rigidbody();
    	Quaternion ir_rotation = new Quaternion();
    	Vector3 ir_position    = new Vector3();
    	wheelColliders = GameObject.Find("WheelColliders");
    	morningstar = GameObject.Find("Directional Light").GetComponent<Light>();
    	wheels.Add(wheelColliders.transform.Find("frontRight").GetComponent<WheelCollider>());
		wheels.Add(wheelColliders.transform.Find("frontLeft").GetComponent<WheelCollider>());
		//wheels.Add(wheelColliders.transform.Find("backRight").GetComponent<WheelCollider>());
		//wheels.Add(wheelColliders.transform.Find("backLeft").GetComponent<WheelCollider>());
		//Wheels = wheelColliders.transform.Find("frontRight").GetComponent<WheelCollider>();
		foreach(WheelCollider wheel in wheels){
			
			if(wheel.name.Contains("front")){
				wheel.motorTorque = 50f;
				//wheel.steerAngle = 45f;
			}
		}
		timeElapsed  = 0;
		timeCurrent = 0;
		steerMode   = 0;

		ir_sensors = new List<GameObject>();
		for(int i = 0; i < 8; i++){
			ir_rotation = Quaternion.Euler(0, 45 * i, 90);
			ir_position = this.transform.position;
			print("ir_rotation " + ir_rotation.ToString());
			ir_sensor = Instantiate(ir_cone, ir_position, ir_rotation).GetComponent<Rigidbody>();
			ir_position = ir_sensor.transform.forward * 525;
			ir_sensor.MovePosition(ir_position);

			//ir_sensor.transform.position = ir_position;
		}
    	//print(wheelColliders.name + " contains " + frontRightCollider.name + "and that's nice, 'cause....");
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
 
        Transform visualWheel = collider.transform.GetChild(0);
 
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
 
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
        print(visualWheel.name + ": position " + visualWheel.transform.position.ToString() + " : rotation " + visualWheel.transform.rotation.ToString());
    }
 
    public void FixedUpdate()
    {
    	print("Bringer of light: " + morningstar.intensity.ToString());
    	timeCurrent = Time.timeSinceLevelLoad;
    	if(timeCurrent > timeElapsed){
    		if(steerMode == 0){
				foreach(WheelCollider wheel in wheels){
					
					if(wheel.name.Contains("Left")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}else if(wheel.name.Contains("Right")){
						wheel.motorTorque = 500f;
						wheel.steerAngle = 0f;
					}
				}
				steerMode = 1;
			}else if(steerMode == 1){
				foreach(WheelCollider wheel in wheels){
					if(wheel.name.Contains("Left")){
						wheel.motorTorque = -500f;
						wheel.steerAngle = 0f;
					}else if(wheel.name.Contains("Right")){
						wheel.motorTorque = -500f;
						wheel.steerAngle = 0f;
					}
				}
				steerMode = 0;
			}
			timeElapsed = timeCurrent + 30.0f;
		}
//        float motor = maxMotorTorque * Input.GetAxis("Vertical");
//        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
// 
//        foreach (AxleInfo axleInfo in axleInfos) {
//            if (axleInfo.steering) {
//                axleInfo.leftWheel.steerAngle = steering;
//                axleInfo.rightWheel.steerAngle = steering;
//            }
//            if (axleInfo.motor) {
//                axleInfo.leftWheel.motorTorque = motor;
//                axleInfo.rightWheel.motorTorque = motor;
//            }
//            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
//            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
//        }
    }
}