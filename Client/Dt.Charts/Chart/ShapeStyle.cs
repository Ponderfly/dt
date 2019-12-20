#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-01 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using System.ComponentModel;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endregion

namespace Dt.Charts
{
    [EditorBrowsable((EditorBrowsableState) EditorBrowsableState.Never)]
    public class ShapeStyle
    {
        private DoubleCollection _dashArray;
        private Brush _fill;
        private Brush _stroke;
        private double _thickness;
        private Transform _transform;
        internal bool DarkOutline;
        internal Brush FillAuto;
        internal Brush StrokeAuto;

        internal event EventHandler Changed;

        public ShapeStyle()
        {
            DarkOutline = true;
            _thickness = 2.0;
        }

        internal ShapeStyle(Brush fill, Brush stroke, double thickness, DoubleCollection dashes)
        {
            DarkOutline = true;
            _thickness = 2.0;
            Fill = fill;
            Stroke = stroke;
            StrokeThickness = thickness;
            StrokeDashArray = dashes;
        }

        internal void Apply(Shape sh)
        {
            Apply(null, sh, new Point(double.NaN, double.NaN), Extensions.EmptyRect, double.NaN, true);
        }

        internal void Apply(UIElement parent, Shape sh)
        {
            Apply(parent, sh, new Point(double.NaN, double.NaN), Extensions.EmptyRect, double.NaN, true);
        }

        internal void Apply(UIElement parent, Shape sh, Point center, Rect bounds, double radius)
        {
            Apply(parent, sh, center, bounds, radius, true);
        }

        internal void Apply(UIElement parent, Shape sh, Point center, Rect bounds, double radius, bool fill)
        {
            if ((sh.Fill == null) && fill)
            {
                Brush brush = Fill;
                sh.Fill = brush;
            }
            if (sh.Stroke == null)
            {
                UIElement tag = parent;
                if (!(parent is Lines))
                {
                    tag = sh.Tag as UIElement;
                }
                if (((tag is Lines) && !(tag is Area)) && (Stroke == StrokeAuto))
                {
                    if (((Lines) tag).IsFilled)
                    {
                        sh.Stroke = Stroke;
                    }
                    else
                    {
                        sh.Stroke = Fill;
                    }
                }
                else
                {
                    sh.Stroke = Stroke;
                }
            }
            if (StrokeThickness != 0.0)
            {
                sh.StrokeThickness = StrokeThickness;
            }
            if (StrokeDashArray != null)
            {
                DoubleCollection strokeDashArray = sh.StrokeDashArray;
                if (strokeDashArray == null)
                {
                    strokeDashArray = new DoubleCollection();
                    sh.StrokeDashArray = strokeDashArray;
                }
                else
                {
                    strokeDashArray.Clear();
                }
                int num = StrokeDashArray.Count;
                for (int i = 0; i < num; i++)
                {
                    strokeDashArray.Add(StrokeDashArray[i]);
                }
            }
            if (RenderTransform != null)
            {
                sh.RenderTransform = RenderTransform;
            }
        }

        internal bool CanConvertToString()
        {
            if ((Stroke != null) && !(Stroke is SolidColorBrush))
            {
                return false;
            }
            if (Fill != null)
            {
                return (Fill is SolidColorBrush);
            }
            return true;
        }

        internal string ConvertToString()
        {
            ConverterHelper helper = new ConverterHelper();
            if (Stroke is SolidColorBrush)
            {
                helper.Add("Stroke", Stroke);
            }
            if (Fill is SolidColorBrush)
            {
                helper.Add("Fill", Fill);
            }
            if (StrokeThickness != 1.0)
            {
                helper.Add("StrokeThickness", (double) StrokeThickness);
            }
            return helper.ConvertToString();
        }

        private void FireChanged(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(sender, e);
            }
        }

        internal static ShapeStyle ParseString(string s)
        {
            ShapeStyle style = null;
            ConverterHelper helper = ConverterHelper.ParseString(s);
            if (helper != null)
            {
                style = new ShapeStyle();
                if (helper.ContainsKey("StrokeThickness"))
                {
                    style.StrokeThickness = double.Parse(helper["StrokeThickness"].ToString(), (IFormatProvider) CultureInfo.InvariantCulture);
                }
            }
            return style;
        }

        public Brush Fill
        {
            get { return  _fill; }
            set
            {
                _fill = value;
                FireChanged(this, EventArgs.Empty);
            }
        }

        public Transform RenderTransform
        {
            get { return  _transform; }
            set
            {
                _transform = value;
                FireChanged(this, EventArgs.Empty);
            }
        }

        public Brush Stroke
        {
            get { return  _stroke; }
            set
            {
                _stroke = value;
                FireChanged(this, EventArgs.Empty);
            }
        }

        public DoubleCollection StrokeDashArray
        {
            get { return  _dashArray; }
            set
            {
                _dashArray = value;
                FireChanged(this, EventArgs.Empty);
            }
        }

        public double StrokeThickness
        {
            get { return  _thickness; }
            set
            {
                _thickness = value;
                FireChanged(this, EventArgs.Empty);
            }
        }
    }
}

