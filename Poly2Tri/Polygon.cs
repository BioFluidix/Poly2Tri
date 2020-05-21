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

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poly2Tri
{
    public class Polygon : ITriangulatable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected List<TriangulationPoint> _points = new List<TriangulationPoint>();
        protected List<TriangulationPoint> _steinerPoints;
        protected List<Polygon> _holes;

        protected List<DelaunayTriangle> m_triangles;

        protected PolygonPoint _last;

        /// <summary>
        /// To create a polygon we need atleast 3 separate points
        /// </summary>
        /// 
        public Polygon(PolygonPoint p1, PolygonPoint p2, PolygonPoint p3)
        {
            p1._next = p2;
            p2._next = p3;
            p3._next = p1;
            p1._previous = p3;
            p2._previous = p1;
            p3._previous = p2;
            _points.Add(p1);
            _points.Add(p2);
            _points.Add(p3);
        }

        /// <summary>
        /// Requires atleast 3 points
        /// </summary>
        /// 
        /// <param name="points">ordered list of points forming the polygon. 
        /// No duplicates are allowed</param>
        public Polygon(List<PolygonPoint> points)
        {
            // Lets do one sanity check that first and last point hasn't got same position
            // Its something that often happen when importing polygon data from other formats
            if (points[0].Equals(points[points.Count - 1]))
            {
                logger.Warn("Removed duplicate point");
                points.RemoveAt(points.Count - 1);
            }
            _points.AddRange(points);
        }

        /// <summary>
        /// Requires atleast 3 points
        /// </summary>
        /// 
        public Polygon(PolygonPoint[] points) : this(points.ToList()) { }

        public TriangulationMode getTriangulationMode()
        {
            return TriangulationMode.POLYGON;
        }

        public int pointCount()
        {
            int count = _points.Count;
            if (_steinerPoints != null)
            {
                count += _steinerPoints.Count;
            }
            return count;
        }

        public void addSteinerPoint(TriangulationPoint point)
        {
            if (_steinerPoints == null)
            {
                _steinerPoints = new List<TriangulationPoint>();
            }
            _steinerPoints.Add(point);
        }

        public void addSteinerPoints(List<TriangulationPoint> points)
        {
            if (_steinerPoints == null)
            {
                _steinerPoints = new List<TriangulationPoint>();
            }
            _steinerPoints.AddRange(points);
        }

        public void clearSteinerPoints()
        {
            if (_steinerPoints != null)
            {
                _steinerPoints.Clear();
            }
        }

        /// <summary>
        /// Assumes: that given polygon is fully inside the current polygon 
        /// </summary>
        /// <param name="poly">a subtraction polygon</param>
        /// 
        public void addHole(Polygon poly)
        {
            if (_holes == null)
            {
                _holes = new List<Polygon>();
            }
            _holes.Add(poly);
            // XXX: tests could be made here to be sure it is fully inside
            //        addSubtraction( poly.getPoints() );
        }

        /// <summary>
        /// Will insert a point in the polygon after given point 
        /// </summary>
        public void insertPointAfter(PolygonPoint a, PolygonPoint newPoint)
        {
            // Validate that 
            int index = _points.IndexOf(a);
            if (index != -1)
            {
                newPoint.setNext(a.getNext());
                newPoint.setPrevious(a);
                a.getNext().setPrevious(newPoint);
                a.setNext(newPoint);
                _points.Insert(index + 1, newPoint);
            }
            else
            {
                throw new Exception("Tried to insert a point into a Polygon after a point not belonging to the Polygon");
            }
        }

        public void addPoints(List<PolygonPoint> list)
        {
            PolygonPoint first;
            foreach (PolygonPoint p in list)
            {
                p.setPrevious(_last);
                if (_last != null)
                {
                    p.setNext(_last.getNext());
                    _last.setNext(p);
                }
                _last = p;
                _points.Add(p);
            }
            first = (PolygonPoint)_points[0];
            _last.setNext(first);
            first.setPrevious(_last);
        }

        /// <summary>
        /// Will add a point after the last point added
        /// </summary>
        public void addPoint(PolygonPoint p)
        {
            p.setPrevious(_last);
            p.setNext(_last.getNext());
            _last.setNext(p);
            _points.Add(p);
        }

        public void removePoint(PolygonPoint p)
        {
            PolygonPoint next, prev;

            next = p.getNext();
            prev = p.getPrevious();
            prev.setNext(next);
            next.setPrevious(prev);
            _points.Remove(p);
        }

        public PolygonPoint getPoint()
        {
            return _last;
        }

        public List<TriangulationPoint> getPoints()
        {
            return _points;
        }

        public List<DelaunayTriangle> getTriangles()
        {
            return m_triangles;
        }

        public void addTriangle(DelaunayTriangle t)
        {
            m_triangles.Add(t);
        }

        public void addTriangles(List<DelaunayTriangle> list)
        {
            m_triangles.AddRange(list);
        }

        public void clearTriangulation()
        {
            if (m_triangles != null)
            {
                m_triangles.Clear();
            }
        }

        /// <summary>
        /// Creates constraints and populates the context with points
        /// </summary>
        public void prepareTriangulation(TriangulationContext tcx)
        {
            if (m_triangles == null)
            {
                m_triangles = new List<DelaunayTriangle>(_points.Count);
            }
            else
            {
                m_triangles.Clear();
            }

            // Outer constraints
            for (int i = 0; i < _points.Count - 1; i++)
            {
                tcx.newConstraint(_points[i], _points[i + 1]);
            }
            tcx.newConstraint(_points[0], _points[_points.Count - 1]);
            tcx.addPoints(_points);

            // Hole constraints
            if (_holes != null)
            {
                foreach (Polygon p in _holes)
                {
                    for (int i = 0; i < p._points.Count - 1; i++)
                    {
                        tcx.newConstraint(p._points[i], p._points[i + 1]);
                    }
                    tcx.newConstraint(p._points[0], p._points[p._points.Count - 1]);
                    tcx.addPoints(p._points);
                }
            }

            if (_steinerPoints != null)
            {
                tcx.addPoints(_steinerPoints);
            }
        }

    }
}
