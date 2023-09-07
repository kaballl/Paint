using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Paint
{
    abstract class IAction
    {
        public virtual void Undo(Canvas canvas) { }
        public virtual void Redo(Canvas canvas) { }
    }
    class AddAction : IAction
    {
        UIElement uiElement;

        public AddAction(UIElement uie)
        {
            uiElement = uie;
        }
        public override void Undo(Canvas canvas)
        {      
            canvas.Children.Remove(uiElement);
        }
        public override void Redo(Canvas canvas)
        {
            canvas.Children.Add(uiElement);
        }
    }

    class DelAction : IAction
    {
        UIElement uiElement;

        public DelAction(UIElement uie)
        {
            uiElement = uie;
        }
        public override void Undo(Canvas canvas)
        {
            canvas.Children.Add(uiElement);
        }
        public override void Redo(Canvas canvas)
        {
            canvas.Children.Remove(uiElement);
        }
    }

    class SeriesAction : IAction
    {
        List<IAction> listAction;

        public SeriesAction(List<IAction> chd)
        {
            listAction = chd;
        }
        public override void Undo(Canvas canvas)
        {
            foreach ( IAction hd in listAction)
            {
                hd.Undo( canvas);
            }                
        }
        public override void Redo(Canvas canvas)
        {
            foreach (IAction hd in listAction)
            {
                hd.Redo(canvas);
            }
        }
    }


    class UndoRedoManager
    {
        List<IAction> UndoList = new List<IAction>();
        List<IAction>  RedoList = new List<IAction>();

        public void Add(IAction action)
        {
            UndoList.Add(action);
            RedoList.Clear();
        }

        public void Undo( Canvas canvas)
        {
            if (UndoList.Count == 0)
            {
                return;
            }
            IAction temp = UndoList.Last();
            UndoList.Remove(temp);
            RedoList.Add(temp);
            temp.Undo(canvas);
        }

        public void Redo( Canvas canvas)
        {
            if (RedoList.Count == 0)
            {
                return;
            }
            IAction temp = RedoList.Last();
            RedoList.Remove(temp);
            UndoList.Add(temp);
            temp.Redo(canvas);
        }

    }
}
