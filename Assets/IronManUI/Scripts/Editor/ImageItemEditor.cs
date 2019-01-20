using UnityEngine;
using UnityEditor;

// namespace IronManUI {
//     [CustomEditor(typeof(ThreeDItem))]
//     public class ImageItemEditor : Editor {

//         Vector2 scroll;

//         public override void OnInspectorGUI() {
//             DrawDefaultInspector();

//             ThreeDItemModel model  = (this.target as ThreeDItem).model as ThreeDItemModel;

//             GUILayout.BeginHorizontal();
//             GUILayout.Label("Model Scaling:");
//             float scale = EditorGUILayout.FloatField(model.scale.x);
//             model.scale.Set(scale, scale, scale);
//             GUILayout.EndHorizontal();

//             GUILayout.Label("Resource Name:");
//             model.resource = EditorGUILayout.TextField(model.resource);
//         }
//     }
// }