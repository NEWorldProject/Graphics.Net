// OpenGL: DataBuffer.cs
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
using System.Runtime.InteropServices;
using Core;

namespace OpenGL
{
    public static partial class Gl
    {
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
        
        internal unsafe delegate void GenBuffersProc(int n, uint* buffers);

        internal unsafe delegate void DeleteBuffersProc(int n, uint* buffers);

        internal unsafe delegate void NamedBufferDataProc(uint buffer, UIntPtr size, void* data, uint usage);

        internal delegate void BindBufferBaseProc(uint target, uint index, uint buffer);

        internal unsafe delegate void NamedBufferSubDataProc(uint buffer, UIntPtr offset, UIntPtr size,
            void* data);

        internal static GenBuffersProc GenBuffers;
        internal static DeleteBuffersProc DeleteBuffers;
        internal static NamedBufferDataProc NamedBufferData;
        internal static BindBufferBaseProc BindBufferBase;
        internal static NamedBufferSubDataProc NamedBufferSubData;

        static partial void InitDataBuffer()
        {
            GenBuffers = Get<GenBuffersProc>("glGenBuffers");
            DeleteBuffers = Get<DeleteBuffersProc>("glDeleteBuffers");
            NamedBufferData = Get<NamedBufferDataProc>("glNamedBufferDataEXT");
            BindBufferBase = Get<BindBufferBaseProc>("glBindBufferBase");
            NamedBufferSubData = Get<NamedBufferSubDataProc>("glNamedBufferSubDataEXT");
        }
    }

    public class DataBuffer : StrictDispose
    {
        public unsafe DataBuffer()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.GenBuffers(1, addr);
            }
        }

        protected override unsafe void Release()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.DeleteBuffers(1, addr);
            }
        }

        public unsafe void DataRaw(int size, void* data, uint usage) =>
            Gl.NamedBufferData(_hdc, (UIntPtr) size, data, usage);

        public unsafe void DataSectionRaw(uint offset, int size, void* data) =>
            Gl.NamedBufferSubData(_hdc, (UIntPtr) offset, (UIntPtr) size, data);

        public uint Raw() => _hdc;

        private uint _hdc;

        public unsafe void Data(float[] data, uint usage)
        {
            var ptr = Marshal.AllocHGlobal(data.Length * sizeof(float));
            Marshal.Copy(data, 0, ptr, data.Length);
            DataRaw(data.Length * sizeof(float), (void*) ptr, usage);
            Marshal.FreeHGlobal(ptr);
        }

        public unsafe void DataSection(uint offset, float[] data)
        {
            var ptr = Marshal.AllocHGlobal(data.Length * sizeof(float));
            Marshal.Copy(data, 0, ptr, data.Length);
            DataSectionRaw(offset, data.Length * sizeof(float), (void*) ptr);
            Marshal.FreeHGlobal(ptr);
        }

        public void BindBase(uint usage, uint index) => Gl.BindBufferBase(usage, index, _hdc);
    }
}