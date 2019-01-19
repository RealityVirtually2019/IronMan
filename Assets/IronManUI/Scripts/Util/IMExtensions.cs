using UnityEngine;

using System.Collections.Generic;

namespace IronManUI {

    public static class IMExtensions {
        public static AbstractIMComponent GetIronManComponent(this Collider collider) {
            return collider.gameObject.GetComponent<AbstractIMComponent>();
        }

        public static AbstractIMComponent GetIronManComponent(this Collision collision) {
            return collision.gameObject.GetComponent<AbstractIMComponent>();
        }

        public static void DestroyAllGameObjects<T>(this T[] toDestroy) where T : Object {
            foreach (var o in toDestroy)
                Object.Destroy(o);
        }

    }
}