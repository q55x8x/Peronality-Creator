using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastColoredTextBoxNS;

namespace Personality_Creator
{
    public class GotoStyle : Style
    {
        private TextStyle Textstyle;
        public GotoStyle(TextStyle style)
        {
            this.Textstyle = style;
        }
        public override void Draw(Graphics gr, Point position, Range range)
        {
            Size size = GetSizeOfRange(range);
            Rectangle rect = new Rectangle(position, size);
            StyleVisualMarker marker = new StyleVisualMarker(rect, this.Textstyle);
            AddVisualMarker(range.tb, marker);
        }
    }
}
