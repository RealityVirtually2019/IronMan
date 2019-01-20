using UnityEngine;
using TMPro;

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

        //TODO Needs TLC
        public static Bounds GetBounds(this GameObject o) {
            Bounds bounds = new Bounds();
            int i = 0;

            foreach (var renderer in o.GetComponentsInChildren<MeshRenderer>()) {
                if (i++ == 0)
                    bounds = renderer.bounds;
                else
                    bounds.Encapsulate(renderer.bounds);
            }

            foreach (var renderer in o.GetComponentsInChildren<TextMeshPro>()) {
                if (i++ == 0)
                    bounds = renderer.bounds;
                else
                    bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }

    }
}