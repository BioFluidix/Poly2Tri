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

using System;
using System.Collections.Generic;

namespace Poly2Tri
{
    public abstract class TriangulationPoint : Point, IEquatable<TriangulationPoint>
    {
        /// <summary>
        /// List of edges this point constitutes an upper ending point (CDT)
        /// </summary>
        private List<DTSweepConstraint> edges;

        public override string ToString()
            => $"[{getX()}, {getY()}]";

        public List<DTSweepConstraint> getEdges()
        {
            return edges;
        }

        public void addEdge(DTSweepConstraint e)
        {
            if (edges == null)
            {
                edges = new List<DTSweepConstraint>();
            }
            edges.Add(e);
        }

        public bool hasEdges()
        {
            return edges != null;
        }

        /// <summary>
        /// getEdge
        /// </summary>
        /// <param name="p">edge destination point</param>
        /// <returns>the edge from this point to given point</returns>
        public DTSweepConstraint getEdge(TriangulationPoint p)
        {
            foreach (DTSweepConstraint c in edges)
            {
                if (c.getP() == p)
                {
                    return c;
                }
            }
            return null;
        }

        public override int GetHashCode()
        {
            long bits = BitConverter.DoubleToInt64Bits(getX());
            bits ^= BitConverter.DoubleToInt64Bits(getY()) * 31;
            return (((int)bits) ^ ((int)(bits >> 32)));
        }

        public bool Equals(TriangulationPoint other)
        {
            if (other == null) return false;
            return getX() == other.getX() && getY() == other.getY();
        }

        public override bool Equals(object obj)
        {
            TriangulationPoint tp = obj as TriangulationPoint;
            if (tp != null)
            {
                return Equals(tp);
            }
            else
            {
                return false;
            }
        }
        public static bool operator ==(TriangulationPoint tp0, TriangulationPoint tp1)
        {
            if (ReferenceEquals(tp0, tp1)) return true;
            if (ReferenceEquals(tp0, null)) return false;
            if (ReferenceEquals(tp1, null)) return false;

            return tp0.Equals(tp1);
        }

        public static bool operator !=(TriangulationPoint tp0, TriangulationPoint tp1)
        {
            if (ReferenceEquals(tp0, tp1)) return false;
            if (ReferenceEquals(tp0, null)) return true;
            if (ReferenceEquals(tp1, null)) return true;

            return !tp0.Equals(tp1);
        }



    }
}
