// Core: Mat4.cs
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
    public struct Mat4F
    {
        private const float Pi = 3.1415926535897932f;

        public float[] Data;

        public Mat4F(float x)
        {
            Data = new[]
            {
                x, 0.0f, 0.0f, 0.0f,
                0.0f, x, 0.0f, 0.0f,
                0.0f, 0.0f, x, 0.0f,
                0.0f, 0.0f, 0.0f, x
            };
        }

        public static Mat4F operator +(Mat4F lhs, Mat4F rhs)
        {
            var result = lhs;
            for (var i = 0; i < 16; ++i)
            {
                result.Data[i] += rhs.Data[i];
            }

            return result;
        }

        public static Mat4F operator *(Mat4F lhs, Mat4F rhs)
        {
            var res = new Mat4F(0.0f);
            res.Data[0] = lhs.Data[0] * rhs.Data[0] + lhs.Data[1] * rhs.Data[4] + lhs.Data[2] * rhs.Data[8] +
                          lhs.Data[3] * rhs.Data[12];
            res.Data[1] = lhs.Data[0] * rhs.Data[1] + lhs.Data[1] * rhs.Data[5] + lhs.Data[2] * rhs.Data[9] +
                          lhs.Data[3] * rhs.Data[13];
            res.Data[2] = lhs.Data[0] * rhs.Data[2] + lhs.Data[1] * rhs.Data[6] + lhs.Data[2] * rhs.Data[10] +
                          lhs.Data[3] * rhs.Data[14];
            res.Data[3] = lhs.Data[0] * rhs.Data[3] + lhs.Data[1] * rhs.Data[7] + lhs.Data[2] * rhs.Data[11] +
                          lhs.Data[3] * rhs.Data[15];
            res.Data[4] = lhs.Data[4] * rhs.Data[0] + lhs.Data[5] * rhs.Data[4] + lhs.Data[6] * rhs.Data[8] +
                          lhs.Data[7] * rhs.Data[12];
            res.Data[5] = lhs.Data[4] * rhs.Data[1] + lhs.Data[5] * rhs.Data[5] + lhs.Data[6] * rhs.Data[9] +
                          lhs.Data[7] * rhs.Data[13];
            res.Data[6] = lhs.Data[4] * rhs.Data[2] + lhs.Data[5] * rhs.Data[6] + lhs.Data[6] * rhs.Data[10] +
                          lhs.Data[7] * rhs.Data[14];
            res.Data[7] = lhs.Data[4] * rhs.Data[3] + lhs.Data[5] * rhs.Data[7] + lhs.Data[6] * rhs.Data[11] +
                          lhs.Data[7] * rhs.Data[15];
            res.Data[8] = lhs.Data[8] * rhs.Data[0] + lhs.Data[9] * rhs.Data[4] + lhs.Data[10] * rhs.Data[8] +
                          lhs.Data[11] * rhs.Data[12];
            res.Data[9] = lhs.Data[8] * rhs.Data[1] + lhs.Data[9] * rhs.Data[5] + lhs.Data[10] * rhs.Data[9] +
                          lhs.Data[11] * rhs.Data[13];
            res.Data[10] = lhs.Data[8] * rhs.Data[2] + lhs.Data[9] * rhs.Data[6] + lhs.Data[10] * rhs.Data[10] +
                           lhs.Data[11] * rhs.Data[14];
            res.Data[11] = lhs.Data[8] * rhs.Data[3] + lhs.Data[9] * rhs.Data[7] + lhs.Data[10] * rhs.Data[11] +
                           lhs.Data[11] * rhs.Data[15];
            res.Data[12] = lhs.Data[12] * rhs.Data[0] + lhs.Data[13] * rhs.Data[4] + lhs.Data[14] * rhs.Data[8] +
                           lhs.Data[15] * rhs.Data[12];
            res.Data[13] = lhs.Data[12] * rhs.Data[1] + lhs.Data[13] * rhs.Data[5] + lhs.Data[14] * rhs.Data[9] +
                           lhs.Data[15] * rhs.Data[13];
            res.Data[14] = lhs.Data[12] * rhs.Data[2] + lhs.Data[13] * rhs.Data[6] + lhs.Data[14] * rhs.Data[10] +
                           lhs.Data[15] * rhs.Data[14];
            res.Data[15] = lhs.Data[12] * rhs.Data[3] + lhs.Data[13] * rhs.Data[7] + lhs.Data[14] * rhs.Data[11] +
                           lhs.Data[15] * rhs.Data[15];
            return res;
        }

        // Swap row r1, row r2
        public void SwapRows(uint r1, uint r2)
        {
            Generic.Swap(ref Data[r1 * 4 + 0], ref Data[r2 * 4 + 0]);
            Generic.Swap(ref Data[r1 * 4 + 1], ref Data[r2 * 4 + 1]);
            Generic.Swap(ref Data[r1 * 4 + 2], ref Data[r2 * 4 + 2]);
            Generic.Swap(ref Data[r1 * 4 + 3], ref Data[r2 * 4 + 3]);
        }

        // Row r *= k
        public void MultRow(uint r, float k)
        {
            Data[r * 4 + 0] *= k;
            Data[r * 4 + 1] *= k;
            Data[r * 4 + 2] *= k;
            Data[r * 4 + 3] *= k;
        }

        // Row dst += row src * k
        public void MultAndAdd(uint src, uint dst, float k)
        {
            Data[dst * 4 + 0] += Data[src * 4 + 0] * k;
            Data[dst * 4 + 1] += Data[src * 4 + 1] * k;
            Data[dst * 4 + 2] += Data[src * 4 + 2] * k;
            Data[dst * 4 + 3] += Data[src * 4 + 3] * k;
        }


        // Get transposed matrix
        public Mat4F Transposed()
        {
            return new Mat4F(0.0f)
            {
                Data =
                {
                    [0] = Data[0],
                    [1] = Data[4],
                    [2] = Data[8],
                    [3] = Data[12],
                    [4] = Data[1],
                    [5] = Data[5],
                    [6] = Data[9],
                    [7] = Data[13],
                    [8] = Data[2],
                    [9] = Data[6],
                    [10] = Data[10],
                    [11] = Data[14],
                    [12] = Data[3],
                    [13] = Data[7],
                    [14] = Data[11],
                    [15] = Data[15]
                }
            };
        }

        // Inverse matrix
        public Mat4F Inverse(float[] data)
        {
            Data = data;
            var res = Identity();
            for (uint i = 0; i < 4; i++)
            {
                var p = i;
                for (var j = i + 1; j < 4; j++)
                {
                    if (Math.Abs(Data[j * 4 + i]) > Math.Abs(Data[p * 4 + i])) p = j;
                }

                res.SwapRows(i, p);
                SwapRows(i, p);
                res.MultRow(i, 1.0f / Data[i * 4 + i]);
                MultRow(i, 1.0f / Data[i * 4 + i]);
                for (var j = i + 1; j < 4; j++)
                {
                    res.MultAndAdd(i, j, -Data[j * 4 + i]);
                    MultAndAdd(i, j, -Data[j * 4 + i]);
                }
            }

            for (var i = 3; i >= 0; i--)
            {
                for (uint j = 0; j < i; j++)
                {
                    res.MultAndAdd((uint) i, j, -Data[j * 4 + i]);
                    MultAndAdd((uint) i, j, -Data[j * 4 + i]);
                }
            }

            return this;
        }

        // Construct a translation matrix
        public static Mat4F Translation(Vec3<float> delta) => new Mat4F(1.0f)
        {
            Data =
            {
                [3] = delta.X,
                [7] = delta.Y,
                [11] = delta.Z
            }
        };

        // Construct a identity matrix
        public static Mat4F Identity() => new Mat4F(1.0f);

        // Construct a rotation matrix
        public static Mat4F Rotation(float degrees, Vec3<float> vec)
        {
            vec.Normalize();
            var alpha = degrees * Pi / 180.0f;
            var s = (float) Math.Sin(alpha);
            var c = (float) Math.Cos(alpha);
            var t = 1.0f - c;
            return new Mat4F(0.0f)
            {
                Data =
                {
                    [0] = t * vec.X * vec.X + c,
                    [1] = t * vec.X * vec.Y - s * vec.Z,
                    [2] = t * vec.X * vec.Z + s * vec.Y,
                    [4] = t * vec.X * vec.Y + s * vec.Z,
                    [5] = t * vec.Y * vec.Y + c,
                    [6] = t * vec.Y * vec.Z - s * vec.X,
                    [8] = t * vec.X * vec.Z - s * vec.Y,
                    [9] = t * vec.Y * vec.Z + s * vec.X,
                    [10] = t * vec.Z * vec.Z + c,
                    [15] = 1.0f
                }
            };
        }

        // Construct a perspective projection matrix
        public static Mat4F Perspective(float fov, float aspect, float zNear, float zFar)
        {
            var f = 1.0f / Math.Tan(fov * Pi / 180.0 / 2.0);
            var a = zNear - zFar;
            return new Mat4F(0.0f)
            {
                Data =
                {
                    [0] = (float) (f / aspect),
                    [5] = (float) f,
                    [10] = (zFar + zNear) / a,
                    [11] = 2.0f * zFar * zNear / a,
                    [14] = -1.0f
                }
            };
        }

        // Construct an orthogonal projection matrix
        public static Mat4F Ortho(float left, float right, float top, float bottom, float zNear, float zFar)
        {
            var a = right - left;
            var b = top - bottom;
            var c = zFar - zNear;
            return new Mat4F(0.0f)
            {
                Data =
                {
                    [0] = 2.0f / a,
                    [3] = -(right + left) / a,
                    [5] = 2.0f / b,
                    [7] = -(top + bottom) / b,
                    [10] = -2.0f / c,
                    [11] = -(zFar + zNear) / c,
                    [15] = 1.0f
                }
            };
        }
    }
}