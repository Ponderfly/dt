#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2013-10-07 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.CalcEngine;
using System;
#endregion

namespace Dt.CalcEngine.Functions
{
    /// <summary>
    /// Returns the <see cref="T:System.Double" /> accrued interest for a security that pays periodic interest.
    /// </summary>
    public class CalcAccrintFunction : CalcBuiltinFunction
    {
        /// <summary>
        /// Indicates whether the Evaluate method can process missing arguments.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the Evaluate method can process missing arguments; 
        /// otherwise, <see langword="false" />.
        /// </value>
        public override bool AcceptsMissingArgument(int i)
        {
            if ((i != 4) && (i != 6))
            {
                return (i == 7);
            }
            return true;
        }

        /// <summary>
        /// Returns the <see cref="T:System.Double" /> accrued interest for a security that pays periodic interest.
        /// </summary>
        /// <param name="args"><para>
        /// The args contains 6 - 8 items: issue, first_interest, settlement, rate, par, frequency, [basis], [calc_method].
        /// </para>
        /// <para>
        /// Issue is the security's issue date.
        /// </para>
        /// <para>
        /// First_interest is the security's first interest date.
        /// </para>
        /// <para>
        /// Settlement is the security's settlement date.
        /// The security settlement date is the date after the issue date when the security is traded to the buyer.
        /// </para>
        /// <para>
        /// Rate is the security's annual coupon rate.
        /// </para>
        /// <para>
        /// Par is the security's par value. If you omit par, ACCRINT uses $1,000.
        /// </para>
        /// <para>
        /// Frequency is the number of coupon payments per year.
        /// For annual payments, frequency = 1; for semiannual, frequency = 2; for quarterly, frequency = 4.
        /// </para>
        /// <para>
        /// Basis is the type of day count basis to use.
        /// </para></param>
        /// <returns>
        /// A <see cref="T:System.Double" /> value that indicates the evaluate result.
        /// </returns>
        public override object Evaluate(object[] args)
        {
            base.CheckArgumentsLength(args);
            DateTime time = CalcConvert.ToDateTime(args[0]);
            CalcConvert.ToDateTime(args[1]);
            DateTime time2 = CalcConvert.ToDateTime(args[2]);
            double num = CalcConvert.ToDouble(args[3]);
            double num2 = CalcHelper.ArgumentExists(args, 4) ? CalcConvert.ToDouble(args[4]) : 1000.0;
            int num3 = CalcConvert.ToInt(args[5]);
            int basis = CalcHelper.ArgumentExists(args, 6) ? CalcConvert.ToInt(args[6]) : 0;
            if (CalcHelper.ArgumentExists(args, 7))
            {
                CalcConvert.ToBool(args[7]);
            }
            if ((num <= 0.0) || (num2 <= 0.0))
            {
                return CalcErrors.Number;
            }
            if ((basis < 0) || (4 < basis))
            {
                return CalcErrors.Number;
            }
            if (((num3 != 1) && (num3 != 2)) && (num3 != 4))
            {
                return CalcErrors.Number;
            }
            if (DateTime.Compare(time, time2) >= 0)
            {
                return CalcErrors.Number;
            }
            double num5 = FinancialHelper.days_monthly_basis(time, time2, basis);
            double num6 = FinancialHelper.annual_year_basis(time, basis);
            if ((num5 < 0.0) || (num6 <= 0.0))
            {
                return CalcErrors.Number;
            }
            double num7 = (num2 * num) / Convert.ToDouble(num3);
            double num8 = num5 / num6;
            return (double) ((num7 * Convert.ToDouble(num3)) * num8);
        }

        /// <summary>
        /// Gets the maximum number of arguments for the function.
        /// </summary>
        /// <value>
        /// The maximum number of arguments for the function.
        /// </value>
        public override int MaxArgs
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Gets the minimum number of arguments for the function.
        /// </summary>
        /// <value>
        /// The minimum number of arguments for the function.
        /// </value>
        public override int MinArgs
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// Gets The name of the function.
        /// </summary>
        /// <value>
        /// The name of the function.
        /// </value>
        public override string Name
        {
            get
            {
                return "ACCRINT";
            }
        }
    }
}

