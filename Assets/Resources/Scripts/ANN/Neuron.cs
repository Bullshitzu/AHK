using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Neuron {

    public List<Synapse> InputSynapses;
    public float currentValue {
        get {
            return _currentValue;
        }
        set {
            _currentValue = Mathf.Clamp01(value);
        }
    }
    private float _currentValue;

    public Rect guiRect;

    public Neuron () {
        InputSynapses = new List<Synapse>();
    }

    public void RecalculateValue () {
        float sum = 0;
        float multSum = 0;

        for (int i = 0; i < InputSynapses.Count; i++) {
            sum += InputSynapses[i].GetValue();
            multSum += InputSynapses[i].multiplier;
        }

        if (InputSynapses.Count > 0) currentValue = sum / multSum;
        else currentValue = 0;
    }
}
