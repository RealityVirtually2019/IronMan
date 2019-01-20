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
    
    public class ThreeDItemModel : IMComponentModel {
        public float resourceScale = 1f;
        public string resource = "";

        override public void Copy(IMComponentModel o) {
            var o1 = o as ThreeDItemModel;
            if (o1 != null) {
                resource = o1.resource;
                resourceScale = o1.resourceScale;
            }
        }
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class ThreeDItem : AbstractIMComponent {
        
        public ThreeDItemModel threeDModel;
        override public IMComponentModel model {
            get {
                if (threeDModel == null) 
                    threeDModel = new ThreeDItemModel();
                return threeDModel;
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
            ThreeDItemModel model = this.model as ThreeDItemModel;
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

                // var bounds = gameObject.GetBounds();        //TODO needs help
                // boxCollider.size = bounds.size;
                // boxCollider.center = bounds.center;
            }

            if (transform.childCount > 0) {
                transform.GetChild(0).transform.localScale = new Vector3(model.resourceScale, model.resourceScale, model.resourceScale);
            }
            
            base.Update();
        }

        override protected IMComponentModel CreateDefaultModel()
        {
            return new ThreeDItemModel();
        }

        protected override Type GetModelType()
        {
            return typeof(ThreeDItemModel);
        }
    }
}