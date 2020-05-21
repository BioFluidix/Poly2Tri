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

    public abstract class TriangulationContext
    {
        protected bool _debugEnabled = false;

        protected List<DelaunayTriangle> _triList = new List<DelaunayTriangle>();

        protected List<TriangulationPoint> _points = new List<TriangulationPoint>(200);
        protected TriangulationMode _triangulationMode;
        protected ITriangulatable _triUnit;


        private int _stepCount = 0;


        public int getStepCount() { return _stepCount; }

        public void done()
        {
            _stepCount++;
        }

        public abstract TriangulationAlgorithm algorithm();

        public abstract TriangulationConstraint newConstraint(TriangulationPoint a, TriangulationPoint b);

        public abstract void isDebugEnabled(bool b);

        public virtual void clear()
        {
            _points.Clear();
            _stepCount = 0;
        }


        public virtual void prepareTriangulation(ITriangulatable t)
        {
            _triUnit = t;
            _triangulationMode = t.getTriangulationMode();
            t.prepareTriangulation(this);
        }

        public void addToList(DelaunayTriangle triangle)
        {
            _triList.Add(triangle);
        }

        public List<DelaunayTriangle> getTriangles()
        {
            return _triList;
        }

        public ITriangulatable getTriangulatable()
        {
            return _triUnit;
        }

        public List<TriangulationPoint> getPoints()
        {
            return _points;
        }

        public void update(string message)
        {
            _stepCount++;
            Console.Out.WriteLine(message);
        }


        public TriangulationMode getTriangulationMode()
        {
            return _triangulationMode;
        }

        public bool isDebugEnabled()
        {
            return _debugEnabled;
        }

        public void addPoints(List<TriangulationPoint> points)
        {
            _points.AddRange(points);
        }


    }

    public abstract class TriangulationContext<A> : TriangulationContext where A : TriangulationDebugContext
    {

        #region _debug

        protected A _debug;

        public A getDebugContext()
        {
            return _debug;
        }

        public override void clear()
        {
            base.clear();

            if (_debug != null)
            {
                _debug.clear();
            }

        }

        #endregion

    }
}
