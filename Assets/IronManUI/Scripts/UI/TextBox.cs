using UnityEngine;

using TMPro;


namespace IronManUI {

    public class TextBox : AbstractIMComponent {

        public TextMeshPro textMesh;

        public string text {
            get {
                return textMesh.text;
            }
            set {
                textMesh.text = value;
            }
        }

        


    }
}