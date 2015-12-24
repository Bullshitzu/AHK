using UnityEngine;
using System.Collections;

public class AHKController : MonoBehaviour {

    public static AHKController instance;
    public Brain brain;

    public enum EngineType {
        Left,
        Right
    }

    [Header("GO Refs")]
	public GameObject engineLeftFan;
	public GameObject engineRightFan;

    public GameObject intakeFanLeft;
    public GameObject intakeFanRight;

    public GameObject flightTarget;

    [Header("Controls")]
    public Engine engineLeft;
    public Engine engineRight;

    public Gyro gyro;

    [Header("Properties")]
    public float engineFanSpeed;
    public float intakeFanSpeed;

    public bool enableTraining;
    
    void Start () {
        instance = this;
        GetComponent<Rigidbody>().centerOfMass = new Vector3(-0.01f, 2.5f, -2.7f);
        brain = new Brain();
    }

    void Update () {

        brain.Execute(enableTraining);

        engineLeftFan.transform.Rotate(0, engineFanSpeed * Time.deltaTime, 0, Space.Self);
        engineRightFan.transform.Rotate(0, engineFanSpeed * Time.deltaTime, 0, Space.Self);

        intakeFanLeft.transform.Rotate(0, 0, intakeFanSpeed * Time.deltaTime, Space.Self);
        intakeFanRight.transform.Rotate(0, 0, intakeFanSpeed * Time.deltaTime, Space.Self);

    }

    #region Public Interface

    public static void SetEnginePower (EngineType engine, float power) { // 0 to 1
        if (engine == EngineType.Left) {
            instance.engineLeft.SetPower(Mathf.Clamp01(power));
        }
        else if (engine == EngineType.Right) {
            instance.engineRight.SetPower(Mathf.Clamp01(power));
        }
    }
	
	public static void SetEngineTilt (EngineType engine, float angle) { // -1 to 1
		if(engine == EngineType.Left) {
            instance.engineLeft.SetTilt(Mathf.Clamp(angle, -1, 1));
		}
		else if (engine == EngineType.Right) {
			instance.engineRight.SetTilt(Mathf.Clamp(angle, -1, 1));
		}
	}

    public static void SetGyroPower (float power) { // -1 to 1
        instance.gyro.SetPower(Mathf.Clamp(power, -1, 1));
    }

    #endregion

}
