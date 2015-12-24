using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Brain {

    public List<NeuronInput> Inputs;
    public List<Neuron> Outputs;

    public List<Neuron> MainColumn;

    public int InputCount=12;
    public int OutputCount=10;

    public GameObject AHK;
    public AHKController Controller;

    public List<ANNTrainer> TrainingSet;

    public Brain () {

        // INITIALIZATION

        Inputs = new List<NeuronInput>();
        Outputs = new List<Neuron>();
        MainColumn = new List<Neuron>();

        // REFERENCE GRABS

        AHK = AHKController.instance.gameObject;
        Controller = AHKController.instance;

        // INPUTS

        for (int i = 0; i < InputCount; i++) {
            Inputs.Add(new NeuronInput());
        }

        // MAIN COLUMN

        for (int i = 0; i < InputCount + 1; i++) {
            Neuron tempNeuron = new Neuron();
            for (int j = 0; j < InputCount; j++) {
                tempNeuron.InputSynapses.Add(new Synapse(Inputs[j]));
            }
            MainColumn.Add(tempNeuron);
        }

        // OUTPUTS

        for (int i = 0; i < OutputCount; i++) {
            Neuron tempNeuron = new Neuron();
            for (int j = 0; j < InputCount + 1; j++) {
                tempNeuron.InputSynapses.Add(new Synapse(MainColumn[j]));
            }
            Outputs.Add(tempNeuron);
        }

        // TRAINERS

        TrainingSet = new List<ANNTrainer>();
        TrainingSet.Add(new TrainerBaseline(this));
        TrainingSet.Add(new TrainerCorrelation(this));
        TrainingSet.Add(new TrainerGeneticHover(this));
    }

    public void Execute (bool withTraining) {

        UpdateInputs();

        if (withTraining) {
            if (TrainingSet.Count > 0) {
                if (TrainingSet[0].Execute(this)) {
                    TrainingSet.RemoveAt(0);
                }
            }
        }

        for (int i = 0; i < MainColumn.Count; i++) {
            MainColumn[i].RecalculateValue();
        }

        for (int i = 0; i < OutputCount; i++) {
            Outputs[i].RecalculateValue();
        }

        UpdateOutputs();

    }

    public void UpdateInputs () {

        // position relative to target point (local) x3
        // local translation speed x3
        // rotation relative to target point (local) x3
        // local rotation speed x3

        Vector3 relPos = AHK.transform.InverseTransformPoint(Controller.flightTarget.transform.position) / 10f; //todo: offset for CoM
        Inputs[0].currentValue = relPos.x + 0.5f;
        Inputs[1].currentValue = relPos.y + 0.5f;
        Inputs[2].currentValue = relPos.z + 0.5f;

        Vector3 vel = AHK.GetComponent<Rigidbody>().velocity / 10f;
        Inputs[3].currentValue = vel.x + 0.5f;
        Inputs[4].currentValue = vel.y + 0.5f;
        Inputs[5].currentValue = vel.z + 0.5f;

        Vector3 relRot = AHK.transform.localRotation.eulerAngles - Controller.flightTarget.transform.localRotation.eulerAngles;
        relRot = new Vector3(relRot.x > 180 ? -(360 - relRot.x) : relRot.x, relRot.y > 180 ? -(360 - relRot.y) : relRot.y, relRot.z > 180 ? -(360 - relRot.z) : relRot.z) / 30f;
        Inputs[6].currentValue = relRot.x + 0.5f;
        Inputs[7].currentValue = relRot.y + 0.5f;
        Inputs[8].currentValue = relRot.z + 0.5f;

        Vector3 angVel = AHK.GetComponent<Rigidbody>().angularVelocity / 2f;
        Inputs[9].currentValue = angVel.x + 0.5f;
        Inputs[10].currentValue = angVel.y + 0.5f;
        Inputs[11].currentValue = angVel.z + 0.5f;

    }

    public void UpdateOutputs () {

        // engine 1 thrust x2
        // engine 2 thrust x2

        // engine 1 tilt x2
        // engine 2 tilt x2

        // gyro x2

        AHKController.SetEnginePower(AHKController.EngineType.Left, (Outputs[0].currentValue - Outputs[1].currentValue + 1) / 2f);
        AHKController.SetEnginePower(AHKController.EngineType.Right, (Outputs[2].currentValue - Outputs[3].currentValue + 1) / 2f);

        AHKController.SetEngineTilt(AHKController.EngineType.Left, Outputs[4].currentValue - Outputs[5].currentValue);
        AHKController.SetEngineTilt(AHKController.EngineType.Right, Outputs[6].currentValue - Outputs[7].currentValue);

        AHKController.SetGyroPower(Outputs[8].currentValue - Outputs[9].currentValue);

    }

    public void SimulateInput (int input) {

        switch (input) {
            case 0:
                AHK.transform.position = new Vector3(-5f, 0, 0);
                break;
            case 1:
                AHK.transform.position = new Vector3(0, -5f, 0);
                break;
            case 2:
                AHK.transform.position = new Vector3(0, 0, -5f);
                break;

            case 3:
                AHK.GetComponent<Rigidbody>().velocity = new Vector3(5f, 0, 0);
                break;
            case 4:
                AHK.GetComponent<Rigidbody>().velocity = new Vector3(0, 5f, 0);
                break;
            case 5:
                AHK.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 5f);
                break;

            case 6:
                AHK.transform.localRotation = Quaternion.Euler(15f, 0, 0);
                break;
            case 7:
                AHK.transform.localRotation = Quaternion.Euler(0, 15f, 0);
                break;
            case 8:
                AHK.transform.localRotation = Quaternion.Euler(0, 0, 15f);
                break;

            case 9:
                AHK.GetComponent<Rigidbody>().angularVelocity = new Vector3(1f, 0, 0);
                break;
            case 10:
                AHK.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 1f, 0);
                break;
            case 11:
                AHK.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 1f);
                break;
        }


    }
}
