/**
 * Author:    Aaron Moffatt, Bob Levy
 * Created:   01.19.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;
using UnityEngine.UI;
using System;

namespace IronManUI {

    [System.Serializable]
    public class ImageBoxModel : IMComponentModel {
        public string text = "La_Fortuna_Waterfall_Pool.jpg";
    }

    [RequireComponent(typeof(BoxCollider))]
    // public class ImageBox : AbstractIMComponent {
    //     protected BoxCollider boxCollider;
    //     public Image image;
    //     public string text {
    //         get {
    //             return image.source
    //         }
    //         set {
    //             textMesh.text = value;
    //         }
    //     }

        override protected void OnEnable() {
            base.OnEnable();
            boxCollider = GetComponent<BoxCollider>();
        }

//        override protected void Update() {
//            textMesh.text = (model as TextBoxModel).text;
//            var bounds = textMesh.bounds;
//            boxCollider.size = new Vector3(bounds.size.x, bounds.size.y, touchThickness);
//            boxCollider.center = bounds.center;
//            base.Update();
//        }

//        override protected IMComponentModel CreateDefaultModel()
//        {
//            return new ImageBoxModel();
//        }

//        protected override Type GetModelType()
//        {
//            return typeof(ImageBoxModel);
//        }

        // public static Texture2D LoadPNG(string filePath) {
        //     Texture2D tex = null;
        //     byte[] fileData;
        //     if (File.Exists(filePath)) {
        //         fileData = File.ReadAllBytes(filePath);
        //         tex = new Texture2D(2, 2);
        //         tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        //     }
        //     return tex;
        // }
    }
}