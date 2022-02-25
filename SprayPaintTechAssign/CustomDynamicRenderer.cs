using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Input;



namespace SprayPaintTechAssign
{
    internal class CustomDynamicRenderer : DynamicRenderer
    {

        [ThreadStatic]
        static private Brush brush = null;

        [ThreadStatic]
        static private Pen pen = null;

        private Point prevPoint;



        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            // Allocate memory to store the previous point to draw from.
            prevPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
            base.OnStylusDown(rawStylusInput);
        }



        protected override void OnDraw(DrawingContext drawingContext,
                                       StylusPointCollection stylusPoints,
                                       Geometry geometry, Brush fillBrush)
        {
            // Create a new Brush
            brush = new SolidColorBrush(penEdit.colorSelector);
            brush.Opacity = penEdit.Opacity;


            // Create a new Pen
            pen = new Pen(brush, 2d);


            // Draw ellipses between
            // all the StylusPoints that have come in.
            for (int i = 0; i < stylusPoints.Count; i++)
            {
                Point pt = (Point)stylusPoints[i];
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
