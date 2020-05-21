﻿/* Poly2Tri
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
    /// <summary>
    /// A transform that aligns the XY plane normal [0,0,1] with any given target normal
    /// http://www.cs.brown.edu/~jfh/papers/Moller-EBA-1999/paper.pdf
    /// </summary>
    /// 
    /// author: Thomas Åhlén, thahlen@gmail.com
    /// 
    public class XYToAnyTransform : Matrix3Transform
    {
        /// <summary>
        /// Assumes target normal is normalized
        /// </summary>
        public XYToAnyTransform(double nx, double ny, double nz)
        {
            setTargetNormal(nx, ny, nz);
        }

        /// <summary>
        /// Assumes target normal is normalized
        /// </summary>
        /// 
        public void setTargetNormal(double nx, double ny, double nz)
        {
            double h, f, c, vx, vy, hvx;

            vx = ny;
            vy = -nx;
            c = nz;

            h = (1 - c) / (1 - c * c);
            hvx = h * vx;
            f = (c < 0) ? -c : c;

            if (f < 1.0 - 1.0E-4)
            {
                m00 = c + hvx * vx;
                m01 = hvx * vy;
                m02 = -vy;
                m10 = hvx * vy;
                m11 = c + h * vy * vy;
                m12 = vx;
                m20 = vy;
                m21 = -vx;
                m22 = c;
            }
            else
            {
                // if "from" and "to" vectors are nearly parallel
                m00 = 1;
                m01 = 0;
                m02 = 0;
                m10 = 0;
                m11 = 1;
                m12 = 0;
                m20 = 0;
                m21 = 0;
                if (c > 0)
                {
                    m22 = 1;
                }
                else
                {
                    m22 = -1;
                }
            }

        }
    }
}
