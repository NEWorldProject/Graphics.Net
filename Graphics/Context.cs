// Graphics: Context.cs
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
using System.Threading;
using Core;
using OpenGL;
using SDL2;

namespace Graphics
{
    public static class CrossContextResource
    {
        private class Backing : StrictDispose
        {
            public T InjectItem<T>(T target) where T : StrictDispose => Inject(target);

            public static Backing Instance() => _instance ?? (_instance = new Backing());

            private static Backing _instance;
        }

        public static T Inject<T>(T target) where T : StrictDispose => Backing.Instance().InjectItem(target);

        internal static void Reference() => ++_refCount;

        internal static void Dereference()
        {
            if (--_refCount == 0)
                Backing.Instance().Dispose();
        }

        private static int _refCount;
    }

    public class Context : StrictDispose
    {
        public delegate void Renderer();

        public Context(Window window)
        {
            var ctx = SDL.SDL_GL_CreateContext(window.Raw());
            if (ctx == null)
                throw new Exception("OpenGL context could not be created! SDL Error: " + SDL.SDL_GetError());
            _hdc = ctx;
            _host = window;
            SDL.SDL_GL_SetSwapInterval(1);
            if (_isFirstContext)
            {
                Gl.Init(SDL.SDL_GL_GetProcAddress);
                _isFirstContext = false;
            }

            CrossContextResource.Reference();
        }

        protected override void Release()
        {
            if (_renderFlag)
                CloseRenderThread();
            CrossContextResource.Dereference();
            SDL.SDL_GL_DeleteContext(_hdc);
        }

        public void MakeCurrent()
        {
            SDL.SDL_GL_MakeCurrent(_host.Raw(), _hdc);
        }

        public void StartRenderThread(Renderer renderFunction)
        {
            UnBindCurrent();
            if (_renderFlag)
                throw new Exception("Render thread already running");
            _renderFlag = true;
            _renderThread = new Thread(() =>
            {
                SDL.SDL_GL_MakeCurrent(_host.Raw(), _hdc);
                while (_renderFlag)
                {
                    renderFunction();
                    SDL.SDL_GL_SwapWindow(_host.Raw());
                }
            });
            _renderThread.Start();
        }

        public void CloseRenderThread()
        {
            _renderFlag = false;
            _renderThread.Join();
        }

        public void UnBindCurrent()
        {
            if (SDL.SDL_GL_GetCurrentContext() == _hdc)
                SDL.SDL_GL_MakeCurrent(_host.Raw(), IntPtr.Zero);
        }

        private IntPtr _hdc;

        private Window _host;

        private bool _renderFlag;

        private Thread _renderThread;

        private static bool _isFirstContext = true;
    }
}