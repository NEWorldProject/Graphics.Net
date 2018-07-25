// Graphics: Application.cs
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
    public class Application : StrictDispose
    {
        protected Application()
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) < 0)
                throw new Exception("SDL could not initialize! SDL_Error: " + SDL.SDL_GetError());
            //Use OpenGL 4.5 core
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 4);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 5);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
            SDL.SDL_GL_SetAttribute(SDL.SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK,
                (int) SDL.SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
        }

        protected override void Release() => SDL.SDL_Quit();

        public void Run()
        {
            while (!_appShouldExit)
                WaitEvents();
        }

        public void PullEvents()
        {
            while (SDL.SDL_PollEvent(out var sdlEvent) != 0)
                HandleEvent(ref sdlEvent);
        }

        public void WaitEvents()
        {
            SDL.SDL_WaitEvent(out var sdlEvent);
            HandleEvent(ref sdlEvent);
            PullEvents();
        }

        public int HasQueuingEvents()
        {
            SDL.SDL_Event[] events = { };
            return SDL.SDL_PeepEvents(events, 1, SDL.SDL_eventaction.SDL_PEEKEVENT,
                SDL.SDL_EventType.SDL_FIRSTEVENT, SDL.SDL_EventType.SDL_LASTEVENT);
        }

        private bool _appShouldExit;

        private void HandleEvent(ref SDL.SDL_Event ev)
        {
            switch (ev.type)
            {
                case SDL.SDL_EventType.SDL_QUIT:
                    _appShouldExit = true;
                    break;
                case SDL.SDL_EventType.SDL_FIRSTEVENT:
                    break;
                case SDL.SDL_EventType.SDL_WINDOWEVENT:
                    break;
                case SDL.SDL_EventType.SDL_SYSWMEVENT:
                    break;
                case SDL.SDL_EventType.SDL_KEYDOWN:
                    break;
                case SDL.SDL_EventType.SDL_KEYUP:
                    break;
                case SDL.SDL_EventType.SDL_TEXTEDITING:
                    break;
                case SDL.SDL_EventType.SDL_TEXTINPUT:
                    break;
                case SDL.SDL_EventType.SDL_MOUSEMOTION:
                    break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    break;
                case SDL.SDL_EventType.SDL_MOUSEBUTTONUP:
                    break;
                case SDL.SDL_EventType.SDL_MOUSEWHEEL:
                    break;
                case SDL.SDL_EventType.SDL_JOYAXISMOTION:
                    break;
                case SDL.SDL_EventType.SDL_JOYBALLMOTION:
                    break;
                case SDL.SDL_EventType.SDL_JOYHATMOTION:
                    break;
                case SDL.SDL_EventType.SDL_JOYBUTTONDOWN:
                    break;
                case SDL.SDL_EventType.SDL_JOYBUTTONUP:
                    break;
                case SDL.SDL_EventType.SDL_JOYDEVICEADDED:
                    break;
                case SDL.SDL_EventType.SDL_JOYDEVICEREMOVED:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERAXISMOTION:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERBUTTONDOWN:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERBUTTONUP:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERDEVICEADDED:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMOVED:
                    break;
                case SDL.SDL_EventType.SDL_CONTROLLERDEVICEREMAPPED:
                    break;
                case SDL.SDL_EventType.SDL_FINGERDOWN:
                    break;
                case SDL.SDL_EventType.SDL_FINGERUP:
                    break;
                case SDL.SDL_EventType.SDL_FINGERMOTION:
                    break;
                case SDL.SDL_EventType.SDL_DOLLARGESTURE:
                    break;
                case SDL.SDL_EventType.SDL_DOLLARRECORD:
                    break;
                case SDL.SDL_EventType.SDL_MULTIGESTURE:
                    break;
                case SDL.SDL_EventType.SDL_CLIPBOARDUPDATE:
                    break;
                case SDL.SDL_EventType.SDL_DROPFILE:
                    break;
                case SDL.SDL_EventType.SDL_DROPTEXT:
                    break;
                case SDL.SDL_EventType.SDL_DROPBEGIN:
                    break;
                case SDL.SDL_EventType.SDL_DROPCOMPLETE:
                    break;
                case SDL.SDL_EventType.SDL_AUDIODEVICEADDED:
                    break;
                case SDL.SDL_EventType.SDL_AUDIODEVICEREMOVED:
                    break;
                case SDL.SDL_EventType.SDL_RENDER_TARGETS_RESET:
                    break;
                case SDL.SDL_EventType.SDL_RENDER_DEVICE_RESET:
                    break;
                case SDL.SDL_EventType.SDL_USEREVENT:
                    break;
                case SDL.SDL_EventType.SDL_LASTEVENT:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}