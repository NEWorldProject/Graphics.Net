// Core: Utilities.cs
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
    public static partial class Generic
    {
        public static dynamic Min(dynamic a, dynamic b) => Less(a, b) ? a : b;
        public static dynamic Max(dynamic a, dynamic b) => Larger(a, b) ? a : b;

        public static void MinEqual(ref dynamic a, dynamic b)
        {
            if (Less(b, a)) a = b;
        }

        public static void MaxEqual(ref dynamic a, dynamic b)
        {
            if (Larger(b, a)) a = b;
        }
        
        public static void Swap<T>(ref T a, ref T b)
        {
            var t = a;
            a = b;
            b = t;
        }
    }
}
