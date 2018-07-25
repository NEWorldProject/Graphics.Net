// Core: Arithmetic.cs
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
        public static dynamic Cast<T>(dynamic a) => (T) a;
        public static dynamic Add(dynamic a, dynamic b) => a + b;
        public static dynamic Substract(dynamic a, dynamic b) => a - b;
        public static dynamic Multiply(dynamic a, dynamic b) => a * b;
        public static dynamic Divide(dynamic a, dynamic b) => a / b;
        public static dynamic Modulus(dynamic a, dynamic b) => a % b;
        public static dynamic AddBy(ref dynamic a, dynamic b) => a += b;
        public static dynamic SubstractBy(ref dynamic a, dynamic b) => a -= b;
        public static dynamic MultiplyBy(ref dynamic a, dynamic b) => a *= b;
        public static dynamic DivideBy(ref dynamic a, dynamic b) => a /= b;
        public static dynamic ModulusBy(ref dynamic a, dynamic b) => a %= b;
        public static dynamic Square(dynamic a) => a * a;
    }
}