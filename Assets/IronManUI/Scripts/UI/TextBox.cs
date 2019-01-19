/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/
 

using UnityEngine;

using TMPro;


namespace IronManUI {

    [RequireComponent(typeof(BoxCollider))]
    public class TextBox : AbstractIMComponent {
        

        protected BoxCollider boxCollider;

        public TextMeshPro textMesh;

        public string text {
            get {
                return textMesh.text;
            }
            set {
                textMesh.text = value;
            }
        }


        override protected void OnEnable() {
            base.OnEnable();

            boxCollider = GetComponent<BoxCollider>();

            textMesh.autoSizeTextContainer = true;
        }


        override protected void Update() {
            var bounds = textMesh.bounds;
            boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, touchThickness);
            boxCollider.center = bounds.center;
            
            base.Update();
        }
    }
}