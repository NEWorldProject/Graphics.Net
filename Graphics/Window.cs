// Graphics: Window.cs
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
using Core;
using SDL2;

namespace Graphics
{
    public class Window : StrictDispose
    {
        public enum PositionFlag
        {
            PositionCentered = 0x2fff0000,
            PositionDoNotCare = 0x1fff0000
        }

        public Window(string title, Vec2<int> initPos, Vec2<int> initSize)
        {
            var window = SDL.SDL_CreateWindow(title, initPos.X, initPos.Y, initSize.X, initSize.Y,
                SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);
            if (window == null)
                throw new Exception("Window could not be created! SDL_Error:" + SDL.SDL_GetError());
            mHdc = window;
        }

        public void Show() => SDL.SDL_ShowWindow(mHdc);

        public void Hide() => SDL.SDL_HideWindow(mHdc);

        public void Raise() => SDL.SDL_RaiseWindow(mHdc);

        public IntPtr Raw() => mHdc;

        private IntPtr mHdc;
    }
}