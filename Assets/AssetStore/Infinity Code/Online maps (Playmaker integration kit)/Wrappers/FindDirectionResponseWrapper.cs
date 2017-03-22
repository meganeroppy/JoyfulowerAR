using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    public class FindDirectionResponseWrapper : Object
    {
        public List<OnlineMapsDirectionStep>[] routes;

        public int count
        {
            get { return (routes != null) ? routes.Length : 0; }
        }

        public FindDirectionResponseWrapper(List<OnlineMapsDirectionStep>[] routes)
        {
            this.routes = routes;
        }
    }
}