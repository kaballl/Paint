using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Paint
{
    class PropertiesOfShape
    {
        
        public DoubleCollection OutLineType = new DoubleCollection() { 1, 0 };
        public int currSize = 3;
        public bool selectedButtonColor1 = true;
        public Brush ColorOutLineBrush = Brushes.Black;
        public Brush ColorFillBrush = Brushes.White;
        
        
    }
}
