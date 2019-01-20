using UnityEngine;
using UnityEditor;

// namespace IronManUI {

//     [CustomEditor(typeof(TextBox))]
//     public class TextBoxEditor : Editor {

//         Vector2 scroll;

//         public override void OnInspectorGUI() {
//             DrawDefaultInspector();

//             TextBox target = this.target as TextBox;

//             GUILayout.Label("Text");
//             scroll = GUILayout.BeginScrollView(scroll);
//             target.text = GUILayout.TextArea(target.text);
//             GUILayout.EndScrollView();
//         }
//     }
// }