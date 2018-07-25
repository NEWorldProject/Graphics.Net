// Core: Math.cs
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
        public static double Sqrt(dynamic a) => System.Math.Sqrt((double) a);
        public static double Sin(dynamic a) => System.Math.Sin((double) a);
        public static double Cos(dynamic a) => System.Math.Cos((double) a);
        public static double Tan(dynamic a) => System.Math.Tan((double) a);
    }
}