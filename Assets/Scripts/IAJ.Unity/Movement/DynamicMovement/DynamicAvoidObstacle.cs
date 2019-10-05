using System;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;


namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidObstacle : DynamicSeek
    {
        public float AvoidMargin { get; set; }
        public float MaxLookAhead { get; set; }

        private RaycastHit hit;
        private Collider CollisionDetector;

        public DynamicAvoidObstacle(GameObject obstacle)
        {
            this.CollisionDetector = obstacle.GetComponent<Collider>();
            base.Target = new KinematicData();
        }


        public override MovementOutput GetMovement()
        {
            if (base.Character.velocity.magnitude > 0)
            {
                Ray[] rays = new Ray[3];
                float[] lookAheads = new float[3];

                // Left whisker
                Vector3 newdirection = MathHelper.Rotate2D(base.Character.velocity, MathConstants.MATH_1_PI).normalized;
                rays[0] = new Ray(base.Character.Position, newdirection);
                lookAheads[0] = this.MaxLookAhead / 2;

                // Right whisker
                newdirection = MathHelper.Rotate2D(base.Character.velocity, -MathConstants.MATH_1_PI).normalized;
                rays[1] = new Ray(base.Character.Position, newdirection);
                lookAheads[1] = this.MaxLookAhead / 2;

                // centralRay
                rays[2] = new Ray(base.Character.Position, base.Character.velocity.normalized);
                lookAheads[2] = this.MaxLookAhead;

                bool collision;
                for (int i = 0; i < 3; i++)
                {
                    collision = this.CollisionDetector.Raycast(rays[i], out hit, lookAheads[i]);

                    if (collision)
                    {
                        base.Target.Position = hit.point + hit.normal * this.AvoidMargin;

                        return base.GetMovement();
                    }
                }

            }

            return new MovementOutput();
        }
    }
}
