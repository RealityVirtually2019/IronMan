/**
 * Author:    Aaron Moffatt, Bob Levy
 * Created:   01.20.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;


namespace IronManUI {

    public class IMComponentMenuHandler {
        public readonly AbstractIMComponent component;
        

        public IMComponentMenuHandler(AbstractIMComponent parent)
        {
            this.component = parent;
        }

        public void Update() {

        }

        public void CheckShouldTrash() {

        }

        public void AddToCurrentScene() {
            var newParent = PresentationManager.instance.currentSlide;
            if (newParent != null)
                component.transform.parent = newParent.transform;
        }

        public void RemoveFromCurrentScene() {
            var parentSlide = component.GetComponentInParent<Slide>();
            if (parentSlide != null) {

            }
        }

    }

}

