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
        public Fingertip fingertip;
        public Fingertip thumb;

        public FingertipInfo(InteractionHand hand, Fingertip thumb, Fingertip fingertip) {
            this.hand = hand;
            // this.pinch = pinch;
            this.thumb = thumb;
            this.fingertip = fingertip;
        }
    }

    public class IronManLeapManager : MonoBehaviour {
        public GameObject leapRig;
        public float pinchDistanceThresh = .04f;

        public List<FingertipInfo> fingertipInfoList = new List<FingertipInfo>();

        void OnEnable() {
            if (leapRig == null) {
                Debug.LogWarning("LeapRig must be set");
                return;
            }
        }

        void RigHands() {
            foreach (var hand in leapRig.GetComponentsInChildren<InteractionHand>()) {
                Debug.Log("Processing hand");
                foreach (var finger in hand.transform.GetChildren()) {
                    finger.gameObject.AddComponent<Fingertip>();
                }
                
                var thumb = hand.transform.GetChild(0).GetComponent<Fingertip>();
                if (thumb == null)
                    Debug.Log("Warning: thumb not added.");

                var indexFingertip = hand.transform.GetChild(1).GetComponent<Fingertip>();
                if (indexFingertip == null)
                    Debug.Log("Warning: index fingertip not added.");

                // var pinchObj = new GameObject("PinchDetector-" + hand.name);
                // pinchObj.transform.parent = transform;
                // var pinchDetector = pinchObj.AddComponent<PinchDetector>();
                // pinchDetector.HandModel = hand;
                // hand.

                fingertipInfoList.Add(new FingertipInfo(hand, thumb, indexFingertip));
            }
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
                var pinchDistance = Vector3.Distance(fingertipInfo.thumb.transform.position, fingertipInfo.fingertip.transform.position);
                fingertipInfo.fingertip.grabbing = pinchDistance < pinchDistanceThresh;
            }
        }

    }
}

