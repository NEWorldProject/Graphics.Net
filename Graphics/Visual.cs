// Graphics: Visual.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using OpenGL;

namespace Graphics
{
    internal class Vrc : StrictDispose
    {
        public Vrc()
        {
            mMvp = new List<Mat4F> {Mat4F.Identity()};
            mVa = Inject(new VertexArray());
            mMatrix = Inject(new DataBuffer());
            mVa.EnableAttrib(0); // Position attribute
            mVa.AttribBinding(0, 0);
            mVa.AttribFormat(0, 2, Gl.Float, false, 0);
            mMatrix.Data(mMvp.Last().Data, Gl.StreamDraw);
        }

        public void UseVa() => mVa.Use();

        public VertexArray GetVa() => mVa;

        public Vrc PushMatrix(Mat4F matrix)
        {
            mMvp.Add(mMvp.Last() * matrix);
            return this;
        }

        public void PopMatrix() => mMvp.RemoveAt(mMvp.Count - 1);

        public void FlushMatrix() => mMatrix.DataSection(0, mMvp.Last().Data);

        public void Begin() => mMatrix.BindBase(Gl.UniformBuffer, 0);

        private static ThreadLocal<Vrc> _gVisualRenderContext = new ThreadLocal<Vrc>();

        public static void SetVrcHandle(Vrc hInstance) => _gVisualRenderContext.Value = hInstance;

        public static void ReleaseVrc() => _gVisualRenderContext.Value = null;

        public static Vrc Get() => _gVisualRenderContext.Value;

        private List<Mat4F> mMvp;
        private VertexArray mVa;
        private DataBuffer mMatrix;
    }

    public class Visual : StrictDispose
    {
        public OutEfx OutputEffect { get; set; }

        public Mat4F Transform { get; set; }

        public Visual() => Transform = Mat4F.Identity();

        public virtual void RenderThis() => RenderContent();

        protected virtual void RenderContent()
        {
        }
    }

    public class Triangle : Visual
    {
        public Brush BorderBrush { get; set; }
        public Brush FillBrush { get; set; }
        public int BorderWidth { get; set; }

        protected override void RenderContent()
        {
            var ctx = Vrc.Get();
            ctx.PushMatrix(Transform).FlushMatrix();
            ctx.GetVa().BindBuffer(0, mShape, 0, 2 * sizeof(float));
            ctx.UseVa();
            if (FillBrush != null)
            {
                FillBrush.Use();
                Gl.DrawArrays(Gl.Triangles, 0, 3);
            }

            if (BorderBrush != null && BorderWidth > 0)
            {
                Gl.LineWidth(BorderWidth);
                BorderBrush.Use();
                Gl.DrawArrays(Gl.LineLoop, 0, 3);
            }

            ctx.PopMatrix();
        }

        private DataBuffer mShape;

        public void SetShape(float[] shape) => mShape.Data(shape, Gl.StaticDraw);

        public Triangle() => mShape = Inject(new DataBuffer());
    }

    public class VisualCollectionNode : Visual
    {
        public List<Visual> Children { get; set; }

        protected override void RenderContent()
        {
            Vrc.Get().PushMatrix(Transform);
            foreach (var x in Children)
                x.RenderThis();
            Vrc.Get().PopMatrix();
        }
    }

    public class VisualRoot : Visual
    {
        public Visual Content { get; set; }

        public VisualRoot()
        {
            _vrc = Inject(new Vrc());
        }

        public void Render()
        {
            RenderThis();
        }

        public override void RenderThis()
        {
            Vrc.SetVrcHandle(_vrc);
            _vrc.Begin();
            base.RenderThis();
            Vrc.ReleaseVrc();
        }

        protected override void RenderContent()
        {
            _vrc.PushMatrix(Transform);
            Content.RenderThis();
            _vrc.PopMatrix();
        }

        private Vrc _vrc;
    }
}