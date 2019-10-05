using System;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;


namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicAvoidCharacter : DynamicMovement
    {
        public float MaxTimeLookAhead { get; set; }
        public float AvoidMargin { get; set; }

        private KinematicData OtherCharacter;

        public override string Name
        {
            get { return "Avoid Character"; }
        }

        public DynamicAvoidCharacter(KinematicData other)
        {
            this.Output = new MovementOutput();
            this.OtherCharacter = other;
        }

        public override MovementOutput GetMovement()
        {
            var DeltaPos = this.OtherCharacter.Position - this.Character.Position;
            var DeltaVel = this.OtherCharacter.velocity - this.Character.velocity;

            var DeltaSqrSpeed = DeltaVel.sqrMagnitude;

            if (DeltaSqrSpeed == 0) return this.Output;

            var TimeToClosest = -Vector3.Dot(DeltaPos, DeltaVel) / DeltaSqrSpeed;

            if (TimeToClosest > MaxTimeLookAhead) return this.Output;

            var FutureDeltaPos = DeltaPos + DeltaVel * TimeToClosest;
            var FutureDistance = FutureDeltaPos.magnitude;

            if (FutureDistance > 2 * AvoidMargin) return this.Output;

            if (FutureDistance <= 0 || DeltaPos.magnitude < 2 * AvoidMargin)
            {
                this.Output.linear = this.Character.Position - this.OtherCharacter.Position;
            } 
            else 
            {
                this.Output.linear = FutureDeltaPos * -1;
            }

            this.Output.linear = this.Output.linear.normalized * this.MaxAcceleration;

            return Output;
        }
    }
}
