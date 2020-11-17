﻿using System.Runtime.CompilerServices;

namespace FixedMaths.Core
{
    public readonly struct FixedPointVector2
    {
        public static FixedPointVector2 Up { get; } = new FixedPointVector2(FixedPoint.Zero, FixedPoint.One);
        public static FixedPointVector2 Down { get; } = new FixedPointVector2(FixedPoint.Zero, FixedPoint.NegativeOne);
        public static FixedPointVector2 Left { get; } = new FixedPointVector2(FixedPoint.NegativeOne, FixedPoint.Zero);
        public static FixedPointVector2 Right { get; } = new FixedPointVector2(FixedPoint.One, FixedPoint.Zero);
        public static FixedPointVector2 One { get; } = new FixedPointVector2(FixedPoint.One, FixedPoint.One);
        public static FixedPointVector2 Zero { get; } = new FixedPointVector2(FixedPoint.Zero, FixedPoint.Zero);
        
        public static FixedPointVector2 NegativeOne { get; } = new FixedPointVector2(FixedPoint.NegativeOne, FixedPoint.NegativeOne);
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 Interpolate(FixedPointVector2 v1, FixedPointVector2 v2, FixedPoint time)
        {
            return new FixedPointVector2(
                v1.X * (FixedPoint.One - time) + v2.X * time,
                v1.Y * (FixedPoint.One - time) + v2.Y * time
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPoint Dot(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPoint Cross(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPoint Distance(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return MathFixedPoint.Sqrt(((v1.X - v2.X) ^ FixedPoint.Two) + ((v1.Y - v2.Y) ^ FixedPoint.Two));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 From(FixedPoint x, FixedPoint y)
        {
            return new FixedPointVector2(x, y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 From(int x, int y)
        {
            return new FixedPointVector2(FixedPoint.From(x), FixedPoint.From(y));
        }

        public FixedPoint X { get; }
        public FixedPoint Y { get; }


        private FixedPointVector2(FixedPoint x, FixedPoint y)
        {
            X = x;
            Y = y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator +(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return new FixedPointVector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator +(FixedPointVector2 v1, FixedPoint fp)
        {
            return new FixedPointVector2(v1.X + fp, v1.Y + fp);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator -(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return new FixedPointVector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator -(FixedPointVector2 v1, FixedPoint fp)
        {
            return new FixedPointVector2(v1.X - fp, v1.Y - fp);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator -(FixedPointVector2 v1)
        {
            return new FixedPointVector2(-v1.X, -v1.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator *(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return new FixedPointVector2(v1.X * v2.X, v1.Y * v2.Y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator *(FixedPointVector2 v1, FixedPoint fp1)
        {
            return new FixedPointVector2(v1.X * fp1, v1.Y * fp1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator *(FixedPoint fp1, FixedPointVector2 v1)
        {
            return new FixedPointVector2(v1.X * fp1, v1.Y * fp1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator /(FixedPointVector2 v1, FixedPointVector2 v2)
        {
            return new FixedPointVector2(v1.X / v2.X, v1.Y / v2.Y);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FixedPointVector2 operator /(FixedPointVector2 v1, FixedPoint fp1)
        {
            return new FixedPointVector2(v1.X / fp1, v1.Y / fp1);
        }

        
        

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedPointVector2 Normalize()
        {
            var magnitude = MathFixedPoint.Magnitude(this);

            if (magnitude.Equals(FixedPoint.Zero))
            {
                return Sign();
            }

            return this / magnitude;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedPointVector2 NormalizeWithClampedMagnitude(FixedPoint clamp)
        {
            var magnitude = MathFixedPoint.ClampMagnitude(this, clamp);
                
            if (magnitude > FixedPoint.Zero)
            {
                return this / magnitude;
            }
            
            return this;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FixedPointVector2 Sign()
        {
            return new FixedPointVector2(MathFixedPoint.Sign(X), MathFixedPoint.Sign(Y));
        }
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return $"({X.ToString()}, {Y.ToString()})";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Deconstruct(out float x, out float y)
        {
            x = FixedPoint.ConvertToFloat(X);
            y = FixedPoint.ConvertToFloat(Y);
        }
    }
}