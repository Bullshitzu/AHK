using UnityEngine;
using System.Collections;

public class Engine : MonoBehaviour {

    public ParticleSystem flameEffect;
    public Light flameLight;
    public GameObject enginePivot;
    public Rigidbody mainRigidbody;

    public float thrust;

    private float targetPower = 1;
	public float currentPower = 1;

    private float targetTilt = 0;
    public float currentTilt = 0;

    const float tiltShiftSpeed = 2;
    const float powerShiftSpeed = 20;

	void Update () {
		
		currentPower = Mathf.Lerp(currentPower, targetPower, Time.deltaTime * powerShiftSpeed);
        currentTilt = Mathf.MoveTowards(currentTilt, targetTilt, Time.deltaTime * tiltShiftSpeed);
        float currentTiltSmoothed = Mathf.LerpAngle(enginePivot.transform.localRotation.eulerAngles.x, currentTilt * 90, 0.1f);

        enginePivot.transform.localRotation = Quaternion.Euler(currentTiltSmoothed, 0, 0);

        Vector3 thrustVector = transform.up * thrust * currentPower * Time.deltaTime;

        Debug.DrawRay(enginePivot.transform.position, thrustVector, Color.red);

        if (!float.IsNaN(thrustVector.x) && !float.IsNaN(thrustVector.y) && !float.IsNaN(thrustVector.z)) mainRigidbody.AddForceAtPosition(thrustVector, enginePivot.transform.position);
        flameEffect.startColor = new Color(1, 0.8f, 0.8f, currentPower * 0.5f);
        flameLight.intensity = currentPower * 3f;
	}

    public void SetPower (float target) {
        targetPower = target;
    }
    public void SetTilt (float target) {
        targetTilt = target;
    }
    public void Reset () {
        currentPower = 1;
        targetPower = 1;
        currentTilt = 0;
        targetTilt = 0;
        enginePivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
