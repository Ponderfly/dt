#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-02 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
#endregion

namespace Dt.Cells.Data
{
    /// <summary>
    /// Represents chart symbol of BubbleDataSeries.
    /// </summary>
    public class BubbleDataMarker : XYDataMarker, IBubbleDataPoint, IXYDataPoint, IDataPoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Dt.Cells.Data.BubbleDataMarker" /> class.
        /// </summary>
        public BubbleDataMarker()
        {
        }

        internal BubbleDataMarker(SpreadBubbleSeries bubbleSeries, int pointIndex) : base(bubbleSeries, pointIndex)
        {
        }

        private SpreadBubbleSeries BubbleDataSeries
        {
            get { return  (base.DataSeries as SpreadBubbleSeries); }
        }

        /// <summary>
        /// Gets the size value.
        /// </summary>
        /// <value>
        /// The size value.
        /// </value>
        public double? SizeValue
        {
            get
            {
                if (((this.BubbleDataSeries != null) && (base.PointIndex >= 0)) && (base.PointIndex < this.BubbleDataSeries.SizeValues.Count))
                {
                    return new double?(this.BubbleDataSeries.SizeValues[base.PointIndex]);
                }
                return null;
            }
        }
    }
}
