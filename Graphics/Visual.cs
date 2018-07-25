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
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Core;
using OpenGL;

namespace Graphics
{
    internal class VrcFrame : StrictDispose
    {
        public VrcFrame(Vec2<int> size)
        {
            _color = Inject(new Texture(1, PixelInternalFormats.Rgba8, size));
            _color.MagnificationFilter = _color.MinifyingFilter = Texture.Filter.Linear;
        }

        public void AttachCurrent(FrameBuffer frameBuffer)
        {
            frameBuffer.Texture(Gl.ColorAttachment0, _color, 0);
            Gl.Clear(Gl.ColorBufferBit);
        }

        public Texture Fetch() => _color;

        private readonly Texture _color;
    }

    internal class VrcFrames : StrictDispose
    {
        private class VrcFramesInternal : StrictDispose
        {
            public VrcFramesInternal(Vec2<int> size)
            {
                _size = size;
                _frames = new List<VrcFrame>();
            }

            public VrcFrame Request()
            {
                if (_top == _frames.Count)
                    _frames.Add(Inject(new VrcFrame(_size)));
                return _frames[_top++];
            }

            public VrcFrame Current() => _top > 0 ? _frames[_top - 1] : null;

            public void ReleaseLast() => --_top;

            private readonly Vec2<int> _size;

            private int _top;

            private List<VrcFrame> _frames;
        }

        public VrcFrames(Vec2<int> size)
        {
            _frames = Inject(new VrcFramesInternal(size));
            _backBuffer = Inject(new FrameBuffer());
            _stencil = Inject(new RenderBuffer());
            _stencil.SetStorage(PixelInternalFormats.StencilIndex, size);
            _backBuffer.RenderBuffer(Gl.StencilAttachment, _stencil);
        }

        public void Resize(Vec2<int> size)
        {
            Reject(_frames);
            _frames = Inject(new VrcFramesInternal(size));
            _stencil.SetStorage(PixelInternalFormats.StencilIndex, size);
        }

        public VrcFrame Request()
        {
            var frame = _frames.Request();
            frame.AttachCurrent(_backBuffer);
            return frame;
        }

        public void RestoreLast()
        {
            _frames.ReleaseLast();
            _frames.Current() ? .AttachCurrent(_backBuffer);
        }

        public void UseBackBuffer() => _backBuffer.Use();

        private FrameBuffer _backBuffer;

        private RenderBuffer _stencil;

        private VrcFramesInternal _frames;
    }

    internal class Vrc : StrictDispose
    {
        public Vrc()
        {
            Va = Inject(new VertexArray());
            Frames = Inject(new VrcFrames(new Vec2<int>(600, 600)));
            _mvp = new List<Mat4F> {Mat4F.Identity()};
            _matrix = Inject(new DataBuffer());
            CommonBuffer = Inject(new DataBuffer());
            Va.EnableAttrib(0); // Position attribute
            Va.AttribBinding(0, 0);
            Va.AttribFormat(0, 2, Gl.Float, false, 0);
            _matrix.Data(_mvp.Last().Data, Gl.StreamDraw);
        }

        public VertexArray Va;

        public VrcFrames Frames;

        public DataBuffer CommonBuffer;

        public void PushMatrix(Mat4F matrix) => _mvp.Add(_mvp.Last() * matrix);

        public void PopMatrix() => _mvp.RemoveAt(_mvp.Count - 1);

        public void FlushMatrix() => _matrix.DataSection(0, _mvp.Last().Data);

        public void BindMatrix() => _matrix.BindBase(Gl.UniformBuffer, 0);

        private static ThreadLocal<Vrc> _gVisualRenderContext = new ThreadLocal<Vrc>();

        public static void SetVrcHandle(Vrc hInstance) => _gVisualRenderContext.Value = hInstance;

        public static void ReleaseVrc() => _gVisualRenderContext.Value = null;

        public static Vrc Get() => _gVisualRenderContext.Value;

        private List<Mat4F> _mvp;
        private DataBuffer _matrix;
    }

    public class Visual : StrictDispose
    {
        public OutEfx OutputEffect;

        public Mat4F Transform;
        private Rect<float> _rect;

        public Rect<float> Rect
        {
            get => _rect;
            set
            {
                Parent?.UpdateRect();
                _rect = value;
            }
        }

        public Visual() => Transform = Mat4F.Identity();

        public virtual void RenderThis()
        {
            if (OutputEffect != null)
            {
                var frames = Vrc.Get().Frames;
                var frame = frames.Request();
                RenderContent();
                frames.RestoreLast();
                BlitResult(OutputEffect, frame.Fetch());
            }
            else
            {
                RenderContent();
            }
        }

        protected void BlitResult(OutEfx outputEffect, Texture fetch)
        {
            float[] rectData = {
                Rect.Min.X, Rect.Min.Y, Rect.Max.X, Rect.Min.Y, Rect.Min.X, Rect.Max.Y,
                Rect.Max.X, Rect.Min.Y, Rect.Max.X, Rect.Max.Y, Rect.Min.X, Rect.Max.Y
            };
            var ctx = Vrc.Get();
            fetch.Use(0);
            ctx.CommonBuffer.Data(rectData, Gl.StreamDraw);
            ctx.Va.BindBuffer(0, ctx.CommonBuffer, 0, 2 * sizeof(float));
            outputEffect.Use();
            Gl.DrawArrays(Gl.Triangles, 0, 6);
        }

        protected virtual void RenderContent()
        {
        }

        protected virtual void UpdateRect()
        {
        }

        public Visual Parent { get; set; }
    }

    public class Triangle : Visual
    {
        public Brush BorderBrush;
        public Brush FillBrush;
        public int BorderWidth;

        protected override void RenderContent()
        {
            var ctx = Vrc.Get();
            ctx.PushMatrix(Transform);
            ctx.FlushMatrix();
            ctx.Va.BindBuffer(0, mShape, 0, 2 * sizeof(float));
            ctx.Va.Use();
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

        public void SetShape(float[] shape)
        {
            Rect = new Rect<float>(shape);
            mShape.Data(shape, Gl.StaticDraw);
        }

        public Triangle()
        {
            mShape = Inject(new DataBuffer());
        }
    }

    public class VisualCollectionNode : Visual
    {
        public List<Visual> Children;

        public VisualCollectionNode()
        {
            Children = new List<Visual>();
        }

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
        public Visual Content
        {
            get => _content;
            set
            {
                _content = value;
                _content.Parent = this;
                UpdateRect();
            }
        }

        public VisualRoot()
        {
            _vrc = Inject(new Vrc());
            OutputEffect = Inject(new EfxColorTune(new Rgba<float>(1.0f, 1.0f, 0.0f, 1.0f)));
        }

        public void Render()
        {
            Vrc.SetVrcHandle(_vrc);
            _vrc.BindMatrix();
            using (var frames = new VrcFrames(new Vec2<int>(600, 600)))
            {
                frames.UseBackBuffer();
                var frame = frames.Request();
                RenderThis();
                frames.RestoreLast();
                FrameBuffer.UseDefault();
                BlitResult(OutputEffect, frame.Fetch());
            }
            Vrc.ReleaseVrc();
        }
        
        public override void RenderThis() => RenderContent();

        protected override void RenderContent()
        {
            if (Content != null)
            {
                _vrc.PushMatrix(Transform);
                Content.RenderThis();
                _vrc.PopMatrix();
            }
        }

        protected override void UpdateRect() => Rect = Content.Rect;

        private Vrc _vrc;
        private Visual _content;
    }
}