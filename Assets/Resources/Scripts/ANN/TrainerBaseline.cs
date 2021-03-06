using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainerBaseline : TrainerCorrelation {

    public TrainerBaseline (Brain brain) : base(brain) {
        baseList = new float[brain.InputCount];
        iterationCount = brain.MainColumn[0].InputSynapses.Count + 1;
    }

    public override void Mutate (Brain brain) {
        
        foreach (Neuron currNeuron in brain.MainColumn) {
            foreach (Synapse currSynapse in currNeuron.InputSynapses) {
                currSynapse.multiplier = 0.01f;
            }
        }
        foreach (Neuron currNeuron in brain.Outputs) {
            foreach (Synapse currSynapse in currNeuron.InputSynapses) {
                currSynapse.multiplier = 0.01f;
            }
        }
        
        switch (currentIteration) {
            case 1:
                return;
            default:
                int synapseIndex = (currentIteration - 2) % brain.InputCount;

                brain.SimulateInput(synapseIndex);
                //SetIterationConstraints(synapseIndex, brain);

                return;
        }
    }

    public static float[] baseList;

    public override void EndIteration (int num, Brain brain) {

        switch (num) {
            case 1:
                return;
            default:
                int neuronIndex = (num - 2) / brain.InputCount;
                int synapseIndex = (num - 2) % brain.InputCount;

                float currResult = (brain.Inputs[synapseIndex].currentValue);

                baseList[synapseIndex] = currResult;

                Debug.Log(neuronIndex + 1 + " - " + baseList.Length + " - " + synapseIndex + " - " + currResult);

                if (num == iterationCount) {
                    Debug.Log("Done! Applying..");

                    Reset(brain.Controller);
                }
                
                return;
        }
    }
}
