using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class ANNTrainer {

    // OVERRIDE STUFF
    public abstract float Score (AHKController controller);
    public abstract void Mutate (Brain brain);

    public abstract void StartIteration (int num, Brain brain);
    public abstract void EndIteration (int num, Brain brain);

    // UNIVERSAL

    protected float timer = 0;
    protected float timerMax = 1;
    protected float currScore = 0;

    protected int iterationCount = 1;
    protected int currentIteration = 1;

    public virtual bool Execute (Brain brain) { // returns true if done
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

    public void Reset (AHKController controller) {
        controller.transform.position = new Vector3(0, 0, 0);
        controller.transform.rotation = Quaternion.Euler(0, 0, 0);
        controller.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        controller.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        controller.engineLeft.Reset();
        controller.engineRight.Reset();
        controller.gyro.Reset();
    }
}
