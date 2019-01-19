using UnityEngine;

namespace IronManUI {

    public class SlideWaypoint : MonoBehaviour {
        private Spring3Motion scalingMotion = new Spring3Motion();

        public Slide slide;
        public PresentationManager manager {
            get {
                if (slide == null)
                    return null;
                return slide.manager;
            }
        }

        public bool IsCurrentSlide() {
            if (slide == null)
                return false;
            return slide.IsCurrentSlide();
        }

        void Update() {
            scalingMotion.origin = IsCurrentSlide() ? Vector3.zero : Vector3.one;
            scalingMotion.Update();
            transform.localScale = scalingMotion.position;
        }

        void SetSlide(Slide slide) {
            this.slide = slide;
        }

        void OnCollisionEnter(Collision collision) {
            if (manager != null) {
                manager.SlideWaypointTouched(slide);
            }
        }

    }

}