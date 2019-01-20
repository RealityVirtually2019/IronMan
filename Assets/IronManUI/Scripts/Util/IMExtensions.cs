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

        public static void DestroyChildren(this GameObject o) {
            var t = o.transform;
            bool editMode = Application.isEditor && !Application.isPlaying;
            int count = t.childCount;

            for (int i=0; i<count; i++) {
                var child = t.GetChild(i).gameObject;
                // child.transform.parent = null;

                if (editMode)
                    Object.DestroyImmediate(child);
                else
                    Object.Destroy(child.gameObject);
            }
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

        public static Vector3 MinValue(this Vector3 v, float min) {
            var outV = v;
            if (outV.x < min) outV.x = min;
            if (outV.y < min) outV.y = min;
            if (outV.z < min) outV.z = min;
            return outV;
        }

    }
}