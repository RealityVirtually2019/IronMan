/**
 * Author:    Aaron Moffatt
 * Created:   01.19.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;

namespace IronManUI {

    [System.Serializable]
    public class Slide : MonoBehaviour {

        public PresentationManager manager;

        public bool IsCurrentSlide() {
            if (manager == null)
                return false;
            return manager.currentSlide == this;
        }

        public void SetModel(PresentationManager manager, SlideModel slideModel) {
            this.manager = manager;
            name = slideModel.name;
            transform.position = slideModel.position;

            foreach (var compModel in slideModel.components) {
                var comp = manager.InstantiateComponent(compModel);
                if (comp != null)
                    comp.transform.parent = transform;
            }
        }

        public SlideModel ExtractModel() {
            var model = new SlideModel();
            model.name = name;

            foreach (var comp in GetComponentsInChildren<AbstractIMComponent>()) {
                model.components.Add(comp.model);
            }
            return model;
        }

        public void Activate() {

        }

        public void Deactivate() {

        }


    }
}