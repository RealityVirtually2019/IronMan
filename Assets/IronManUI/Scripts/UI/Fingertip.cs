/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;

namespace IronManUI {

    public class GrabInfo {
        public readonly AbstractIMComponent component;
        public readonly Vector3 fingerAnchor;
        public readonly Vector3 componentAnchor;

        public GrabInfo(AbstractIMComponent component, Vector3 fingerAnchor, Vector3 componentAnchor) {
            this.component = component;
            this.fingerAnchor = fingerAnchor;
            this.componentAnchor = componentAnchor;
        }

    }

    [RequireComponent(typeof(Rigidbody))]
    public class Fingertip : MonoBehaviour {

        public Rigidbody rbody;

        private bool previouslyGrabbing;
        public bool grabbing;
        // public bool grabbing {
        //     get {
        //         return _grabbing;
        //     }
        //     set {
        //         if (_grabbing == value)
        //             return;
        //         _grabbing = value;
        //         if (_grabbing)
        //             BeginGrab();
        //         else
        //             EndGrab();
        //     }
        // }

        protected float passiveTouchForce = .5f;

        /** For velocity */
        private Vector3? previousPosition;
        protected Vector3 velocity;

        private AbstractIMComponent touchingComp;       //TODO AM update to handle touching multiple components

        private GrabInfo currentGrab;



        protected void OnEnable() {
            // collider.
            rbody = GetComponent<Rigidbody>();
            previousPosition = transform.position;
        }

        protected void Update() {
            UpdateVelocity();
            UpdateGrab();
            
        }

        protected void UpdateVelocity() {
            /** Calculate velocity (kinematic rigidbodies don't track this...)*/
            if (previousPosition.HasValue) {
                velocity = (transform.position - previousPosition.Value) / Time.deltaTime;
            } else {
                velocity = Vector3.zero;
            }
            previousPosition = transform.position;
        }

        protected void UpdateGrab() {
            if (grabbing != previouslyGrabbing) {
                if (grabbing)
                    BeginGrab();
                else
                    EndGrab();

                previouslyGrabbing = grabbing;
            }

            if (currentGrab == null)
                return;

            var moveDelta = transform.position - currentGrab.fingerAnchor;
            // currentGrab.component.translationMotion.origin = currentGrab.componentAnchor + moveDelta;
            currentGrab.component.model.targetPosition = currentGrab.componentAnchor + moveDelta;
        }


        void OnCollisionEnter(Collision collision) {
            Debug.Log("Fingertip Collision enter");
            var component = collision.GetIronManComponent();
            if (component != null) {
                touchingComp = component;
            }
        }

        void OnCollisionStay(Collision collision) {
            var component = collision.GetIronManComponent();
            if (component == touchingComp) {
                // var deltaVelocity = collision.relativeVelocity - component.velocity;        //AM relative velocity does not incorporate velocity of the non-rigidbody collider
                var deltaVelocity = velocity - component.translationMotion.velocity;
                // Debug.Log("Touched UI Moved: " + component);
                component.translationMotion.AddAcceleration(deltaVelocity * passiveTouchForce);
                // Debug.Log("delta v: " + deltaVelocity);

            }
        }

        void OnCollisionExit(Collision collision) {
            // Debug.Log("Fingertip Collision exit");
            var component = collision.GetIronManComponent();
            if (component == touchingComp) {
                touchingComp = null;
            }
            // if (currentGrab != null && component == currentGrab.component) {
            //     EndGrab();
            // }

        }

        public void BeginGrab() {
                Debug.Log("Beginning Grab: " + touchingComp);
            if (touchingComp != null) {
                currentGrab = new GrabInfo(touchingComp, transform.position, touchingComp.transform.position);
            }
        }


        public void EndGrab() {
            if (currentGrab != null) {
                currentGrab.component.translationMotion.origin = null;      //remove spring force
                currentGrab = null;
                Debug.Log("Ending Grab");
            }
        }




    }
}