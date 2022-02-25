using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Ink;



namespace SprayPaintTechAssign
{
    // A class for rendering custom strokes
    internal class CustomStroke : Stroke
    {

        Brush brush;
        Pen pen;

        public CustomStroke(StylusPointCollection stylusPoints)
            : base(stylusPoints)
        {
            // Create the Brush and Pen used for drawing.
            //CREATE BRUSH HERE---------------------
            brush = new SolidColorBrush(penEdit.colorSelector);
            brush.Opacity = penEdit.Opacity;


            pen = new Pen(brush, 2d);
        }

        protected override void DrawCore(DrawingContext drawingContext,
                                         DrawingAttributes drawingAttributes)
        {
            // Allocate memory to store the previous point to draw from.
            Point prevPoint = new Point(double.NegativeInfinity,
                                        double.NegativeInfinity);

            // Draw linear gradient ellipses between
            // all the StylusPoints in the Stroke.
            for (int i = 0; i < this.StylusPoints.Count; i++)
            {
                Point pt = (Point)this.StylusPoints[i];
                Vector v = Point.Subtract(prevPoint, pt);

                // Only draw if we are at least 4 units away
                // from the end of the last ellipse. Otherwise,
                // we're just redrawing and wasting cycles.

                if (v.Length > 4)
                {
                    // Set the thickness of the stroke
                    drawingContext.DrawEllipse(brush, pen, pt, penEdit.Radius, penEdit.Radius);
                    prevPoint = pt;
                }
            }
        }


    }
}
