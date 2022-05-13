/*
    MIT License

    Copyright (c) [2017] jgallant.com 

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
 */

using System;

namespace Resources.Code.Test {
    public static class MathHelper {
        public static double Clamp(double v, double l, double h) {
            if (v < l) v = l;
            if (v > h) v = h;
            return v;
        }

        public static double Lerp(double t, double a, double b) {
            return a + t * (b - a);
        }

        public static double QuinticBlend(double t) {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static double Bias(double b, double t) {
            return Math.Pow(t, Math.Log(b) / Math.Log(0.5));
        }

        public static double Gain(double g, double t) {
            if (t < 0.5) {
                return Bias(1.0 - g, 2.0 * t) / 2.0;
            }
            else {
                return 1.0 - Bias(1.0 - g, 2.0 - 2.0 * t) / 2.0;
            }
        }

        public static int Mod(int x, int m) {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
