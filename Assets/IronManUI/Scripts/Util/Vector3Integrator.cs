/**
 * Author:    Aaron Moffatt
 * Created:   01.18.2019
 * 
 * MIT Reality Virtually Hackathon
 **/

using UnityEngine;

namespace IronManUI {

    /** AM: Provides basic physical motion influenced by acceleration inputs */
    public class Vector3Motion {
        public float energyLoss = 4f;

        public Vector3 position;
    
        private Vector3 accel;
        public Vector3 velocity { get; private set; }
        
        public void AddAcceleration(Vector3 accel) {
            this.accel += accel;
        }

        virtual public void Update(float deltaTime) {

            velocity += accel * deltaTime;
            velocity *= Mathf.Max(0, (1 - energyLoss * deltaTime));
            position += velocity * deltaTime + accel * (.5f * deltaTime * deltaTime);
            accel = Vector3.zero;
        }

    }

    public class Spring3Motion : Vector3Motion {

        public Vector3? origin;

        /** AM: Controls strength of spring acceleration toward the origin */
        public float springK = 20;

        public void Update() {
            Update(Time.deltaTime);
        }

        override public void Update(float deltaTime) {
            
            if (springK > 0 && origin.HasValue)
                AddAcceleration((origin.Value - position) * springK);
            base.Update(deltaTime);
        }
    }


}