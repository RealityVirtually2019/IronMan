using UnityEngine;

namespace IronManUI {

    /** AM: Provides basic physical motion influenced by acceleration inputs */
    public class Vector3Motion {
        public float dragCoeff;

        public Vector3 position;
    
        private Vector3 accel;
        private Vector3 velocity;
        
        public void AddAcceleration(Vector3 accel) {
            this.accel += accel;
        }

        virtual public void Update(float deltaTime) {

            velocity += (accel - velocity * dragCoeff) * deltaTime;
            position += velocity * deltaTime + accel * deltaTime * deltaTime;

            accel = Vector3.zero;
        }

    }

    public class Spring3Motion : Vector3Motion {

        public Vector3 origin;

        /** AM: Controls strength of spring acceleration toward the origin */
        public float springK;

        override public void Update(float deltaTime) {
            
            if (springK > 0)
                AddAcceleration((origin - position) * springK);
            base.Update(deltaTime);
        }
    }


}