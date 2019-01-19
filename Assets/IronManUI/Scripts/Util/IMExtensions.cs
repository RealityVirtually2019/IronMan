using UnityEngine;

namespace IronManUI {

    public static class IMExtensions {
        public static AbstractIMComponent GetIronManComponent(this Collider collider) {
            return collider.gameObject.GetComponent<AbstractIMComponent>();
        }

        public static AbstractIMComponent GetIronManComponent(this Collision collision) {
            return collision.gameObject.GetComponent<AbstractIMComponent>();
        }

    }
}