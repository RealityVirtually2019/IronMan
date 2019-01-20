/**
 * Author:    Aaron Moffatt, Bob Levy
 * Created:   01.19.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;
using UnityEngine.UI;
// using System.Collections;
using System.IO;
using System;

namespace IronManUI {

    [System.Serializable]
    
    public class ImageModel : IMComponentModel {
        public float resourceScale = 1f;
        public string resource = "";

        override public void Copy(IMComponentModel o) {
            var o1 = o as ImageModel;
            if (o1 != null) {
                resource = o1.resource;
                resourceScale = o1.resourceScale;
            }
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class ImageItem : AbstractIMComponent {
        public ImageModel imageModel;
        override public IMComponentModel model {
            get {
                if (imageModel == null) 
                    imageModel = new ImageModel();
                return imageModel;
            }
        }

        protected BoxCollider boxCollider;
        private string loadedResName;

        override protected void OnEnable() {
            base.OnEnable();

            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
                boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        override protected void OnDisable() {
            base.OnDisable();
            gameObject.DestroyChildren();
            loadedResName = null;
        }

        override protected void Update() {
            ImageModel model = this.model as ImageModel;
            if (model.resource != loadedResName) {
                gameObject.DestroyChildren();

                var resource = Resources.Load(model.resource);
                if (resource != null) {
                    var obj = Instantiate(resource) as GameObject;
                    if (obj != null) {
                        obj.transform.parent = transform;
                        obj.transform.localScale = new Vector3(model.resourceScale, model.resourceScale, model.resourceScale);
                    }
                }
                loadedResName = model.resource;
            }

                // var bounds = gameObject.GetBounds();        //TODO needs help
                // boxCollider.size = bounds.size;
                // boxCollider.center = bounds.center;

            if (transform.childCount > 0) {
                transform.GetChild(0).transform.localScale = new Vector3(model.resourceScale, model.resourceScale, model.resourceScale);
            }

            base.Update();
        }

        override protected IMComponentModel CreateDefaultModel()
        {
            return new ImageModel();
        }

        protected override Type GetModelType()
        {
            return typeof(ImageModel);
        }
    }

}


//
// research on loading 2D images into scene
//

// ref: http://gyanendushekhar.com/2017/07/08/load-image-runtime-unity/

    // public class LoadTexture : MonoBehaviour {
    //     Texture2D myTexture;

    //     // Use this for initialization
    //     void Start () {
    //         // load texture from resource folder
    //         myTexture = Resources.Load ("Images/Factories.jpg") as Texture2D;

    //         GameObject rawImage = GameObject.Find ("RawImage");
    //         rawImage.GetComponent<RawImage> ().texture = myTexture;
    //     }
    // }

// ref: https://forum.unity.com/threads/generating-sprites-dynamically-from-png-or-jpeg-files-in-c.343735/

    // public class IMG2Sprite : MonoBehaviour {
    // 
    // // This script loads a PNG or JPEG image from disk and returns it as a Sprite
    // // Drop it on any GameObject/Camera in your scene (singleton implementation)
    // //
    // // Usage from any other script:
    // // MySprite = IMG2Sprite.instance.LoadNewSprite(FilePath, [PixelsPerUnit (optional)])
    // private static IMG2Sprite _instance;

    // public static IMG2Sprite instance
    // {
    //     get
    //     {
    //     //If _instance hasn't been set yet, we grab it from the scene!
    //     //This will only happen the first time this reference is used.
    //     if(_instance == null)
    //         _instance = GameObject.FindObjectOfType<IMG2Sprite>();
    //     return _instance;
    //     }
    // }
    // public Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f) {

    //     // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

    //     Sprite NewSprite = new Sprite();
    //     Texture2D SpriteTexture = LoadTexture(FilePath);
    //     NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),new Vector2(0,0), PixelsPerUnit);

    //     return NewSprite;
    // }

    // public Texture2D LoadTexture(string FilePath) {

    //     // Load a PNG or JPG file from disk to a Texture2D


    //     Texture2D Tex2D;
    //     byte[] FileData;

    //     if (File.Exists(FilePath)){
    //     FileData = File.ReadAllBytes(FilePath);
    //     Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
    //     if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
    //         return Tex2D;                 // If data = readable -> return texture
    //     }
    //     return null;                     // Return null if load failed
    // }




    // public class ImageBoxModel : IMComponentModel {
    //     public string text = "La_Fortuna_Waterfall_Pool.jpg";
    // }

    // [RequireComponent(typeof(BoxCollider))]
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

        // override protected void OnEnable() {
        //     base.OnEnable();
        //     boxCollider = GetComponent<BoxCollider>();
        // }

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

