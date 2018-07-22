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

namespace Core
{
    public struct Vec2<T>
    {
        private T[] Data;

        public Vec2(T[] data) => Data = data;

        public Vec2(T x, T y) => Data = new[] {x, y};

        public T X
        {
            get => Data[0];
            set => Data[0] = value;
        }

        public T Y
        {
            get => Data[1];
            set => Data[1] = value;
        }
    }

    public struct Vec3<T>
    {
        private T[] Data;

        public Vec3(T[] data) => Data = data;

        public Vec3(T x, T y, T z) => Data = new[] {x, y, z};

        public T X
        {
            get => Data[0];
            set => Data[0] = value;
        }

        public T Y
        {
            get => Data[1];
            set => Data[1] = value;
        }

        public T Z
        {
            get => Data[2];
            set => Data[2] = value;
        }

        public void Normalize()
        {
        }
    }
}