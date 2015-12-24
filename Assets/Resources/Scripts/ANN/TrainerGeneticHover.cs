using UnityEngine;
using System.Collections;

public class TrainerGeneticHover : ANNTrainer {

    public TrainerGeneticHover (Brain brain) {
        iterationCount = 2;
        timerMax = 10;
        lastScore = float.MaxValue;
    }

    private float mutationChance = 5f; // in percent
    private float mutationAmount = 0.1f;

    public override float Score (AHKController controller) {
        float tempScore = Vector3.Distance(controller.transform.position, controller.flightTarget.transform.position);
        tempScore += (controller.flightTarget.transform.forward - controller.transform.forward).magnitude;
        return tempScore;
    }

    public override void Mutate (Brain brain) {

        for (int i = 0; i < brain.MainColumn.Count; i++) {
            for (int j = 0; j < brain.InputCount; j++) {
                float rand = Random.Range(0f, 100f);
                if (rand < mutationChance) {
                    float change = Random.Range(-mutationAmount, mutationAmount);
                    brain.MainColumn[i].InputSynapses[j].multiplier = Mathf.Clamp01(brain.MainColumn[i].InputSynapses[j].multiplier + change);
                }
            }
        }

        for (int i = 0; i < brain.Outputs.Count; i++) {
            for (int j = 0; j < brain.MainColumn.Count; j++) {
                float rand = Random.Range(0f, 100f);
                if (rand < mutationChance) {
                    float change = Random.Range(-mutationAmount, mutationAmount);
                    brain.Outputs[i].InputSynapses[j].multiplier = Mathf.Clamp01(brain.Outputs[i].InputSynapses[j].multiplier + change);
                }
            }
        }
    }

    public override void StartIteration (int num, Brain brain) {
        Reset(brain.Controller);
        Mutate(brain);
    }

    float[][][] savedNetwork;
    public void SaveNetwork (Brain brain) {
        savedNetwork = new float[2][][];
        savedNetwork[0] = new float[brain.MainColumn.Count][];
        for (int i = 0; i < brain.MainColumn.Count; i++) {
            savedNetwork[0][i] = new float[brain.InputCount];
            for (int j = 0; j < brain.InputCount; j++) {
                savedNetwork[0][i][j] = brain.MainColumn[i].InputSynapses[j].multiplier;
            }
        }
        savedNetwork[1] = new float[brain.OutputCount][];
        for (int i = 0; i < brain.Outputs.Count; i++) {
            savedNetwork[1][i] = new float[brain.MainColumn.Count];
            for (int j = 0; j < brain.MainColumn.Count; j++) {
                savedNetwork[1][i][j] = brain.Outputs[i].InputSynapses[j].multiplier;
            }
        }
    }
    public void LoadNetwork (Brain brain) {

        if (savedNetwork == null) return;
        if (savedNetwork[0] == null) return;

        for (int i = 0; i < brain.MainColumn.Count; i++) {
            for (int j = 0; j < brain.InputCount; j++) {
                brain.MainColumn[i].InputSynapses[j].multiplier = savedNetwork[0][i][j];
            }
        }
        for (int i = 0; i < brain.Outputs.Count; i++) {
            for (int j = 0; j < brain.MainColumn.Count; j++) {
                brain.Outputs[i].InputSynapses[j].multiplier = savedNetwork[1][i][j];
            }
        }
    }

    public override bool Execute (Brain brain) {
        if (currentIteration == 0) {
            currentIteration++;
            StartIteration(currentIteration, brain);
            SaveNetwork(brain);
        }

        currScore += Score(brain.Controller);

        timer += Time.deltaTime;
        if (timer >= timerMax) {

            EndIteration(currentIteration, brain);

            currentIteration++;
            if (currentIteration > iterationCount) return true;

            StartIteration(currentIteration, brain);

            if (currScore < lastScore) lastScore = currScore;
            currScore = 0;
            timer = 0;
        }

        return false;
    }

    public override void EndIteration (int num, Brain brain) {

        switch (num) {
            case 1:
                return;
            default:

                if (currScore > lastScore) LoadNetwork(brain);
                SaveNetwork(brain);

                iterationCount++;
                // todo: add an ending condition (based on score?)

                Debug.Log(num + " - " + (currScore < lastScore) + " - " + lastScore + " - " + currScore);

                return;
        }

    }
}
