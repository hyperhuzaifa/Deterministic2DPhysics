﻿using FixedMaths.Core;
using Svelto.ECS;

namespace Physics.Core.EntityComponents
{
    public readonly struct TransformEntityComponent : IEntityComponent
    {
        public static TransformEntityComponent From(FixedPointVector2 position, FixedPointVector2 positionLastPhysicsTick, FixedPointVector2? positionMidpoint = null)
        {
            return new TransformEntityComponent(position, positionLastPhysicsTick, positionMidpoint);
        }
        
        private readonly FixedPointVector2? _positionMidpoint;
        //private readonly FixedPointVector2? _rotationMidpoint;
        //private readonly FixedPointVector2? _scaleMidpoint;
        
        public readonly FixedPointVector2 Position;
        public readonly FixedPointVector2 PositionLastPhysicsTick;
        
        //public readonly FixedPointVector2 Rotation;
        //public readonly FixedPointVector2 RotationLastPhysicsTick;
        
        //public readonly FixedPointVector2 Scale;
        //public readonly FixedPointVector2 ScaleLastPhysicsTick;

        private TransformEntityComponent(FixedPointVector2 position, FixedPointVector2 positionLastPhysicsTick, FixedPointVector2? positionMidpoint = null)
        {
            Position = position;
            PositionLastPhysicsTick = positionLastPhysicsTick;
            _positionMidpoint = positionMidpoint;
        }

        public TransformEntityComponent CloneWithUpdatedPositionMidpoint(FixedPointVector2? positionMidpoint)
        {
            return new TransformEntityComponent(Position, PositionLastPhysicsTick, positionMidpoint);
        }

        public FixedPointVector2 Interpolate(FixedPoint delta)
        {
            if (!_positionMidpoint.HasValue)
            {
                return FixedPointVector2.Interpolate(PositionLastPhysicsTick, Position, delta);
            }

            var magnitude = MathFixedPoint.Magnitude(Position - _positionMidpoint.Value);
            var lastMagnitude = MathFixedPoint.Magnitude(PositionLastPhysicsTick - _positionMidpoint.Value);
            var midPointFp = lastMagnitude / (magnitude + lastMagnitude + FixedPoint.Kludge);

            if (delta < midPointFp)
            {
                var midpointDeltaFp = (delta - midPointFp) / midPointFp + FixedPoint.One;

                return FixedPointVector2.Interpolate(PositionLastPhysicsTick, _positionMidpoint.Value, midpointDeltaFp);
            }
            else
            {
                var midpointDeltaFp = (delta - midPointFp) / (FixedPoint.One - midPointFp); 

                return FixedPointVector2.Interpolate(_positionMidpoint.Value, Position, midpointDeltaFp);
            }
        }
    }
}