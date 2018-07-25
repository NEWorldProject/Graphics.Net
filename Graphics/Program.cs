// Graphics: Program.cs
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

using System.Reflection.Emit;
using Core;
using OpenGL;

namespace Graphics
{
    internal class App : Application
    {
        private static readonly Vec2<int> Position =
            new Vec2<int>((int) Window.PositionFlag.PositionCentered, (int) Window.PositionFlag.PositionCentered);

        private static readonly Vec2<int> Size = new Vec2<int>(600, 600);

        public App()
        {
            var window = Inject(new Window("Hello World!", Position, Size));
            var ctx = Inject(new Context(window));
            _root = Inject(new VisualRoot());
            window.Show();
            Gl.Viewport(0, 0, Size.X, Size.Y);
            SetUpTriangle();
            ctx.StartRenderThread(Render);
        }

        private void SetUpTriangle()
        {
            float[] vertices =
            {
                0.5f, -0.5f, // Bottom Right
                -0.5f, -0.5f, // Bottom Left
                0.0f, 0.5f // Top
            };
            var triangle = Inject(new Triangle());
            triangle.SetShape(vertices);
            triangle.FillBrush = Inject(new SolidColorBrush(new Rgba<float>(1.0f, 1.0f, 1.0f, 1.0f)));
            triangle.BorderBrush = Inject(new SolidColorBrush(new Rgba<float>(0.0f, 0.0f, 0.0f, 1.0f)));
            triangle.BorderWidth = 4;
            _root.Content = triangle;
        }

        private void Render()
        {
            _transform *= Mat4F.Rotation(1, new Vec3<float>(0.0f, 0.0f, 1.0f));
            _root.Transform = _transform;
            Gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            Gl.Clear(Gl.ColorBufferBit);
            _root.Render();
        }

        private Mat4F _transform = Mat4F.Identity();
        private VisualRoot _root;
    }

    internal static class TestProgram
    {
        public static void Main(string[] args)
        {
            using (var app = new App())
            {
                app.Run();
            }
        }
    }
}