using UnityEngine;
using System.Collections;

public class JoystickFlight : MonoBehaviour {

    public float nonlinearityPitch;
    public float nonlinearityRoll;
    public float nonlinearityYaw;
    public float nonlinearityClimb;
    
	void Start () {
	
	}
	
	void Update () {

        // Engines

        float roll = Input.GetAxis("Joy X");
        float ver = Input.GetAxis("Joy Y");

        float climb = Input.GetAxis("Vertical");
        float yaw = Input.GetAxis("Horizontal");

        roll = Mathf.Abs(Mathf.Pow(roll, nonlinearityRoll)) * (roll < 0 ? -1 : 1);
        ver = Mathf.Abs(Mathf.Pow(ver, nonlinearityPitch)) * (ver < 0 ? -1 : 1);
        yaw = Mathf.Abs(Mathf.Pow(yaw, nonlinearityYaw)) * (yaw < 0 ? -1 : 1);
        climb = Mathf.Abs(Mathf.Pow(climb, nonlinearityClimb)) * (climb < 0 ? -1 : 1);

        roll = (roll + 1) / 2;

        AHKController.SetEngineTilt(AHKController.EngineType.Left, yaw - ver);
        AHKController.SetEngineTilt(AHKController.EngineType.Right, -yaw - ver);

        AHKController.SetEnginePower(AHKController.EngineType.Left, roll + climb);
        AHKController.SetEnginePower(AHKController.EngineType.Right, 1-roll + climb);

        // Gyro

        float angle = AHKController.instance.transform.localRotation.eulerAngles.x;

        if (angle > 180) angle = -(360 - angle);

        Vector3 localangularvelocity = AHKController.instance.transform.InverseTransformDirection(AHKController.instance.GetComponent<Rigidbody>().angularVelocity).normalized 
            * AHKController.instance.GetComponent<Rigidbody>().angularVelocity.magnitude;

        AHKController.SetGyroPower(-angle / 15 - localangularvelocity.x);

    }
}
