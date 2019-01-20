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
        public float energyLoss = 6f;

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
        public float springK = 50;

        public void Update() {
            Update(Time.deltaTime);
        }

        override public void Update(float deltaTime) {
            
            if (springK > 0 && origin.HasValue)
                AddAcceleration(CalculateDelta() * springK);
            base.Update(deltaTime);
        }

        virtual protected Vector3 CalculateDelta() {
            return (origin.Value - position);
        }
    }


    /* Angle in degrees */
    public class Spring3AngularMotion : Spring3Motion {
        override public void Update(float deltaTime) {
            base.Update(deltaTime);

            position = Modulus(position, 360);
        }

        //TODO AM cleanup/optimize
        override protected Vector3 CalculateDelta() {
            Vector3 delta = Modulus(origin.Value - position, 360);        //AM this modulus might not be necessary     
            if (delta.x > 180) delta.x -= 360;
            if (delta.y > 180) delta.y -= 360;
            if (delta.z > 180) delta.z -= 360;
            return delta;
        }

        public static float Modulus(float x, float m) {
            return ((x % m) + m) % m;
        }

        public static Vector3 Modulus(Vector3 angle, float m) {
            angle.x = Modulus(angle.x, m);
            angle.y = Modulus(angle.y, m);
            angle.z = Modulus(angle.z, m);
            return angle;
        }

    }


}