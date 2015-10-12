using UnityEngine;
using System.Collections;

public class Synapse {

    public Neuron inputNeuron;
    public float multiplier;

    public Synapse (Neuron inputNeuron) {
        this.inputNeuron = inputNeuron;
        multiplier = 0.5f;
    }

    public float GetValue () {
        return inputNeuron.currentValue * multiplier;
    }



}
