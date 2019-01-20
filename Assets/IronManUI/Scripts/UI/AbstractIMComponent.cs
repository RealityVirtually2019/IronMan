/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/
 
using System;
using UnityEngine;


namespace IronManUI {

    [System.Serializable]
    public class IMComponentModel {
        public Vector3 targetPosition;
        public Vector3 hiddenPosition;

        public Vector3 targetRotation;
        public Vector3 hiddenRotation;

        public Vector3 targetScale = Vector3.one;
        public Vector3 hiddenScale;
    }


    public abstract class AbstractIMComponent : MonoBehaviour {
        protected float touchThickness = .02f;

        public readonly Spring3Motion translationMotion = new Spring3Motion();
        public readonly Spring3Motion rotationMotion = new Spring3Motion();
        public readonly Spring3Motion scalingMotion = new Spring3Motion();


        public bool visible = true;

        private IMComponentModel _model;
        public IMComponentModel model {
            get {
                if (_model == null)
                    _model = CreateDefaultModel();
                return _model;
            }
            set {       //setting a null value resets the model to default
                if (value != null && value.GetType() != GetModelType()) {
                    Debug.LogWarningFormat("Error setting model of type {0} to {1}", value.GetType(), GetType());
                    return;
                }

                _model = value;
            }
        }


        virtual protected void OnEnable() {
            // targetvisible ? transform.position;
            // rotationMotion.position = transform.rotation.eulerAngles;
            // scalingMotion.position = transform.localScale;
            
        }
        
        virtual protected void Update() {

            translationMotion.origin = visible ? model.targetPosition : model.hiddenPosition;
            translationMotion.Update(Time.deltaTime);
            transform.position = translationMotion.position;

            rotationMotion.origin = visible ? model.targetRotation : model.hiddenRotation;
            rotationMotion.Update(Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationMotion.position);

            scalingMotion.origin = visible ? model.targetScale : model.hiddenScale;
            scalingMotion.Update(Time.deltaTime);
            transform.localScale = scalingMotion.position;
        }

        abstract protected Type GetModelType();
        abstract protected IMComponentModel CreateDefaultModel();

        
        // void OnTriggerEnter(Collider collider) {
        //     Debug.Log("Component Collision enter");
        //     var fingertip = collider.gameObject.GetComponent<Fingertip>();
        //     if (fingertip != null) {
        //         Debug.Log("Touched by finger: " + fingertip);
        //     }
        // }

        // void OnTriggerExit(Collider collider) {
        //     Debug.Log("Component Collision exit");

        // }

        

    }


}