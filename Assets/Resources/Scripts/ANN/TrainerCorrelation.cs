using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainerCorrelation : ANNTrainer {

    public override float Score (AHKController controller) { return 0; }

    public override void Mutate (Brain brain)  {

        for (int i = 0; i < brain.OutputCount; i++) {
            for (int j = 0; j < brain.InputCount; j++) {
                brain.MainColumn[i].InputSynapses[j].multiplier = 0.1f;
            }
        }

        switch (currentIteration) {
            case 1:
                return;
            default:
                int neuronIndex = (currentIteration - 2) / brain.InputCount;
                int synapseIndex = (currentIteration - 2) % brain.InputCount;

                brain.MainColumn[neuronIndex].InputSynapses[synapseIndex].multiplier = 1;
                brain.SimulateInput(synapseIndex);
                //SetIterationConstraints(synapseIndex, brain);

                return;
        }
    }

    public TrainerCorrelation (Brain brain) {
        iterationCount = brain.MainColumn.Count * brain.MainColumn[0].InputSynapses.Count + 1;
        scoreSets = new List<float[]>();
        timerMax = 1f;
    }

    public override bool Execute (Brain brain) {
        if (currentIteration == 0) {
            currentIteration++;
            StartIteration(currentIteration, brain);
        }

        currScore += Score(brain.Controller);

        timer += Time.deltaTime;

        if (timer >= timerMax) {

            EndIteration(currentIteration, brain);

            currentIteration++;
            if (currentIteration > iterationCount) return true;

            StartIteration(currentIteration, brain);

            currScore = 0;
            timer = 0;
        }

        return false;
    }

    public static List<float[]> scoreSets;

    public override void StartIteration (int num, Brain brain) {
        Reset(brain.Controller);
        Mutate(brain);
    }

    public void SetIterationConstraints (int num, Brain brain) {

        Rigidbody rigidbody = brain.AHK.GetComponent<Rigidbody>();

        switch (num) {
            case 0:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezeRotationZ;
                break;
            case 1:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
                break;
            case 2:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionZ;
                break;
            case 3:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionX & ~RigidbodyConstraints.FreezeRotationZ;
                break;
            case 4:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionY;
                break;
            case 5:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezePositionZ;
                break;
            case 6:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationX;
                break;
            case 7:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationY;
                break;
            case 8:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationZ;
                break;
            case 9:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationX;
                break;
            case 10:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationY;
                break;
            case 11:
                rigidbody.constraints = RigidbodyConstraints.FreezeAll & ~RigidbodyConstraints.FreezeRotationZ;
                break;
            default:
                rigidbody.constraints = RigidbodyConstraints.None;
                break;
        }
    }

    public override void EndIteration (int num, Brain brain) {

        float[] results;

        switch (num) {
            case 1:
                results = new float[brain.InputCount];
                for (int i = 0; i < brain.InputCount; i++) {
                    results[i] = brain.Inputs[i].currentValue;
                }
                scoreSets.Add(results);
                return;
            default:
                int neuronIndex = (num - 2) / brain.InputCount;
                int synapseIndex = (num - 2) % brain.InputCount;

                if (synapseIndex == 0) {
                    results = new float[brain.InputCount];
                    scoreSets.Add(results);
                }
                else results = scoreSets[neuronIndex];

                float currResult = (TrainerBaseline.baseList[synapseIndex] - brain.Inputs[synapseIndex].currentValue) * 2; // cca 2.5, perfect with an additional genetic pass
                results[synapseIndex] = currResult;

                Debug.Log(neuronIndex + 1 + "   -   " + synapseIndex + "   -   " + brain.Inputs[synapseIndex].currentValue + "  -   " + TrainerBaseline.baseList[synapseIndex] + "  -   " + currResult);

                if (num == iterationCount) {
                    Debug.Log("Done! Applying..");

                    Reset(brain.Controller);

                    for (int i = 0; i < brain.OutputCount; i++) {
                        for (int j = 0; j < brain.InputCount; j++) {
                            brain.MainColumn[i].InputSynapses[j].multiplier = Mathf.Clamp01(scoreSets[i][j]);
                        }
                    }
                }

                return;
        }
    }

    public override string ToString () {
        return "Correlation";
    }
}
