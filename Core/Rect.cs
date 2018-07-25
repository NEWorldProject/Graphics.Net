// Core: Rect.cs
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
    using static Generic;
    
    public struct Rect<T>
    {
        public Vec2<T> Min, Max;

        public Rect(Vec2<T> min, Vec2<T> max)
        {
            Min = min;
            Max = max;
        }

        public Rect(T minX, T minY, T maxX, T maxY)
        {
            Min = new Vec2<T>(minX, minY);
            Max = new Vec2<T>(maxX, maxY);
        }
        
        public Vec2<T> Delta => Max - Min;

        public Rect(Vec2<T> start, params Vec2<T>[] args)
        {
            object minX = start.X, minY = start.Y;
            object maxX = start.X, maxY = start.Y;
            foreach (var point in args)
            {
                object boxX = point.X, boxY = point.Y;
                MinEqual(ref minX, boxX);
                MinEqual(ref minY, boxY);
                MaxEqual(ref maxX, boxX);
                MaxEqual(ref maxY, boxY);
            }
            Min = new Vec2<T>((T)minX, (T)minY);
            Max = new Vec2<T>((T)maxX, (T)maxY);
        }
        
        public Rect(params T[] args)
        {
            object minX = args[0], minY = args[1];
            object maxX = args[0], maxY = args[1];
            for (var i = 2; i < args.Length; ++i)
            {
                object boxX = args[i++], boxY = args[i];
                MinEqual(ref minX, boxX);
                MinEqual(ref minY, boxY);
                MaxEqual(ref maxX, boxX);
                MaxEqual(ref maxY, boxY);
            }
            Min = new Vec2<T>((T)minX, (T)minY);
            Max = new Vec2<T>((T)maxX, (T)maxY);
        }
    }
}
