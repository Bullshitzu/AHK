using UnityEngine;
using System.Collections;

public class Gyro : MonoBehaviour {

    public Rigidbody mainRigidbody;

    float targetPower;
    public float currentPower;

    public float torque;

	void Update () {

        currentPower = Mathf.Lerp(currentPower, targetPower, 0.5f);
        mainRigidbody.WakeUp();

        if(!float.IsNaN(currentPower * torque)) mainRigidbody.AddRelativeTorque(new Vector3(currentPower * torque, 0, 0));
	}
    public void SetPower (float target) {
        targetPower = target;
    }
    public void Reset () {
        targetPower = 0;
        currentPower = 0;
    }
}
