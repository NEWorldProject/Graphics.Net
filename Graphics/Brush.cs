// Graphics: Brush.cs
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
    public class BrushType : StrictDispose
    {
        public BrushType(string vert, string frag)
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

    public class Brush : StrictDispose
    {
        public virtual void Use() => _type.Use();
        protected Brush(BrushType type) => _type = type;
        private BrushType _type;
    }

    public class SolidColorBrush : Brush
    {
        private const string Vertex = @"
#version 330 core
layout (location = 0) in vec2 position;
layout (std140) uniform vertexMvp {
    mat4 mvp;
};
void main() {
    gl_Position = mvp * vec4(position.x, position.y, 0.0, 1.0);
}
";

        private const string Fragment = @"
#version 330
layout (std140) uniform brushData {
    vec4 color;
};
out vec4 outputColor;
void main() {
    outputColor = color;
}
";

        private class SolidColorBrushType : BrushType
        {
            private SolidColorBrushType() : base(Vertex, Fragment)
            {
                ShaderProgram.BindUniformBlock("vertexMvp", 0);
                ShaderProgram.BindUniformBlock("brushData", 1);
            }

            public static SolidColorBrushType Instance()
            {
                if (NeedNewInstance())
                    return _instance = CrossContextResource.Inject(new SolidColorBrushType());
                return _instance;
            }

            private static bool NeedNewInstance()
            {
                if (_instance == null) return true;
                return !_instance.Valid();
            }

            private static SolidColorBrushType _instance;
        }

        public SolidColorBrush() : base(SolidColorBrushType.Instance())
        {
            _brushData = Inject(new DataBuffer());
            var zeroColor = new Rgba<float>(0.0f, 0.0f, 0.0f, 0.0f);
            _brushData.Data(zeroColor.Data, Gl.StaticDraw);
        }

        public SolidColorBrush(Rgba<float> color) : base(SolidColorBrushType.Instance())
        {
            _brushData = Inject(new DataBuffer());
            _brushData.Data(color.Data, Gl.StaticDraw);
        }

        public override void Use()
        {
            base.Use();
            _brushData.BindBase(Gl.UniformBuffer, 1);
        }

        public void SetColor(Rgba<float> color) => _brushData.DataSection(0, color.Data);
        private DataBuffer _brushData;
    }
}