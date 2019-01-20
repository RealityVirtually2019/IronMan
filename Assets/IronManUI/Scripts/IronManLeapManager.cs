/**
 * Author:    Aaron Moffatt
 * Created:   01.19.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using System.Collections.Generic;

using UnityEngine;

using Leap.Unity;
using Leap.Unity.Interaction;

namespace IronManUI {

    public class FingertipInfo {
        public InteractionHand hand;
        // public PinchDetector pinch;
        public Fingertip thumb, index, middle;

        public FingertipInfo(InteractionHand hand, Fingertip thumb, Fingertip index, Fingertip middle) {
            this.hand = hand;
            // this.pinch = pinch;
            this.thumb = thumb;
            this.index = index;
            this.middle = middle;
        }
    }

    public class IronManLeapManager : MonoBehaviour {
        public GameObject leapRig;
        public float pinchDistanceThresh = .034f;

        public List<FingertipInfo> fingertipInfoList = new List<FingertipInfo>();

        void OnEnable() {
            if (leapRig == null) {
                Debug.LogWarning("LeapRig must be set");
                return;
            }
        }

        void RigHands() {
            foreach (var hand in leapRig.GetComponentsInChildren<InteractionHand>()) {
                if (hand.transform.childCount < 3)
                    continue;

                Debug.Log("Processing hand");

                foreach (var finger in hand.transform.GetChildren()) {
                    RigFingertip(finger.gameObject);
                }

                
                var thumb = hand.transform.GetChild(1).GetComponent<Fingertip>();
                if (thumb == null)
                    Debug.Log("Warning: thumb not added.");

                var indexFingertip = hand.transform.GetChild(2).GetComponent<Fingertip>();
                if (indexFingertip == null)
                    Debug.Log("Warning: index fingertip not added.");

                var flippingFingertip = hand.transform.GetChild(3).GetComponent<Fingertip>();
                if (flippingFingertip == null)
                    Debug.Log("Warning: middle fingertip not added.");

                // var pinchObj = new GameObject("PinchDetector-" + hand.name);
                // pinchObj.transform.parent = transform;
                // var pinchDetector = pinchObj.AddComponent<PinchDetector>();
                // pinchDetector.HandModel = hand;
                // hand.

                fingertipInfoList.Add(new FingertipInfo(hand, thumb, indexFingertip, flippingFingertip));
            }
        }

        void RigFingertip(GameObject finger) {
            finger.AddComponent<Fingertip>();
            

            var collider = finger.GetComponent<BoxCollider>();
            if (collider == null)
                collider = finger.AddComponent<BoxCollider>();
            collider.size = new Vector3(.1f,.1f,.1f);

            var rigidBody = finger.GetComponent<Rigidbody>();
            if (rigidBody == null)
                rigidBody = finger.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }

        void OnDisable() {
            fingertipInfoList.Clear();
            // if (fingertipInfoList.Count == 0)
            //     return;

            // foreach (var hand in leapRig.GetComponentsInChildren<InteractionHand>()) {
            //     foreach var fingertip in hand.GetComponentsInChildren<Fingertip
            // }
        }

        void Update() {
            if (fingertipInfoList.Count == 0) {
                RigHands();
            }

            foreach (var fingertipInfo in fingertipInfoList) {
                var indexPinchDistance = Vector3.Distance(fingertipInfo.thumb.transform.position, fingertipInfo.index.transform.position);
                var middlePinchDistance = Vector3.Distance(fingertipInfo.thumb.transform.position, fingertipInfo.middle.transform.position);
                bool indexGrabbing = indexPinchDistance < pinchDistanceThresh;
                bool middleGrabbing = middlePinchDistance < pinchDistanceThresh;
                if (indexGrabbing)
                    middleGrabbing = false;         //don't grab with both fingers
                // Debug.Log("Pinch distance: " + indexPinchDistance + " :: " + indexGrabbing);
                fingertipInfo.index.grabbing = indexGrabbing;
                fingertipInfo.middle.grabbing = middleGrabbing;
            }
        }

    }
}

