using UnityEngine;

namespace IronManUI {

    public class SlideWaypoint : MonoBehaviour {

        public Slide slide;
        public PresentationManager manager {
            get {
                if (slide == null)
                    return null;
                return slide.manager;
            }
        }


        private Spring3Motion scalingMotion = new Spring3Motion();
        private BoxCollider boxCollider;


        public bool IsCurrentSlide() {
            if (slide == null)
                return false;
            return slide.IsCurrentSlide();
        }

        void OnEnable() {
            boxCollider = gameObject.AddComponent<BoxCollider>();

            UpdateColliderBounds();
        }

        void Update() {

            scalingMotion.origin = IsCurrentSlide() ? Vector3.zero : Vector3.one;
            scalingMotion.Update();
            transform.localScale = scalingMotion.position;
        }

        void UpdateColliderBounds() {
            var bounds = gameObject.GetBounds();

            boxCollider.size = bounds.size;
            // boxCollider.center = bounds.center;     //bounds calculation needs to be fixed before using this
        }
        

        void SetSlide(Slide slide) {
            this.slide = slide;
        }

        void OnCollisionEnter(Collision collision) {
            HandleClick();
        }

        void OnMouseDown() {
            HandleClick();
        }

        public void HandleClick() {
            Debug.Log("Waypoint Clicked: " + slide);
            if (manager != null) {
                manager.SlideWaypointTouched(slide);
            }
        }

    }

}