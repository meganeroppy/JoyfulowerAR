using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [System.Serializable]
    public class DrawingElementWrapper : Object
    {
        public OnlineMapsDrawingElement drawingElement;

        public DrawingElementWrapper(OnlineMapsDrawingElement drawingElement)
        {
            this.drawingElement = drawingElement;
        }
    }
}