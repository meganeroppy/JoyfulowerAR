using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class RoutePointsWrapper : Object
    {
        public List<Vector2> points;

        public int count
        {
            get
            {
                if (points == null) return 0;
                return points.Count;
            }
        }

        public RoutePointsWrapper(List<Vector2> points)
        {
            this.points = points;
        }
    }
}