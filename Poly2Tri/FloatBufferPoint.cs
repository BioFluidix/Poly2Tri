/* Poly2Tri
 * Copyright (c) 2009-2010, Poly2Tri Contributors
 * http://code.google.com/p/poly2tri/
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice,
 *   this list of conditions and the following disclaimer.
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 * * Neither the name of Poly2Tri nor the names of its contributors may be
 *   used to endorse or promote products derived from this software without specific
 *   prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
 * A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


namespace Poly2Tri
{
    public class FloatBufferPoint : TriangulationPoint
    {
    private readonly float[] _fb;
    private readonly int _ix, _iy, _iz;

    public FloatBufferPoint(float[] fb, int index)
    {
        _fb = fb;
        _ix = index;
        _iy = index + 1;
        _iz = index + 2;
    }

    public sealed override double getX()
    {
        return _fb[_ix];
    }
    public sealed override double getY()
    {
        return _fb[_iy];
    }
    public sealed override double getZ()
    {
        return _fb[_iz];
    }

    public sealed override float getXf()
    {
        return _fb[_ix];
    }
    public sealed override float getYf()
    {
        return _fb[_iy];
    }
    public sealed override float getZf()
    {
        return _fb[_iz];
    }

    
    public override void set(double x, double y, double z)
    {
        _fb[_ix] = (float)x;
        _fb[_iy] = (float)y;
        _fb[_iz] = (float)z;
    }

    public static TriangulationPoint[] toPoints(float[] fb)
    {
        FloatBufferPoint[] points = new FloatBufferPoint[fb.Length / 3];
        for (int i = 0, j = 0; i < points.Length; i++, j += 3)
        {
            points[i] = new FloatBufferPoint(fb, j);
        }
        return points;
    }
}
}
