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
        public string resource = "";
        public Vector3 scale = Vector3.one;
    }

    [RequireComponent(typeof(BoxCollider))]
    public class ThreeDItem : AbstractIMComponent {
        

        protected BoxCollider boxCollider;
        private string loadedResName;

        override protected void OnEnable() {
            base.OnEnable();

            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null)
                boxCollider = gameObject.AddComponent<BoxCollider>();
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
                        obj.transform.localScale = model.scale;
                    }
                }
                loadedResName = model.resource;
            }

            if (transform.childCount > 0) {
                transform.GetChild(0).transform.localScale = model.scale;
            }
            var bounds = gameObject.GetBounds();        //TODO needs help
            boxCollider.size = bounds.size;
            boxCollider.center = bounds.center;
            
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