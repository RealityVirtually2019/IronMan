using UnityEngine;
using UnityEditor;

namespace IronManUI {

    [CustomEditor(typeof(PresentationManager))]
    public class PresentationManagerEditor : Editor {

        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            PresentationManager target = this.target as PresentationManager;

            if (GUILayout.Button("Save Presentation")) {
                target.Save();
            }
        }
    }
}