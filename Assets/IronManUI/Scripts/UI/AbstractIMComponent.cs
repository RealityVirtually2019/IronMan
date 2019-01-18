using UnityEngine;


namespace IronManUI {

    public abstract class AbstractIMComponent : MonoBehaviour {
        protected Spring3Motion translationMotion = new Spring3Motion();
        
        public void Update() {

            translationMotion.Update(Time.deltaTime);
            transform.position = translationMotion.position;

        }

    }


}