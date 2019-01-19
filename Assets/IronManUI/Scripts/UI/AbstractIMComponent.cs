/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/
 

 using UnityEngine;


namespace IronManUI {

    public abstract class AbstractIMComponent : MonoBehaviour {
        protected float touchThickness = .25f;

        public readonly Spring3Motion translationMotion = new Spring3Motion();

        virtual protected void OnEnable() {
            translationMotion.position = transform.position;
        }
        
        virtual protected void Update() {

            translationMotion.Update(Time.deltaTime);
            transform.position = translationMotion.position;

        }


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