/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/


using UnityEngine;

using TMPro;
using System;

namespace IronManUI {

    [System.Serializable]
    
    public class TextBoxModel : IMComponentModel {
        public string text = "Sample text";

        override public void Copy(IMComponentModel o) {
            var o1 = o as TextBoxModel;
            if (o1 != null) {
                text = o1.text;
            }
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class TextBox : AbstractIMComponent {
        

        protected BoxCollider boxCollider;

        public TextMeshPro textMesh;

        public TextBoxModel textBoxModel;

        // public string text;

        override public IMComponentModel model {
            get {
                if (textBoxModel == null)
                    textBoxModel = new TextBoxModel();
                return textBoxModel;
            }
        }

        override protected void OnEnable() {
            base.OnEnable();

            boxCollider = GetComponent<BoxCollider>();

            textMesh.autoSizeTextContainer = true;
        }

        // override public void SetModel(IMComponentModel value) {
        //     base.SetModel(value);

        //     var model = value as TextBoxModel;
        //     if (model != null)
        //         text = model.text;
        // }

        // override public IMComponentModel ExtractModel() {
        //     TextBoxModel model = base.ExtractModel() as TextBoxModel;
        //     model.text = text;

        //     return model;
        // }


        override protected void Update() {
            textMesh.text = textBoxModel.text;

            var bounds = textMesh.bounds;
            boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, touchThickness);
            boxCollider.center = bounds.center;
            
            base.Update();
        }

        override protected IMComponentModel CreateDefaultModel()
        {
            return new TextBoxModel();
        }

        protected override Type GetModelType()
        {
            return typeof(TextBoxModel);
        }
    }
}