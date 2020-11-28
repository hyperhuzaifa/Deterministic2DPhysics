﻿using System;
using Svelto.Common;
using Svelto.ECS;
using SveltoDeterministic2DPhysicsDemo.Graphics;
using SveltoDeterministic2DPhysicsDemo.Maths;
using SveltoDeterministic2DPhysicsDemo.Physics.EntityComponents;
using SveltoDeterministic2DPhysicsDemo.Physics.Loggers;
using SveltoDeterministic2DPhysicsDemo.Physics.Loggers.Data;
using Colour = SveltoDeterministic2DPhysicsDemo.Graphics.Colour;

namespace SveltoDeterministic2DPhysicsDemo.Physics.Engines
{
    [Sequenced(nameof(PhysicsEngineNames.DebugPhysicsDrawEngine))]
    public class DebugPhysicsDrawEngine : IQueryingEntitiesEngine, IScheduledGraphicsEngine
    {
        private readonly EngineScheduler _engineScheduler;
        private readonly IGraphics _graphics;
        
        public string Name => nameof(DebugPhysicsDrawEngine);
        public EntitiesDB entitiesDB { get; set; }

        public DebugPhysicsDrawEngine(EngineScheduler engineScheduler, IGraphics graphics)
        {
            _engineScheduler = engineScheduler;
            _graphics = graphics;
        }

        public void Ready()
        {
            _engineScheduler.RegisterScheduledGraphicsEngine(this);
        }

        public void Draw(FixedPoint delta, ulong physicsTick)
        {
            foreach (var ((transforms, count), _) in entitiesDB.QueryEntities<TransformEntityComponent>(GameGroups.TransformGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transformEntityComponent = ref transforms[i];

                    var (drawX, drawY) = transformEntityComponent.Interpolate(delta);

                    _graphics.DrawPlus(Colour.Aqua, (int) Math.Round(drawX), (int) Math.Round(drawY), 3);
                }
            }
            
            foreach (var ((transforms, colliders, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, BoxColliderEntityComponent>(GameGroups.RigidBodyWithBoxColliderGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transformEntityComponent = ref transforms[i];
                    ref var boxColliderEntityComponent = ref colliders[i];

                    var point = transformEntityComponent.Interpolate(delta);
                    var aabb = boxColliderEntityComponent.ToAABB(point);

                    var (minX, minY) = aabb.Min;
                    var (maxX, maxY) = aabb.Max;

                    _graphics.DrawBox(Colour.PaleVioletRed, (int) Math.Round(minX), (int) Math.Round(minY), (int) Math.Round(maxX), (int) Math.Round(maxY));
                }
            }
            
            foreach (var ((transforms, colliders, count), _) in entitiesDB.QueryEntities<TransformEntityComponent, CircleColliderEntityComponent>(GameGroups.RigidBodyWithCircleColliderGroups))
            {
                for (var i = 0; i < count; i++)
                {
                    ref var transformEntityComponent = ref transforms[i];
                    ref var circleColliderEntityComponent = ref colliders[i];

                    var point = transformEntityComponent.Interpolate(delta);

                    var x = FixedPoint.ConvertToInteger(MathFixedPoint.Round(point.X + circleColliderEntityComponent.Center.X));
                    var y = FixedPoint.ConvertToInteger(MathFixedPoint.Round(point.Y + circleColliderEntityComponent.Center.Y));
                    var radius = FixedPoint.ConvertToInteger(circleColliderEntityComponent.Radius);
                    
                    _graphics.DrawCircle(Colour.PaleVioletRed, x, y, radius);
                }
            }

            foreach (var point in FixedPointVector2Logger.Instance.GetPoints(physicsTick))
            {
                var (drawX, drawY) = point.Point;

                switch (point.Shape)
                {
                    case Shape.Cross:
                        _graphics.DrawCross(point.Colour, (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Plus:
                        _graphics.DrawPlus(point.Colour, (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Circle:
                        _graphics.DrawCircle(point.Colour, (int) Math.Round(drawX), (int) Math.Round(drawY), point.Radius ?? 3);
                        break;
                    case Shape.Box:
                        var (minX, minY) = point.BoxMin ?? FixedPointVector2.Zero;
                        var (maxX, maxY) = point.BoxMax ?? FixedPointVector2.Zero;
                        _graphics.DrawBox(point.Colour, (int)Math.Round(drawX + minX), (int)Math.Round(drawY + minY), (int)Math.Round(drawX + maxX), (int)Math.Round(drawY + maxY));
                        break;
                    case Shape.Line:
                        var (fromX, fromY) = point.BoxMin ?? FixedPointVector2.Zero;
                        var (toX, toY) = point.BoxMax ?? FixedPointVector2.Zero;
                        _graphics.DrawLine(point.Colour, (int)Math.Round(fromX), (int)Math.Round(fromY), (int)Math.Round(toX), (int)Math.Round(toY));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}