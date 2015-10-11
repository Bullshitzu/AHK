using UnityEngine;
using System.Collections;

public class Gyro : MonoBehaviour {

    public Rigidbody mainRigidbody;

    float targetPower;
    float currentPower;

    public float torque;

	void Update () {

        currentPower = Mathf.Lerp(currentPower, targetPower, 0.5f);

        mainRigidbody.AddRelativeTorque(new Vector3(currentPower * torque, 0, 0));

	}

    public void SetPower (float target) {
        targetPower = target;
    }
}
