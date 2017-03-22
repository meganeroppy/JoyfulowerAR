using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class RouteWrapper : Object
    {
        public List<OnlineMapsDirectionStep> steps;

        public int count
        {
            get { return (steps != null) ? steps.Count : 0; }
        }

        public RouteWrapper(List<OnlineMapsDirectionStep> steps)
        {
            this.steps = steps;
        }
    }
}