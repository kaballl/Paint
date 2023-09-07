using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Paint
{
    class DoActionEventArgs
    {
        public IAction action;
    }
    delegate void DoActionHandler(object sender, DoActionEventArgs e);

    class DrawAbstract
    {
        public event DoActionHandler ActionCreated;

        protected Point? startPoint;
        protected Adorner currAdnr;

        protected AdornerLayer adnrLayer;
        protected Canvas MyCanvas;
        protected PropertiesOfShape PropertiesOfShape;

        public void NotifyActionCreated(IAction hd)
        {
            if ( ActionCreated != null)
            {
                ActionCreated(this, new DoActionEventArgs(){ action = hd });
            }
        }
        public DrawAbstract(Canvas canvas, PropertiesOfShape ttv)
        {
            MyCanvas = canvas;
            adnrLayer = AdornerLayer.GetAdornerLayer(canvas);
            PropertiesOfShape = ttv;
        }

        public virtual void MouseDown()
        {
            startPoint = Mouse.GetPosition(MyCanvas);
        }
        public virtual void MouseMove()
        { }
        public virtual void MouseUp()
        {
            startPoint = null;
        }

        
        
    }
}
