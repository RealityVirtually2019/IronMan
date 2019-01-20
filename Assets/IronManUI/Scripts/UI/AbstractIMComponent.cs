/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/
 
using System;
using System.Collections.Generic;
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

        virtual public void Copy(IMComponentModel o) {
            targetPosition = o.targetPosition;
            hiddenPosition = o.hiddenPosition;

            targetRotation = o.targetRotation;
            hiddenRotation = o.hiddenRotation;

            targetScale = o.targetScale;
            hiddenScale = o.hiddenScale;
        }
    }


    public abstract class AbstractIMComponent : MonoBehaviour {
        protected float touchThickness = .02f;

        public readonly Spring3Motion translationMotion = new Spring3Motion();
        public readonly Spring3AngularMotion rotationMotion = new Spring3AngularMotion();
        public readonly Spring3Motion scalingMotion = new Spring3Motion();

        // public Vector3 targetPosition {
        //     get {
        //         return _internalModel.targetPosition;
        //     }
        //     set {
        //         _internalModel.targetPosition = value;
        //     }
        // }

        // public Vector3 targetRotation {
        //     get {
        //         return _internalModel.targetRotation;
        //     }
        //     set {
        //         _internalModel.targetRotation = value;
        //     }
        // }

        // public Vector3 targetScale {
        //     get {
        //         return _internalModel.targetScale;
        //     }
        //     set {
        //         _internalModel.targetScale = value;
        //     }
        // }

        abstract public IMComponentModel model { get; }


        public bool visible = true;

        public GrabHandler grab { get; private set; }
        private IMComponentMenuHandler menuHandler;

        // private IMComponentModel _internalModel;
        //     get {
        //         if (_model == null)
        //             _model = CreateDefaultModel();
        //         return _model;
        //     }
        //     set {       //setting a null value resets the model to default
        //         if (value != null && value.GetType() != GetModelType()) {
        //             Debug.LogWarningFormat("Error setting model of type {0} to {1}", value.GetType(), GetType());
        //             return;
        //         }

        //         _model = value;
        //     }
        // }

        // virtual public void SetModel(IMComponentModel value) {
        //     if (value != null && value.GetType() != GetModelType()) {
        //         Debug.LogWarningFormat("Error setting model of type {0} to {1}", value.GetType(), GetType());
        //         return;
        //     }

        //     if (value == null)
        //         _internalModel = CreateDefaultModel();
        //     else
        //         _internalModel = value;
        // }

        // virtual public IMComponentModel ExtractModel() {
        //     return _internalModel;
        // }


        virtual protected void OnEnable() {
            grab = new GrabHandler(this);
            menuHandler = new IMComponentMenuHandler(this);
            // model = CreateDefaultModel();
            // targetvisible ? transform.position;
            // rotationMotion.position = transform.rotation.eulerAngles;
            // scalingMotion.position = transform.localScale;
            
        }

        virtual protected void OnDisable() {
            grab = null;
            menuHandler = null;
        }

        
        virtual protected void Update() {

            grab.Update();
            menuHandler.Update();

            translationMotion.origin = visible ? model.targetPosition : model.hiddenPosition;
            translationMotion.Update(Time.deltaTime);
            transform.position = translationMotion.position;

            rotationMotion.origin = visible ? model.targetRotation : model.hiddenRotation;
            rotationMotion.Update(Time.deltaTime);
            transform.rotation = Quaternion.Euler(rotationMotion.position);

            scalingMotion.origin = visible ? model.targetScale : model.hiddenScale;
            scalingMotion.Update(Time.deltaTime);
            transform.localScale = scalingMotion.position.MinValue(0);
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

    public class GrabHandler {
        // private Dictionary<Fingertip, GrabInfo> grabs = new Dictionary<Fingertip, GrabInfo>();
        private List<Fingertip> grabs = new List<Fingertip>(2);
        private AbstractIMComponent parent;

        private Vector3 componentPositionAnchor;
        private Vector3 componentRotationAnchor;
        private Vector3 componentScaleAnchor;

        private Vector3? grabAvgAnchor;
        private float grabRadiusAnchor;
        private Vector3 grabRotationAnchor;

        // private Vector3? grabCenter;

        public GrabHandler(AbstractIMComponent parent) {
            this.parent = parent;
        }

        public void Begin(Fingertip finger, Vector3 grabPoint) {
            if (grabs.Count > 1)        //Max two grab points for now
                return;
            if (grabs.Contains(finger))
                return;
            grabs.Add(finger);//, new GrabInfo(grabPoint, parent.transform.position);
            grabAvgAnchor = null;
        }

        // public void End(Fingertip finger) {
        //     if (grabs.Remove(finger)) {
        //         grabAvgAnchor = null;
        //     }
        // }

        public void Update() {
            //check that all grabing fingers are still grabbing. Remove them if not
            List<Fingertip> removeKeys = new List<Fingertip>();
            foreach (var finger in grabs) {
                if (!finger.grabbing)
                    removeKeys.Add(finger);
            }
            if (removeKeys.Count > 0) {
                grabAvgAnchor = null;
            }

            foreach (var key in removeKeys) {
                grabs.Remove(key);
            }

            if (grabs.Count > 0) {
                if (grabAvgAnchor.HasValue) {
                    Vector3 grabAvg, grabRotation;
                    float grabRadius;
                    CalculateGrabStats(out grabAvg, out grabRadius, out grabRotation);

                    parent.model.targetPosition = componentPositionAnchor + (grabAvg - grabAvgAnchor.Value);
                    parent.model.targetRotation = componentRotationAnchor + (grabRotation - grabRotationAnchor);
                    
                    if (grabRadiusAnchor > .01 && grabRadius > .01) //safety checking
                        parent.model.targetScale = componentScaleAnchor * (grabRadius/ grabRadiusAnchor);

                }
                else {
                    Vector3 p;
                    CalculateGrabStats(out p, out grabRadiusAnchor, out grabRotationAnchor);
                    grabAvgAnchor = p;

                    var t = parent.transform;
                    componentPositionAnchor = t.position;
                    componentRotationAnchor = t.rotation.eulerAngles;
                    componentScaleAnchor = t.localScale;
                }
            }

        }

        void CalculateGrabStats(out Vector3 position, out float radius, out Vector3 rotation) {
            if (grabs.Count == 1) {
                var transform = grabs[0].transform;
                position = transform.position;
                rotation = Vector3.up;  //transform.rotation.eulerAngles;   //AM disable single hand rotation
                radius = 1;
            }
            else if (grabs.Count == 2) {
                var p0 = grabs[0].transform.position;
                var p1 = grabs[1].transform.position;
                position = (p1 + p0) * .5f;
                Vector3 delta = (p1 - p0);
                radius = delta.magnitude;
                rotation = Quaternion.LookRotation(delta, Vector3.up).eulerAngles;          //AM any way to optimize this?
                rotation.x = 0;     //AM: Don't support tilting the text. At least not until I can work out better math for this

            }
            else {
                position = Vector3.zero;
                radius = 0;
                rotation = Vector3.zero;
            }
        }

    // public class GrabInfo {
    //     public readonly Vector3 grabAnchor;
    //     public readonly Vector3 componentAnchor;

    //     public GrabInfo(Vector3 grabAnchor, Vector3 componentAnchor) {
    //         this.grabAnchor = grabAnchor;
    //         this.componentAnchor = componentAnchor;
    //     }

    // }
    }

}