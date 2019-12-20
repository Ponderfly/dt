#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-01 创建
******************************************************************************/
#endregion

#region 引用命名
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
#endregion

namespace Dt.Charts
{
    public partial class StepArea : Area
    {
        internal override object Clone()
        {
            StepArea clone = new StepArea();
            base.CloneAttributes(clone);
            clone.Smoothed = base.Smoothed;
            return clone;
        }

        protected override bool Render(RenderContext rc)
        {
            Point[] points = rc.Points;
            if (points == null)
            {
                return false;
            }
            BaseRenderer renderer = rc.Renderer as BaseRenderer;
            bool inverted = (renderer != null) && renderer.Inverted;
            bool flag2 = (renderer != null) && renderer.IsStacked;
            double[] previousValues = rc.PreviousValues;
            double d = inverted ? rc.ConvertX(base.Origin) : rc.ConvertY(base.Origin);
            double naN = double.NaN;
            if ((rc.OptimizationRadiusScope & OptimizationRadiusScope.Lines) > ((OptimizationRadiusScope) 0))
            {
                naN = rc.OptimizationRadius;
            }
            if (double.IsNaN(d))
            {
                d = inverted ? (rc.XReversed ? (rc.Bounds2D.X + rc.Bounds2D.Width) : rc.Bounds2D.X) : (rc.YReversed ? rc.Bounds2D.Y : (rc.Bounds2D.Y + rc.Bounds2D.Height));
            }
            Rect cr = rc.IsCustomClipping ? new Rect(rc.Bounds2D.X - 2.0, rc.Bounds2D.Y - 2.0, rc.Bounds2D.Width + 4.0, rc.Bounds2D.Height + 4.0) : Extensions.EmptyRect;
            if (flag2 && (previousValues != null))
            {
                points = Lines.CreateSteps(points, inverted);
                previousValues = Lines.CreateSteps(previousValues);
                int length = points.Length;
                Point[] pts = new Point[2 * length];
                for (int i = 0; i < length; i++)
                {
                    pts[i] = points[i];
                    if (inverted)
                    {
                        pts[length + i] = new Point(rc.ConvertX(previousValues[(length - i) - 1]), points[(length - i) - 1].Y);
                    }
                    else
                    {
                        pts[length + i] = new Point(points[(length - i) - 1].X, rc.ConvertY(previousValues[(length - i) - 1]));
                    }
                }
                if (pts != null)
                {
                    PathGeometry geometry = base._geometry;
                    PathFigure figure = base.RenderSegment(pts);
                    geometry.Figures.Add(figure);
                }
            }
            else
            {
                points = Lines.CreateSteps(points, inverted);
                List<Point[]> list = base.SplitPointsWithHoles(points);
                if (list != null)
                {
                    PathGeometry geometry2 = base._geometry;
                    for (int j = 0; j < list.Count; j++)
                    {
                        PathFigure figure2 = null;
                        figure2 = base.RenderNonStacked(list[j], d, inverted, naN, cr);
                        if (figure2 != null)
                        {
                            geometry2.Figures.Add(figure2);
                        }
                    }
                }
            }
            RectangleGeometry geometry3 = new RectangleGeometry();
            geometry3.Rect = rc.Bounds2D;
            base.Clip = geometry3;
            return true;
        }
    }
}

