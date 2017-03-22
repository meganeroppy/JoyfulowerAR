using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class RouteStepWrapper : Object
    {
        public OnlineMapsDirectionStep step;

        public RouteStepWrapper(OnlineMapsDirectionStep step)
        {
            this.step = step;
        }
    }
}