// OpenGL: Basics.cs
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
using System.Text;

namespace OpenGL
{
    public static partial class Gl
    {
        public const uint Byte = 0x1400;
        public const uint UnsignedByte = 0x1401;
        public const uint Short = 0x1402;
        public const uint UnsignedShort = 0x1403;
        public const uint Int = 0x1404;
        public const uint UnsignedInt = 0x1405;
        public const uint Float = 0x1406;
        public const uint Double = 0x140A;
        public const uint StreamDraw = 0x88E0;
        public const uint StreamRead = 0x88E1;
        public const uint StreamCopy = 0x88E2;
        public const uint StaticDraw = 0x88E4;
        public const uint StaticRead = 0x88E5;
        public const uint StaticCopy = 0x88E6;
        public const uint DynamicDraw = 0x88E8;
        public const uint DynamicRead = 0x88E9;
        public const uint DynamicCopy = 0x88EA;
        public const uint UniformBuffer = 0x8A11;
        public const uint Lines = 0x0001;
        public const uint LineLoop = 0x0002;
        public const uint LineStrip = 0x0003;
        public const uint Triangles = 0x0004;
        public const uint TriangleStrip = 0x0005;
        public const uint TriangleFan = 0x0006;
        public const uint FragmentShader = 0x8B30;
        public const uint VertexShader = 0x8B31;
        public const uint GeometryShader = 0x8DD9;
        public const uint InfoLogLength = 0x8B84;
        public const uint CompileStatus = 0x8B81;
        public const uint LinkStatus = 0x8B82;
        public const uint ColorBufferBit = 0x4000;
        public const uint DepthBufferBit = 0x0100;
        public const uint StencilBufferBit = 0x0400;

        internal static byte[] Utf8ToNative(string s) => s == null ? null : Encoding.UTF8.GetBytes(s + "\0");

        internal static unsafe string Utf8ToManaged(IntPtr s)
        {
            var sBase = (byte*) s;
            if (sBase == null)
                return null;
            var numPtr = (byte*) s;
            while (*numPtr != 0)
                ++numPtr;
            var num = (int) (numPtr - sBase);
            char* chars1 = stackalloc char[num];
            var chars2 = Encoding.UTF8.GetChars(sBase, num, chars1, num);
            var str = new string(chars1, 0, chars2);
            return str;
        }
    }
}