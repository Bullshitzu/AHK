using UnityEngine;
using System.Collections;

public class AHKController : MonoBehaviour {

    static AHKController instance;

    public enum EngineType {
        Left,
        Right
    }

    #region References
    [Header("GO Refs")]
	public GameObject engineLeftFan;
	public GameObject engineRightFan;

    public GameObject intakeFanLeft;
    public GameObject intakeFanRight;

    // other references here...

    #endregion

    [Header("Controls")]
    public Engine engineLeft;
    public Engine engineRight;

    public Gyro gyro;

    [Header("Properties")]
    public float engineFanSpeed;
    public float intakeFanSpeed;

    
    void Start () {
        instance = this;
    }

    void Update () {

        engineLeftFan.transform.Rotate(0, engineFanSpeed * Time.deltaTime, 0, Space.Self);
        engineRightFan.transform.Rotate(0, engineFanSpeed * Time.deltaTime, 0, Space.Self);

        intakeFanLeft.transform.Rotate(0, 0, intakeFanSpeed * Time.deltaTime, Space.Self);
        intakeFanRight.transform.Rotate(0, 0, intakeFanSpeed * Time.deltaTime, Space.Self);


    }

    #region Public Interface

    public static void SetEnginePower (EngineType engine, float power) {
        if (engine == EngineType.Left) {
            instance.engineLeft.SetPower(power);
        }
        else if (engine == EngineType.Right) {
            instance.engineRight.SetPower(power);
        }
    }

    #endregion

}
