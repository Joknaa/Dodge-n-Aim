using UnityEngine;
using System.Collections;
using UnityEditor;

namespace LevelGeneration {
    [CustomEditor(typeof(LevelGenerator))]
    public class LevelGeneratorEditor : Editor {
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            LevelGenerator levelGenerator = (LevelGenerator)target;
            if (GUILayout.Button("Preview Level")) levelGenerator.RandomizeLevel(true);
            if (GUILayout.Button("Save Level")) levelGenerator.SaveLevel();
            if (GUILayout.Button("Clear")) levelGenerator.Reset();
        }
    }
}