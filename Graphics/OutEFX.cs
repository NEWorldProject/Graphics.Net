// Graphics: OutEFX.cs
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
using OpenGL;

namespace Graphics
{
    public class OutEfxType : StrictDispose
    {
        public OutEfxType(string vert, string frag)
        {
            ShaderProgram = Inject(new Program());
            var vertex = new Shader(Gl.VertexShader, vert);
            var fragment = new Shader(Gl.FragmentShader, frag);
            ShaderProgram.Link(new[] {vertex, fragment});
            fragment.Dispose();
            vertex.Dispose();
        }

        public void Use() => ShaderProgram.Use();
        protected Program ShaderProgram;
    }

    public class OutEfx : StrictDispose
    {
        public virtual void Use() => _type.Use();
        protected OutEfx(OutEfxType type) => _type = type;
        private OutEfxType _type;
    }

    public class EfxColorTune : OutEfx
    {
        private const string Vertex = @"
#version 430 core
layout (location = 0) in vec2 position;
layout (std140, binding = 0) uniform vertexMvp { mat4 mvp; };
out vec2 texCrood;
void main() {
    gl_Position = mvp * vec4(position.x, position.y, 0.0, 1.0);
    texCrood = (vec2(gl_Position.x, gl_Position.y) + vec2(1.0, 1.0)) / 2.0;
}
";

        private const string Fragment = @"
#version 430 core
layout (std140, binding = 1) uniform efxData {
    vec4 color;
};
layout(binding = 2) uniform sampler2D implictInput;
in vec2 texCrood;
out vec4 outputColor;
void main() {
    outputColor = texture(implictInput, texCrood) * color;
}
";

        private class EfxColorTuneType : OutEfxType
        {
            private EfxColorTuneType() : base(Vertex, Fragment) => ShaderProgram.Uniform(2, 0);

            public static EfxColorTuneType Instance()
            {
                if (NeedNewInstance())
                    return _instance = CrossContextResource.Inject(new EfxColorTuneType());
                return _instance;
            }

            private static bool NeedNewInstance()
            {
                if (_instance == null) return true;
                return !_instance.Valid();
            }

            private static EfxColorTuneType _instance;
        }

        public EfxColorTune() : base(EfxColorTuneType.Instance())
        {
            _efxData = Inject(new DataBuffer());
            var zeroColor = new Rgba<float>(0.0f, 0.0f, 0.0f, 0.0f);
            _efxData.Data(zeroColor.Data, Gl.StaticDraw);
        }

        public EfxColorTune(Rgba<float> color) : base(EfxColorTuneType.Instance())
        {
            _efxData = Inject(new DataBuffer());
            _efxData.Data(color.Data, Gl.StaticDraw);
        }

        public override void Use()
        {
            base.Use();
            _efxData.BindBase(Gl.UniformBuffer, 1);
        }

        public void SetColor(Rgba<float> color) => _efxData.DataSection(8, color.Data);
        private DataBuffer _efxData;
    }
    
    public class EfxBlit : OutEfx
    {
        private const string Vertex = @"
#version 430 core
layout (location = 0) in vec2 position;
layout (std140, binding = 0) uniform vertexMvp { mat4 mvp; };
out vec2 texCrood;
void main() {
    gl_Position = mvp * vec4(position.x, position.y, 0.0, 1.0);
    texCrood = (vec2(gl_Position.x, gl_Position.y) + vec2(1.0, 1.0)) / 2.0;
}
";

        private const string Fragment = @"
#version 430 core
layout(binding = 1) uniform sampler2D implictInput;
in vec2 texCrood;
out vec4 outputColor;
void main() {
    outputColor = texture(implictInput, texCrood);
}
";

        private class EfxBlitType : OutEfxType
        {
            private EfxBlitType() : base(Vertex, Fragment) => ShaderProgram.Uniform(1, 0);

            public static EfxBlitType Instance()
            {
                if (NeedNewInstance())
                    return _instance = CrossContextResource.Inject(new EfxBlitType());
                return _instance;
            }

            private static bool NeedNewInstance()
            {
                if (_instance == null) return true;
                return !_instance.Valid();
            }
            
            private static EfxBlitType _instance;
        }

        public EfxBlit() : base(EfxBlitType.Instance())
        {
        }
    }
}