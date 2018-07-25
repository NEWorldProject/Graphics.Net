// Core: StrictDispose.cs
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

namespace Core
{
    public class StrictDispose : IDisposable
    {
        ~StrictDispose() => Dispose();

        protected T Inject<T>(T target) where T : StrictDispose
        {
            if (_first != null)
                target._sibling = _first;
            _first = target;
            return target;
        }

        protected void Reject<T>(T target, bool disposeNow = true) where T : StrictDispose
        {
            if (target == _first)
            {
                _first = target._sibling;
                return;
            }
            
            var current = _first;
            while (current._sibling != target)
                current = current._sibling;
            current._sibling = target._sibling;
            
            if (disposeNow)
                target.Dispose();
        }

        protected virtual void Release()
        {
        }

        private void TravelRelease()
        {
            for (var current = _first; current != null; current = current._sibling)
                current.Dispose();
        }

        public void Dispose()
        {
            if (_released)
                return;
            TravelRelease();
            Release();
            _released = true;
        }

        public bool Valid() => !_released;

        private StrictDispose _first, _sibling;

        private bool _released;
    }
}