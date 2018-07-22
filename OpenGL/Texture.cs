// OpenGL: Texture.cs
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
        internal const uint Texture2D = 0x0DE1;
        internal const uint Texture0 = 0x84C0;

        internal unsafe delegate void GenTexturesProc(int n, uint* textures);

        internal unsafe delegate void DeleteTexturesProc(int n, uint* textures);

        internal delegate void TextureParameteriProc(uint texture, uint target, uint pname, int param);

        internal delegate void TextureParameterfProc(uint texture, uint target, uint pname, float param);

        internal delegate void BindTextureProc(uint target, uint texture);

        internal delegate void ActiveTextureProc(uint slot);

        internal delegate void TextureImage2DProc(uint texture, uint target, int level, int internalformat, int width,
            int height, int border, uint format, uint type, byte[] pixels);

        internal static GenTexturesProc GenTextures;
        internal static DeleteTexturesProc DeleteTextures;
        internal static TextureParameteriProc TextureParameteri;
        internal static TextureParameterfProc TextureParameterf;
        internal static BindTextureProc BindTexture;
        internal static ActiveTextureProc ActiveTexture;
        internal static TextureImage2DProc TextureImage2D;

        static partial void InitTexture()
        {
            GenTextures = Get<GenTexturesProc>("glGenTextures");
            DeleteTextures = Get<DeleteTexturesProc>("glDeleteTextures");
            TextureParameteri = Get<TextureParameteriProc>("glTextureParameteriEXT");
            TextureParameterf = Get<TextureParameterfProc>("glTextureParameterfEXT");
            BindTexture = Get<BindTextureProc>("glBindTexture");
            ActiveTexture = Get<ActiveTextureProc>("glActiveTexture");
            TextureImage2D = Get<TextureImage2DProc>("glTextureImage2DEXT");
        }
    }

    public class Texture : StrictDispose
    {
        public unsafe Texture()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.GenTextures(1, addr);
            }
        }

        protected override unsafe void Release()
        {
            fixed (uint* addr = &_hdc)
            {
                Gl.DeleteTextures(1, addr);
            }
        }

        public void SetParameter(uint name, int param) => Gl.TextureParameteri(_hdc, Gl.Texture2D, name, param);

        public void SetParameter(uint name, float param) => Gl.TextureParameterf(_hdc, Gl.Texture2D, name, param);

        public void Use(uint slot)
        {
            // Lack of DSA, simulate it
            Gl.ActiveTexture(Gl.Texture0 + slot);
            Gl.BindTexture(Gl.Texture2D, _hdc);
        }

        public void Image(int level, int internalFormat, Vec2<int> size, uint format, uint type, byte[] data) =>
            Gl.TextureImage2D(_hdc, Gl.Texture2D, level, internalFormat, size.X, size.Y, 0, format, type, data);

        public uint Raw() => _hdc;

        private uint _hdc;
    }
}