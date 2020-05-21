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
    /// TriangulationUtil
    /// </summary>
    /// 
    /// author: Thomas Åhlén, thahlen@gmail.com
    /// 
    public class TriangulationUtil
    {
        public const double EPSILON = 1e-12;

        // Returns triangle circumcircle point and radius
        //    public static Tuple2<TPoint, Double> circumCircle( TPoint a, TPoint b, TPoint c )
        //    {
        //        double A = det( a, b, c );
        //        double C = detC( a, b, c );
        //
        //        double sa = a.getX() * a.getX() + a.getY() * a.getY();
        //        double sb = b.getX() * b.getX() + b.getY() * b.getY();
        //        double sc = c.getX() * c.getX() + c.getY() * c.getY();
        //
        //        TPoint bx1 = new TPoint( sa, a.getY() );
        //        TPoint bx2 = new TPoint( sb, b.getY() );
        //        TPoint bx3 = new TPoint( sc, c.getY() );
        //        double bx = det( bx1, bx2, bx3 );
        //
        //        TPoint by1 = new TPoint( sa, a.getX() );
        //        TPoint by2 = new TPoint( sb, b.getX() );
        //        TPoint by3 = new TPoint( sc, c.getX() );
        //        double by = det( by1, by2, by3 );
        //
        //        double x = bx / ( 2 * A );
        //        double y = by / ( 2 * A );
        //
        //        TPoint center = new TPoint( x, y );
        //        double radius = Math.sqrt( bx * bx + by * by - 4 * A * C ) / ( 2 * Math.abs( A ) );
        //
        //        return new Tuple2<TPoint, Double>( center, radius );
        //    }

        /// <summary>
        /// smartIncircle
        /// Requirement:
        /// <list type="number">
        /// <item>a,b and c form a triangle.</item>
        /// <item>a and d is know to be on opposite side of bc</item>
        /// </list>
        /// <code>
        ///                a
        ///                +
        ///               / \
        ///              /   \
        ///            b/     \c
        ///            +-------+ 
        ///           /    B    \  
        ///          /           \ 
        /// </code>
        /// <b>Fact</b>: d has to be in area B to have a chance to be inside the circle formed by
        ///  a,b and c<br>
        ///  d is outside B if orient2d(a,b,d) or orient2d(c,a,d) is CW<br>
        ///  This preknowledge gives us a way to optimize the incircle test
        /// </summary>
        /// <param name="a">triangle point, opposite d</param>
        /// <param name="b">triangle point</param>
        /// <param name="c">triangle point</param>
        /// <param name="d">point opposite a</param> 
        /// <return>true if d is inside circle, false if on circle edge</return>
        /// 
        public static bool smartIncircle(TriangulationPoint pa,
                                             TriangulationPoint pb,
                                             TriangulationPoint pc,
                                             TriangulationPoint pd )
        {
            double pdx = pd.getX();
            double pdy = pd.getY();
            double adx = pa.getX() - pdx;
            double ady = pa.getY() - pdy;
            double bdx = pb.getX() - pdx;
            double bdy = pb.getY() - pdy;

            double adxbdy = adx * bdy;
            double bdxady = bdx * ady;
            double oabd = adxbdy - bdxady;
            //        oabd = orient2d(pa,pb,pd);
            if (oabd <= 0)
            {
                return false;
            }

            double cdx = pc.getX() - pdx;
            double cdy = pc.getY() - pdy;

            double cdxady = cdx * ady;
            double adxcdy = adx * cdy;
            double ocad = cdxady - adxcdy;
            //      ocad = orient2d(pc,pa,pd);
            if (ocad <= 0)
            {
                return false;
            }

            double bdxcdy = bdx * cdy;
            double cdxbdy = cdx * bdy;

            double alift = adx * adx + ady * ady;
            double blift = bdx * bdx + bdy * bdy;
            double clift = cdx * cdx + cdy * cdy;

            double det = alift * (bdxcdy - cdxbdy) + blift * ocad + clift * oabd;

            return det > 0;
        }

        /// <summary>
        /// inScanArea
        /// </summary>
        /// <see cref="smartIncircle"/>
        /// 
        public static bool inScanArea(TriangulationPoint pa,
                                          TriangulationPoint pb,
                                          TriangulationPoint pc,
                                          TriangulationPoint pd )
        {
            double pdx = pd.getX();
            double pdy = pd.getY();
            double adx = pa.getX() - pdx;
            double ady = pa.getY() - pdy;
            double bdx = pb.getX() - pdx;
            double bdy = pb.getY() - pdy;

            double adxbdy = adx * bdy;
            double bdxady = bdx * ady;
            double oabd = adxbdy - bdxady;
            //        oabd = orient2d(pa,pb,pd);
            if (oabd <= 0)
            {
                return false;
            }

            double cdx = pc.getX() - pdx;
            double cdy = pc.getY() - pdy;

            double cdxady = cdx * ady;
            double adxcdy = adx * cdy;
            double ocad = cdxady - adxcdy;
            //      ocad = orient2d(pc,pa,pd);
            if (ocad <= 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Forumla to calculate signed area
        /// <list type="bullet">
        /// <item>Positive if CCW</item>
        /// <item>Negative if CW</item>
        /// <item>0 if collinear</item>
        /// </list>
        /// <code>
        /// A[P1,P2,P3]  =  (x1*y2 - y1*x2) + (x2*y3 - y2*x3) + (x3*y1 - y3*x1)
        ///              =  (x1-x3)*(y2-y3) - (y1-y3)*(x2-x3)
        /// </code>             
        /// </summary>
        public static Orientation orient2d(TriangulationPoint pa,
                                            TriangulationPoint pb,
                                            TriangulationPoint pc)
        {
            double detleft = (pa.getX() - pc.getX()) * (pb.getY() - pc.getY());
            double detright = (pa.getY() - pc.getY()) * (pb.getX() - pc.getX());
            double val = detleft - detright;
            if (val > -EPSILON && val < EPSILON)
            {
                return Orientation.Collinear;
            }
            else if (val > 0)
            {
                return Orientation.CCW;
            }
            return Orientation.CW;
        }

        public enum Orientation
        {
            CW, CCW, Collinear
        }
    }
}
