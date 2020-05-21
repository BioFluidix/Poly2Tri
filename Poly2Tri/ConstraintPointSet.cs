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

using System.Collections.Generic;

namespace Poly2Tri
{
    /// <summary>
    /// Exteet by adding some Constraints on how it will be triangulated.
    /// A constraint defines an edge between two points in the set, these edges can not
    /// be crossed. They will be enforced triangle edges after a triangulation.
    /// </summary>
    /// 
    /// author: Thomas Åhlén, thahlen@gmail.com
    /// 
    public class ConstrainedPointSet : PointSet
    {
        int[] _index;
        List<TriangulationPoint> _constrainedPointList = null;

        public ConstrainedPointSet(List<TriangulationPoint> points, int[] index) : base(points)
        {
            _index = index;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// 
        /// <param name="points">A list of all points in PointSet</param>
        /// <param name="constraints">Pairs of two points defining a constraint, all points <b>must</b> be part of given PointSet!</param>
        /// 
        public ConstrainedPointSet(List<TriangulationPoint> points, List<TriangulationPoint> constraints) : base(points)
        {

            _constrainedPointList = new List<TriangulationPoint>();
            _constrainedPointList.AddRange(constraints);
        }

        public override TriangulationMode getTriangulationMode()
        {
            return TriangulationMode.CONSTRAINED;
        }

        public int[] getEdgeIndex()
        {
            return _index;
        }


        public override void prepareTriangulation(TriangulationContext tcx)
        {
            base.prepareTriangulation(tcx);
            if (_constrainedPointList != null)
            {
                TriangulationPoint p1, p2;
                for (int i = 0; i < _constrainedPointList.Count; i += 2)
                {
                    p1 = _constrainedPointList[i];
                    p2 = _constrainedPointList[i + 1];
                    tcx.newConstraint(p1, p2);
                }

                //    Iterator iterator = _constrainedPointList.iterator();
                //while (iterator.hasNext())
                //{
                //    p1 = (TriangulationPoint)iterator.next();
                //    p2 = (TriangulationPoint)iterator.next();
                //    tcx.newConstraint(p1, p2);
                //}
            }
            else
            {
                for (int i = 0; i < _index.Length; i += 2)
                {
                    // XXX: must change!!
                    tcx.newConstraint(_points[_index[i]], _points[_index[i + 1]]);
                }
            }
        }

        /// <summary>
        /// Peforms a validation on given input
        /// TODO: TO BE IMPLEMENTED!
        /// <list type="number">
        /// <item>Check's if there any constraint edges are crossing or collinear</item>
        /// <item></item>
        /// </list>
        /// </summary>
        public bool isValid()
        {
            return true;
        }
    }
}
