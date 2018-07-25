// OpenGL: FrameBuffer.cs
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

using System.Net.Configuration;
using Core;

namespace OpenGL
{
    public static partial class Gl
    {
        public const uint FrameBuffer = 0x8D40;
        public const uint ColorAttachment0 = 0x8CE0;
        public const uint ColorAttachment1 = 0x8CE1;
        public const uint ColorAttachment2 = 0x8CE2;
        public const uint ColorAttachment3 = 0x8CE3;
        public const uint ColorAttachment4 = 0x8CE4;
        public const uint ColorAttachment5 = 0x8CE5;
        public const uint ColorAttachment6 = 0x8CE6;
        public const uint ColorAttachment7 = 0x8CE7;
        public const uint ColorAttachment8 = 0x8CE8;
        public const uint ColorAttachment9 = 0x8CE9;
        public const uint ColorAttachment10 = 0x8CEA;
        public const uint ColorAttachment11 = 0x8CEB;
        public const uint ColorAttachment12 = 0x8CEC;
        public const uint ColorAttachment13 = 0x8CED;
        public const uint ColorAttachment14 = 0x8CEE;
        public const uint ColorAttachment15 = 0x8CEF;
        public const uint DepthAttachment = 0x8D00;
        public const uint StencilAttachment = 0x8D20;
        
        internal unsafe delegate void CreateFramebuffersProc(int n, uint* framebuffers);

        internal unsafe delegate void DeleteFramebuffersProc(int n, uint* framebuffers);

        internal delegate void BindFramebufferProc(uint target, uint texture);

        internal delegate void NamedFramebufferTextureProc(uint framebuffer, uint attachment, uint texture, int level);

        internal delegate void NamedFramebufferRenderbufferProc(uint framebuffer, uint attachment,
            uint renderbuffertarget, uint renderbuffer);

        internal static CreateFramebuffersProc CreateFramebuffers;
        internal static DeleteFramebuffersProc DeleteFramebuffers;
        internal static BindFramebufferProc BindFramebuffer;
        internal static NamedFramebufferTextureProc NamedFramebufferTexture;
        internal static NamedFramebufferRenderbufferProc NamedFramebufferRenderbuffer;

        static partial void InitFrameBuffer()
        {
            CreateFramebuffers = Get<CreateFramebuffersProc>("glCreateFramebuffers");
            DeleteFramebuffers = Get<DeleteFramebuffersProc>("glDeleteFramebuffers");
            BindFramebuffer = Get<BindFramebufferProc>("glBindFramebuffer");
            NamedFramebufferTexture = Get<NamedFramebufferTextureProc>("glNamedFramebufferTexture");
            NamedFramebufferRenderbuffer = Get<NamedFramebufferRenderbufferProc>("glNamedFramebufferRenderbuffer");
        }
    }

    public class FrameBuffer : StrictDispose
    {
        public unsafe FrameBuffer()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.CreateFramebuffers(1, addr);
            }
        }

        protected override unsafe void Release()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.DeleteFramebuffers(1, addr);
            }
        }
        
        public static void UseDefault() => Gl.BindFramebuffer(Gl.FrameBuffer, 0);

        public void Use() => Gl.BindFramebuffer(Gl.FrameBuffer, _hdc);

        public void Texture(uint attachment, Texture texture, int level) =>
            Gl.NamedFramebufferTexture(_hdc, attachment, texture?.Raw() ?? 0, level);

        public void RenderBuffer(uint attachment, RenderBuffer buffer) =>
            Gl.NamedFramebufferRenderbuffer(_hdc, attachment, Gl.RenderBuffer, buffer?.Raw() ?? 0);

        public uint Raw() => _hdc;

        private uint _hdc;
    }
}