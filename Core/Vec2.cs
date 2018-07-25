// Core: Vec2.cs
// Graphics.Net: General Application Framework API and GUI For .Net
// Copyright (C) 2015-2018 NEWorld Team
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace Core
{
    using static Generic;
    
    public struct Vec2<T>
    {
        public Vec2(T x, T y)
        {
            X = x;
            Y = y;
        }

        public T X, Y;

        public double LengthSqr() => Square(X) + Square(Y);

        public double Length() => Math.Sqrt(LengthSqr());
        
        public void Normalize()
        {
            object length = Cast<T>(Length());
            X = Divide(X, length);
            Y = Divide(Y, length);
        }
        
        public static Vec2<T> operator + (Vec2<T> lhs, Vec2<T> rhs) => 
            new Vec2<T>(Add(lhs.X, rhs.X), Add(lhs.Y, rhs.Y));

        public static Vec2<T> operator - (Vec2<T> lhs, Vec2<T> rhs) =>
            new Vec2<T>(Substract(lhs.X, rhs.X), Substract(lhs.Y, rhs.Y));
    }

    public struct Vec3<T>
    {
        public Vec3(T x, T y, T z) 
        {
            X = x;
            Y = y;
            Z = z;
        }

        public T X, Y, Z;

        public double LengthSqr() => Square(X) + Square(Y) + Square(Z);

        public double Length() => Math.Sqrt(LengthSqr());

        public void Normalize()
        {
            object length = Cast<T>(Length());
            X = Divide(X, length);
            Y = Divide(Y, length);
            Z = Divide(Z, length);
        }
        
        public static Vec3<T> operator + (Vec3<T> lhs, Vec3<T> rhs) =>
            new Vec3<T>(Add(lhs.X, rhs.X), Add(lhs.Y, rhs.Y), Add(lhs.Z, rhs.Z));

        public static Vec3<T> operator - (Vec3<T> lhs, Vec3<T> rhs) =>
            new Vec3<T>(Substract(lhs.X, rhs.X), Substract(lhs.Y, rhs.Y), Substract(lhs.Z, rhs.Z));
    }
}