using UnityEngine;
using System.Collections;

public class TrainingHarness : MonoBehaviour {

	void Update () {

        Vector3 localRot = transform.localRotation.eulerAngles;

        if (localRot.x > 90 && localRot.x < 180) localRot.x = 90;
        if (localRot.x < 270 && localRot.x > 180) localRot.x = 270;

        if (localRot.y > 90 && localRot.y < 180) localRot.y = 90;
        if (localRot.y < 270 && localRot.y > 180) localRot.y = 270;

        if (localRot.z > 90 && localRot.z < 180) localRot.z = 90;
        if (localRot.z < 270 && localRot.z > 180) localRot.z = 270;

        transform.localRotation = Quaternion.Euler(localRot);
	}
}
