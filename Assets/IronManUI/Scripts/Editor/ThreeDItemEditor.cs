using UnityEngine;
using UnityEditor;

// namespace IronManUI {

//     [CustomEditor(typeof(ThreeDItem))]
//     public class ThreeDItemEditor : Editor {

//         Vector2 scroll;

//         public override void OnInspectorGUI() {
//             DrawDefaultInspector();

//             ThreeDItem model  = (this.target as ThreeDItem);

//             GUILayout.BeginHorizontal();
//             GUILayout.Label("Model Scaling:");
//             float scale = EditorGUILayout.FloatField(model.scale.x);
//             model.scale.Set(scale, scale, scale);
//             GUILayout.EndHorizontal();

//             GUILayout.Label("Resource Name:");
//             model.resourceName = EditorGUILayout.TextField(model.resourceName);
//         }
//     }
// }