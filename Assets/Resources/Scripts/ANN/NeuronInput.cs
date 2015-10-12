using UnityEngine;
using System.Collections;

public class NeuronInput : Neuron {

    public NeuronInput () { }

    public void SetValue (float val) {
        currentValue = val;
    }

}
