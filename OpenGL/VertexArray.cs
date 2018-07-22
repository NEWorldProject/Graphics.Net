// OpenGL: VertexArray.cs
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
using Core;

namespace OpenGL
{
    public static partial class Gl
    {
        internal unsafe delegate void GenVertexArraysProc(int n, uint* arrays);

        internal unsafe delegate void DeleteVertexArraysProc(int n, uint* arrays);

        internal delegate void BindVertexArrayProc(uint array);

        internal delegate void EnableVertexArrayAttribProc(uint vaobj, uint index);

        internal delegate void DisableVertexArrayAttribProc(uint vaobj, uint index);

        internal delegate void VertexArrayVertexAttribFormatProc(uint vaobj, uint attribindex, int size,
            uint type, byte normalized, uint relativeoffset);

        internal delegate void VertexArrayVertexAttribBindingProc(uint vaobj, uint attribindex, uint bindingindex);

        internal delegate void VertexArrayBindVertexBufferProc(uint vaobj, uint bindingindex, uint buffer,
            UIntPtr offset, int stride);

        internal static GenVertexArraysProc GenVertexArrays;
        internal static DeleteVertexArraysProc DeleteVertexArrays;
        internal static BindVertexArrayProc BindVertexArray;
        internal static EnableVertexArrayAttribProc EnableVertexArrayAttrib;
        internal static DisableVertexArrayAttribProc DisableVertexArrayAttrib;
        internal static VertexArrayVertexAttribFormatProc VertexArrayVertexAttribFormat;
        internal static VertexArrayVertexAttribBindingProc VertexArrayVertexAttribBinding;
        internal static VertexArrayBindVertexBufferProc VertexArrayBindVertexBuffer;

        static partial void InitVertexArray()
        {
            GenVertexArrays = Get<GenVertexArraysProc>("glGenVertexArrays");
            DeleteVertexArrays = Get<DeleteVertexArraysProc>("glDeleteVertexArrays");
            BindVertexArray = Get<BindVertexArrayProc>("glBindVertexArray");
            EnableVertexArrayAttrib = Get<EnableVertexArrayAttribProc>("glEnableVertexArrayAttribEXT");
            DisableVertexArrayAttrib = Get<DisableVertexArrayAttribProc>("glDisableVertexArrayAttribEXT");
            VertexArrayVertexAttribFormat =
                Get<VertexArrayVertexAttribFormatProc>("glVertexArrayVertexAttribFormatEXT");
            VertexArrayVertexAttribBinding =
                Get<VertexArrayVertexAttribBindingProc>("glVertexArrayVertexAttribBindingEXT");
            VertexArrayBindVertexBuffer = Get<VertexArrayBindVertexBufferProc>("glVertexArrayBindVertexBufferEXT");
        }
    }

    public class VertexArray : StrictDispose
    {
        public unsafe VertexArray()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.GenVertexArrays(1, addr);
            }
        }

        protected override unsafe void Release()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.DeleteVertexArrays(1, addr);
            }
        }

        public void Use() => Gl.BindVertexArray(_hdc);

        public void EnableAttrib(uint index) => Gl.EnableVertexArrayAttrib(_hdc, index);

        public void DisableAttrib(uint index) => Gl.DisableVertexArrayAttrib(_hdc, index);

        public void AttribFormat(uint index, int size, uint type, bool normalized, uint relativeOffset)
        {
            Use();
            byte norm = 0;
            if (normalized) norm = 1;
            Gl.VertexArrayVertexAttribFormat(_hdc, index, size, type, norm, relativeOffset);
        }

        public void AttribBinding(uint attribIndex, uint bufferIndex) =>
            Gl.VertexArrayVertexAttribBinding(_hdc, attribIndex, bufferIndex);

        public void BindBuffer(uint index, DataBuffer buffer, uint offset, int stride) =>
            Gl.VertexArrayBindVertexBuffer(_hdc, index, buffer.Raw(), (UIntPtr) offset, stride);

        public uint Raw() => _hdc;

        private uint _hdc;
    }
}