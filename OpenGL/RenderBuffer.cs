// OpenGL: RenderBuffer.cs
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

using Core;

namespace OpenGL
{
    public static partial class Gl
    {
        internal unsafe delegate void GenRenderbuffersProc(int n, uint* renderbuffers);

        internal unsafe delegate void DeleteRenderbuffersProc(int n, uint* renderbuffers);

        internal delegate void NamedRenderbufferStorageProc(uint renderbuffer, uint format, int width, int height);

        internal static GenRenderbuffersProc GenRenderbuffers;
        internal static DeleteRenderbuffersProc DeleteRenderbuffers;
        internal static NamedRenderbufferStorageProc NamedRenderbufferStorage;

        static partial void InitRenderBuffer()
        {
            GenRenderbuffers = Get<GenRenderbuffersProc>("glGenRenderbuffers");
            DeleteRenderbuffers = Get<DeleteRenderbuffersProc>("glDeleteRenderbuffers");
            NamedRenderbufferStorage = Get<NamedRenderbufferStorageProc>("glNamedRenderbufferStorageEXT");
        }
    }

    public class RenderBuffer : StrictDispose
    {
        public unsafe RenderBuffer()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.GenRenderbuffers(1, addr);
            }
        }

        protected override unsafe void Release()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.DeleteRenderbuffers(1, addr);
            }
        }

        public void SetStorage(uint fmt, Vec2<int> size) =>
            Gl.NamedRenderbufferStorage(_hdc, fmt, size.X, size.Y);

        public uint Raw() => _hdc;

        private uint _hdc;
    }
}