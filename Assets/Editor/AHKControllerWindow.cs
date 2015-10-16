using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AHKStatus : EditorWindow {
    
    [MenuItem("Window/AHK")]
    static void Init () {
        AHKStatus window = (AHKStatus)EditorWindow.GetWindow(typeof(AHKStatus));
        window.Show();
        RecalculatePositions();
    }

    float timer = 0;
    void Update () {
        
        Repaint();

        timer += Time.deltaTime;
        if (timer > 1) {
            RecalculatePositions();
            timer = 0;
        }
    }

    Texture2D NodeIcon;
    string[] InputNames = { "Position", "Velocity", "Rotation", "Ang. Velocity" };
    string[] OutputNames = { "Engine 1 Thrust", "Engine 2 Thrust", "Engine 1 Tilt", "Engine 2 Tilt", "Gyroscope" };

    static void RecalculatePositions () {

        for (int i = 0; i < AHKController.instance.brain.InputCount; i++) {
            AHKController.instance.brain.Inputs[i].guiRect = new Rect(150, 30 * i + 50, 10, 10);
        }

        for (int i = 0; i < AHKController.instance.brain.OutputCount; i++) {
            AHKController.instance.brain.MainColumn[i].guiRect = new Rect(300, 30 * i + 80, 10, 10);
        }

        for (int i = 0; i < AHKController.instance.brain.OutputCount; i++) {
            AHKController.instance.brain.Outputs[i].guiRect = new Rect(450, 30 * i + 80, 10, 10);
        }

    }

    void OnGUI () {

        try {

            GUI.color = new Color(1, 1, 1, 1);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Aerial Hunter Killer", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            // ENGINES

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUILayout.Label("Engine 1");
            GUILayout.Label("Thrust: " + ((int)(AHKController.instance.engineLeft.currentPower * 10000) / 100f) + "%");
            GUILayout.Label("Tilt: " + ((int)(AHKController.instance.engineLeft.currentTilt * 900) / 10f) + " deg");

            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("Gyro");
            GUILayout.Label("Power: " + ((int)(AHKController.instance.gyro.currentPower * 10000) / 100f) + "%");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();

            GUILayout.Label("Engine 2");
            GUILayout.Label("Thrust: " + ((int)(AHKController.instance.engineRight.currentPower * 10000) / 100f) + "%");
            GUILayout.Label("Tilt: " + ((int)(AHKController.instance.engineRight.currentTilt * 900) / 10f) + " deg");

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // ANN

            if (NodeIcon == null) {
                NodeIcon = AssetDatabase.LoadAssetAtPath("Assets/Resources/Graphics/GUI/Node.psd", typeof(Texture2D)) as Texture2D;
                RecalculatePositions();
            }
            GUI.skin.box.normal.background = NodeIcon;

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Neural Network", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Inputs");
            GUILayout.Label("Neurons");
            GUILayout.Label("Outputs");
            GUILayout.EndHorizontal();

            GUI.skin.box.fixedWidth = 0;
            GUI.skin.box.fixedHeight = 0;
            GUI.skin.box.stretchWidth = true;
            GUI.skin.box.stretchHeight = true;
            Rect annRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.box);

            GUI.BeginGroup(annRect);

            // ANN HERE

            List<NeuronInput> Inputs = AHKController.instance.brain.Inputs;
            List<Neuron> Main = AHKController.instance.brain.MainColumn;
            List<Neuron> Outputs = AHKController.instance.brain.Outputs;

            // LABELS

            for (int i = 0; i < 4; i++) {
                GUI.Label(new Rect(20, 60 + i * 90, 110, 50), InputNames[i]);
            }

            for (int i = 0; i < 5; i++) {
                GUI.Label(new Rect(490, 70 + i * 60, 110, 50), OutputNames[i]);
            }

            // SYNAPSES

            for (int i = 0; i < Main.Count; i++) {
                for (int j = 0; j < Main[i].InputSynapses.Count; j++) {
                    Drawing.DrawLine(GetCenterOfRect(Main[i].guiRect), GetCenterOfRect(Main[i].InputSynapses[j].inputNeuron.guiRect), GetColorFromValue01(Main[i].InputSynapses[j].multiplier), 1, false);
                }
            }

            for (int i = 0; i < Outputs.Count; i++) {
                for (int j = 0; j < Outputs[i].InputSynapses.Count; j++) {
                    Drawing.DrawLine(GetCenterOfRect(Outputs[i].guiRect), GetCenterOfRect(Outputs[i].InputSynapses[j].inputNeuron.guiRect), GetColorFromValue01(Outputs[i].InputSynapses[j].multiplier), 1, false);
                }
            }

            // NEURONS

            for (int i = 0; i < Inputs.Count; i++) {
                GUI.color = GetColorFromValue01(Inputs[i].currentValue);
                GUI.Box(Inputs[i].guiRect, GUIContent.none);
            }

            for (int i = 0; i < Main.Count; i++) {
                GUI.color = GetColorFromValue01(Main[i].currentValue);
                GUI.Box(Main[i].guiRect, GUIContent.none);
            }

            for (int i = 0; i < Outputs.Count; i++) {
                GUI.color = GetColorFromValue01(Outputs[i].currentValue);
                GUI.Box(Outputs[i].guiRect, GUIContent.none);
            }

            GUI.EndGroup();

        }
        catch (System.Exception) { }
    }

    Color GetColorFromValue01 (float value) {
        if (float.IsNaN(value)) value = 0;
        return new Color(1-value, value, 0, 1);
    }
    Color GetColorFromValue11 (float value) {
        if (float.IsNaN(value)) value = 0;
        value = (value + 1) / 2f;
        return new Color(1 - value, value, 0, 1);
    }
    Vector2 GetCenterOfRect (Rect rect) {
        return new Vector2(rect.x + 5, rect.y + 5);
    }
}
