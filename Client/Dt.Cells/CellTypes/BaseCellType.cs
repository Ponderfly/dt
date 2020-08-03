#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-03 创建
******************************************************************************/
#endregion

#region 引用命名
using Dt.Cells.Data;
using Dt.Cells.UI;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
#endregion

namespace Dt.Cells.CellTypes
{
    internal class BaseCellType : ICellType, IZoomSupport, IFormulaEditingSupport
    {
        Cell _dataContext;
        protected FrameworkElement _displayElement;
        protected FrameworkElement _editingElement;
        double _zoomFactor = 1.0;
        FontFamily appliedFontFamily;
        double? appliedFontSize;
        FontStretch? appliedFontStretch = null;
        FontStyle? appliedFontStyle = null;
        FontWeight? appliedFontWeight = null;
        Brush appliedForeground;
        HorizontalAlignment? appliedHorizontalAlignment = null;
        Windows.UI.Xaml.Thickness? appliedMargin = null;
        double? appliedShrinkFactor = null;
        string appliedText = "";
        TextAlignment? appliedTextAlignment = TextAlignment.Left;
        bool? appliedTextWrapping = false;
        VerticalAlignment? appliedVerticalAlignment = null;
        Type cachedValueType;

        public virtual bool ApplyEditing(SheetView sheetView, bool allowFormula)
        {
            TextBox box = _editingElement as TextBox;
            if (box != null)
            {
                bool isFormulaApplied = false;
                string appliedFormula = null;
                bool flag2 = ApplyValueToCell(sheetView, DataContext, allowFormula, box.Text, cachedValueType, out isFormulaApplied, out appliedFormula);
                cachedValueType = null;
                return flag2;
            }
            return false;
        }

        internal static bool ApplyValueToCell(SheetView sheetView, Cell bindingCell, bool allowFormula, object editorValue, Type valueType, out bool isFormulaApplied, out string appliedFormula)
        {
            isFormulaApplied = false;
            appliedFormula = null;
            if (bindingCell != null)
            {
                if (ContainsArrayFormula(bindingCell.Worksheet.FindFormulas(bindingCell.Row.Index, bindingCell.Column.Index, 1, 1)))
                {
                    return false;
                }
                string str = (string) (editorValue as string);
                if ((allowFormula && (str != null)) && (str.StartsWith("=") && (str.Length > 1)))
                {
                    appliedFormula = str.TrimStart(new char[] { '=' });
                    try
                    {
                        isFormulaApplied = true;
                        bindingCell.Formula = appliedFormula;
                    }
                    catch
                    {
                        return false;
                    }
                    return true;
                }
                if (!string.IsNullOrEmpty(bindingCell.Formula))
                {
                    bindingCell.Formula = null;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    try
                    {
                        if (str.StartsWith("'="))
                        {
                            str = str.Substring(1);
                        }
                        IFormatter actualFormatter = bindingCell.ActualFormatter;
                        if ((actualFormatter != null) && !(actualFormatter is AutoFormatter))
                        {
                            object obj2 = actualFormatter.Parse(str);
                            object obj3 = null;
                            if (obj2 == null)
                            {
                                obj3 = str;
                            }
                            else
                            {
                                obj3 = obj2;
                            }
                            obj3 = sheetView.RaiseCellValueApplying(bindingCell.Row.Index, bindingCell.Column.Index, obj3);
                            bindingCell.Value = obj3;
                        }
                        else
                        {
                            UpdateFormatter(str, bindingCell, valueType);
                        }
                        goto Label_0139;
                    }
                    catch (InvalidCastException)
                    {
                        bindingCell.Value = editorValue as string;
                        goto Label_0139;
                    }
                }
                bindingCell.Value = null;
            }
        Label_0139:
            return true;
        }

        internal static bool ContainsArrayFormula(object[,] formulas)
        {
            if (formulas != null)
            {
                for (int i = 0; i < formulas.GetLength(0); i++)
                {
                    CellRange range = formulas[i, 0] as CellRange;
                    if ((range != null) && ((range.RowCount > 1) || (range.ColumnCount > 1)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual FrameworkElement CreateDisplayElement()
        {
            return new TextBlock();
        }

        protected virtual FrameworkElement CreateEditingElement()
        {
            return new EditingElement();
        }

        Windows.UI.Xaml.Thickness GetDefaultMarginForDisplay(double fontSize)
        {
            double zoomFactor = ZoomFactor;
            Windows.UI.Xaml.Thickness excelBlank = MeasureHelper.GetExcelBlank();
            Windows.UI.Xaml.Thickness textBlockBlank = MeasureHelper.GetTextBlockBlank(fontSize);
            double left = excelBlank.Left - textBlockBlank.Left;
            double right = excelBlank.Right - textBlockBlank.Right;
            double top = excelBlank.Top - textBlockBlank.Top;
            return new Windows.UI.Xaml.Thickness(left, top, right, excelBlank.Bottom - textBlockBlank.Bottom);
        }

        Windows.UI.Xaml.Thickness GetDefaultPaddingForEdit(double fontSize)
        {
            double zoomFactor = ZoomFactor;
            Windows.UI.Xaml.Thickness excelBlank = MeasureHelper.GetExcelBlank();
            Windows.UI.Xaml.Thickness textBoxBlank = MeasureHelper.GetTextBoxBlank(fontSize);
            double left = excelBlank.Left - textBoxBlank.Left;
            double right = excelBlank.Right - textBoxBlank.Right;
            double top = excelBlank.Top - textBoxBlank.Top;
            return new Windows.UI.Xaml.Thickness(left, top, right, excelBlank.Bottom - textBoxBlank.Bottom);
        }

        public virtual FrameworkElement GetDisplayElement()
        {
            if (_displayElement == null)
            {
                _displayElement = CreateDisplayElement();
            }
            return _displayElement;
        }

        public virtual FrameworkElement GetEditingElement()
        {
            if (_editingElement == null)
            {
                _editingElement = CreateEditingElement();
            }
            return _editingElement;
        }

        public virtual bool HasEditingElement()
        {
            return (_editingElement != null);
        }

        public virtual void InitDisplayElement(string text)
        {
            TextAlignment center;
            VerticalAlignment alignment3;
            FontWeight weight;
            FontFamily family;
            Thickness thickness;
            TextBlock displayElement = GetDisplayElement() as TextBlock;
            Cell dataContext = DataContext;
            if (dataContext == null)
            {
                return;
            }
            if (displayElement == null)
            {
                return;
            }
            if (appliedText != text)
            {
                displayElement.Text = text;
                appliedText = text;
            }
            HorizontalAlignment alignment = dataContext.ToHorizontalAlignment();
            if (appliedHorizontalAlignment.HasValue)
            {
                HorizontalAlignment? appliedHorizontalAlignment = this.appliedHorizontalAlignment;
                HorizontalAlignment alignment4 = alignment;
                if ((((HorizontalAlignment) appliedHorizontalAlignment.GetValueOrDefault()) == alignment4) && appliedHorizontalAlignment.HasValue)
                {
                    goto Label_0085;
                }
            }
            displayElement.HorizontalAlignment = alignment;
            appliedHorizontalAlignment = new HorizontalAlignment?(alignment);
        Label_0085:
            center = TextAlignment.Left;
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    center = TextAlignment.Center;
                    break;

                case HorizontalAlignment.Right:
                    center = TextAlignment.Right;
                    break;
            }
            if (appliedTextAlignment.HasValue)
            {
                TextAlignment? appliedTextAlignment = this.appliedTextAlignment;
                TextAlignment alignment6 = center;
                if ((((TextAlignment) appliedTextAlignment.GetValueOrDefault()) == alignment6) && appliedTextAlignment.HasValue)
                {
                    goto Label_00E8;
                }
            }
            displayElement.TextAlignment = center;
            appliedTextAlignment = new TextAlignment?(center);
        Label_00E8:
            alignment3 = dataContext.ActualVerticalAlignment.ToVerticalAlignment();
            if (appliedVerticalAlignment.HasValue)
            {
                VerticalAlignment? appliedVerticalAlignment = this.appliedVerticalAlignment;
                VerticalAlignment alignment7 = alignment3;
                if ((((VerticalAlignment) appliedVerticalAlignment.GetValueOrDefault()) == alignment7) && appliedVerticalAlignment.HasValue)
                {
                    goto Label_013D;
                }
            }
            displayElement.VerticalAlignment = alignment3;
            appliedVerticalAlignment = new VerticalAlignment?(alignment3);
        Label_013D:
            if (string.IsNullOrWhiteSpace(appliedText))
            {
                return;
            }
            Brush actualForeground = dataContext.ActualForeground;
            if (actualForeground != null)
            {
                if (appliedForeground == null)
                {
                    SolidColorBrush brush2 = actualForeground as SolidColorBrush;
                    if ((brush2 == null) || !(brush2.Color == Colors.Black))
                    {
                        displayElement.Foreground = actualForeground;
                        appliedForeground = actualForeground;
                    }
                }
                else
                {
                    displayElement.Foreground = actualForeground;
                    appliedForeground = actualForeground;
                }
            }
            else if (appliedForeground != null)
            {
                displayElement.Foreground = actualForeground;
                displayElement.ClearValue(TextBlock.ForegroundProperty);
            }
            FontStyle actualFontStyle = dataContext.ActualFontStyle;
            if (appliedFontStyle.HasValue)
            {
                FontStyle? appliedFontStyle = this.appliedFontStyle;
                FontStyle style2 = actualFontStyle;
                if ((((FontStyle) appliedFontStyle.GetValueOrDefault()) == style2) && appliedFontStyle.HasValue)
                {
                    goto Label_0210;
                }
            }
            displayElement.FontStyle = actualFontStyle;
            appliedFontStyle = new FontStyle?(actualFontStyle);
        Label_0210:
            weight = dataContext.ActualFontWeight;
            if (!appliedFontWeight.HasValue || (appliedFontWeight.Value.Weight != weight.Weight))
            {
                displayElement.FontWeight = weight;
                appliedFontWeight = new FontWeight?(weight);
            }
            FontStretch actualFontStretch = dataContext.ActualFontStretch;
            if (appliedFontStretch.HasValue)
            {
                FontStretch? appliedFontStretch = this.appliedFontStretch;
                FontStretch stretch2 = actualFontStretch;
                if ((((FontStretch) appliedFontStretch.GetValueOrDefault()) == stretch2) && appliedFontStretch.HasValue)
                {
                    goto Label_02A3;
                }
            }
            displayElement.FontStretch = actualFontStretch;
            appliedFontStretch = new FontStretch?(actualFontStretch);
        Label_02A3:
            family = dataContext.ActualFontFamily;
            if (family != null)
            {
                if (appliedFontFamily != family)
                {
                    displayElement.FontFamily = family;
                    appliedFontFamily = family;
                }
            }
            else if (appliedFontFamily != null)
            {
                displayElement.ClearValue(TextBlock.FontFamilyProperty);
                appliedFontFamily = family;
            }
            bool actualWordWrap = dataContext.ActualWordWrap;
            if (appliedTextWrapping.HasValue)
            {
                bool? appliedTextWrapping = this.appliedTextWrapping;
                bool flag3 = actualWordWrap;
                if ((appliedTextWrapping.GetValueOrDefault() == flag3) && appliedTextWrapping.HasValue)
                {
                    goto Label_033C;
                }
            }
            displayElement.TextWrapping = actualWordWrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
            appliedTextWrapping = new bool?(actualWordWrap);
        Label_033C:
            if (!actualWordWrap && dataContext.ActualShrinkToFit)
            {
                double fontSize = dataContext.ActualFontSize * ZoomFactor;
                if (fontSize > 0.0)
                {
                    double num2 = 1.0;
                    FontFamily fontFamily = family;
                    if (family == null)
                    {
                        fontFamily = displayElement.FontFamily;
                    }
                    object textFormattingMode = null;
                    double width = MeasureHelper.MeasureText(appliedText, fontFamily, fontSize, appliedFontStretch.Value, appliedFontStyle.Value, appliedFontWeight.Value, new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity), false, textFormattingMode, displayElement.UseLayoutRounding, ZoomFactor).Width;
                    double num4 = dataContext.ActualTextIndent * ZoomFactor;
                    double num5 = dataContext.Worksheet.GetActualColumnWidth(dataContext.Column.Index, dataContext.ColumnSpan, dataContext.SheetArea) * ZoomFactor;
                    num5 = MeasureHelper.ConvertExcelCellSizeToTextSize(new Windows.Foundation.Size(num5, double.PositiveInfinity), ZoomFactor).Width;
                    num5 = Math.Max((double) 0.0, (double) (num5 - num4));
                    num2 = num5 / width;
                    if (num5 < width)
                    {
                        appliedShrinkFactor = new double?(num2);
                    }
                    else
                    {
                        appliedShrinkFactor = null;
                    }
                }
            }
            else
            {
                appliedShrinkFactor = null;
            }
            double num6 = dataContext.ActualFontSize * ZoomFactor;
            if (appliedShrinkFactor.HasValue)
            {
                num6 *= appliedShrinkFactor.Value;
            }
            if (appliedFontSize.HasValue)
            {
                double num8 = num6;
                double? appliedFontSize = this.appliedFontSize;
                if ((num8 == ((double) appliedFontSize.GetValueOrDefault())) && appliedFontSize.HasValue)
                {
                    goto Label_0554;
                }
            }
            if (num6 > 0.0)
            {
                displayElement.FontSize = num6;
                appliedFontSize = new double?(num6);
            }
            else if (num6 == 0.0)
            {
                displayElement.Text = "";
                appliedText = null;
            }
            else
            {
                displayElement.ClearValue(TextBlock.FontSizeProperty);
                appliedFontSize = null;
            }
        Label_0554:
            thickness = GetDefaultMarginForDisplay(num6);
            double num7 = dataContext.ActualTextIndent * ZoomFactor;
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    num7 = 0.0;
                    break;

                case HorizontalAlignment.Left:
                    thickness.Left += num7;
                    break;

                case HorizontalAlignment.Right:
                    thickness.Right += num7;
                    break;
            }
            if (appliedMargin.HasValue)
            {
                Windows.UI.Xaml.Thickness? appliedMargin = this.appliedMargin;
                Windows.UI.Xaml.Thickness thickness2 = thickness;
                if (appliedMargin.HasValue && (appliedMargin.GetValueOrDefault() == thickness2))
                {
                    goto Label_05F2;
                }
            }
            displayElement.Margin = thickness;
            appliedMargin = new Windows.UI.Xaml.Thickness?(thickness);
        Label_05F2:
            if (dataContext.ActualUnderline)
            {
                Underline underline = new Underline();
                Run run = new Run();
                run.Text = displayElement.Text;
                underline.Inlines.Add(run);
                displayElement.Inlines.Clear();
                displayElement.Inlines.Add(underline);
            }
            else
            {
                string str = displayElement.Text;
                displayElement.Inlines.Clear();
                displayElement.Text = str;
            }
            InitStrikethroughforDisplayElement(displayElement, dataContext);
        }

        public virtual void InitEditingElement()
        {
            IFormatter formatter2;
            Action action2 = null;
            IFormatter preferredEditingFormatter = null;
            TextBox tbElement = GetEditingElement() as TextBox;
            Cell bindingCell = DataContext;
            StyleInfo info = bindingCell.Worksheet.GetActualStyleInfo(bindingCell.Row.Index, bindingCell.Column.Index, bindingCell.SheetArea, true);
            if ((tbElement == null) || (info == null))
            {
                return;
            }
            bool flag = false;
            string text = string.Empty;
            string formula = "";
            using (((IUIActionExecuter) bindingCell.Worksheet).BeginUIAction())
            {
                int index = bindingCell.Row.Index;
                int column = bindingCell.Column.Index;
                formula = bindingCell.Formula;
                if (formula == null)
                {
                    object[,] objArray = bindingCell.Worksheet.FindFormulas(index, column, 1, 1);
                    if (objArray.GetLength(0) > 0)
                    {
                        string str3 = objArray[0, 1].ToString();
                        int length = str3.Length;
                        if (((length > 2) && str3.StartsWith("{")) && str3.EndsWith("}"))
                        {
                            formula = str3.Substring(1, length - 2);
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(formula))
            {
                text = "=" + formula;
                flag = true;
                goto Label_0293;
            }
            if (bindingCell.Value == null)
            {
                if (bindingCell.Value == null)
                {
                    cachedValueType = null;
                }
                goto Label_0293;
            }
            cachedValueType = bindingCell.Value.GetType();
            preferredEditingFormatter = new GeneralFormatter().GetPreferredEditingFormatter(bindingCell.Value);
            if ((preferredEditingFormatter != null) && (info.Formatter is AutoFormatter))
            {
                try
                {
                    text = preferredEditingFormatter.Format(bindingCell.Value);
                    goto Label_01F7;
                }
                catch
                {
                    text = bindingCell.Text;
                    goto Label_01F7;
                }
            }
            text = bindingCell.Text;
        Label_01F7:
            formatter2 = info.Formatter;
            if (formatter2 is GeneralFormatter)
            {
                GeneralFormatter formatter3 = formatter2 as GeneralFormatter;
                switch (formatter3.GetFormatType(bindingCell.Value))
                {
                    case NumberFormatType.Number:
                    case NumberFormatType.Text:
                        formatter2 = new GeneralFormatter();
                        break;
                }
            }
            if ((formatter2 != null) && !(formatter2 is AutoFormatter))
            {
                text = formatter2.Format(bindingCell.Value);
            }
            if (((text != null) && text.StartsWith("=")) && CanUserEditFormula)
            {
                text = "'" + text;
            }
        Label_0293:
            tbElement.Text = text;
            if (info.FontSize > 0.0)
            {
                tbElement.FontSize = info.FontSize * ZoomFactor;
            }
            else
            {
                tbElement.ClearValue(TextBlock.FontSizeProperty);
            }
            tbElement.FontStyle = info.FontStyle;
            tbElement.FontWeight = info.FontWeight;
            tbElement.FontStretch = info.FontStretch;
            if (info.IsFontFamilySet() && (info.FontFamily != null))
            {
                tbElement.FontFamily = info.FontFamily;
            }
            else if (info.IsFontThemeSet())
            {
                string fontTheme = info.FontTheme;
                IThemeSupport worksheet = bindingCell.Worksheet;
                if (worksheet != null)
                {
                    tbElement.FontFamily = worksheet.GetThemeFont(fontTheme);
                }
            }
            else
            {
                tbElement.ClearValue(Control.FontFamilyProperty);
            }
            Brush foreground = null;
            if (info.IsForegroundSet())
            {
                foreground = info.Foreground;
            }
            else if (info.IsForegroundThemeColorSet())
            {
                Action action = null;
                string fname = info.ForegroundThemeColor;
                if ((!string.IsNullOrEmpty(fname) && (bindingCell.Worksheet != null)) && (bindingCell.Worksheet.Workbook != null))
                {
                    if (action == null)
                    {
                        action = delegate {
                            foreground = new SolidColorBrush(bindingCell.Worksheet.Workbook.GetThemeColor(fname));
                        };
                    }
                    Dt.Cells.Data.UIAdaptor.InvokeSync(action);
                }
            }
            if (foreground != null)
            {
                tbElement.Foreground = foreground;
            }
            else
            {
                if (action2 == null)
                {
                    action2 = delegate {
                        tbElement.Foreground = new SolidColorBrush(Colors.Black);
                    };
                }
                Dt.Cells.Data.UIAdaptor.InvokeSync(action2);
            }
            HorizontalAlignment alignment = bindingCell.ToHorizontalAlignment();
            VerticalAlignment alignment2 = info.VerticalAlignment.ToVerticalAlignment();
            tbElement.VerticalContentAlignment = alignment2;
            if (flag)
            {
                tbElement.TextAlignment = TextAlignment.Left;
            }
            else if (!bindingCell.ActualWordWrap)
            {
                switch (alignment)
                {
                    case HorizontalAlignment.Left:
                    case HorizontalAlignment.Stretch:
                        tbElement.TextAlignment = TextAlignment.Left;
                        break;

                    case HorizontalAlignment.Center:
                        tbElement.TextAlignment = TextAlignment.Center;
                        break;

                    case HorizontalAlignment.Right:
                        tbElement.TextAlignment = TextAlignment.Right;
                        break;
                }
            }
            else
            {
                tbElement.TextAlignment = TextAlignment.Left;
            }
            Windows.UI.Xaml.Thickness defaultPaddingForEdit = GetDefaultPaddingForEdit(tbElement.FontSize);
            tbElement.Margin = defaultPaddingForEdit;
            tbElement.TextWrapping = TextWrapping.Wrap;
            bool actualUnderline = bindingCell.ActualUnderline;
            bool actualStrikethrough = bindingCell.ActualStrikethrough;
        }

        static void InitStrikethroughforDisplayElement(TextBlock tbElement, Cell cell)
        {
            if (cell.ActualStrikethrough)
            {
                foreach (UIElement element in (tbElement.Parent as Panel).Children)
                {
                    if (element is StrikethroughView)
                    {
                        StrikethroughView view = element as StrikethroughView;
                        if (view.LineContainer != null)
                        {
                            foreach (UIElement element2 in view.LineContainer.Children)
                            {
                                if (element2 is Windows.UI.Xaml.Shapes.Line)
                                {
                                    (element2 as Windows.UI.Xaml.Shapes.Line).Stroke = tbElement.Foreground;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }

        public void SetEditingElement(FrameworkElement editingElement)
        {
            if (!object.ReferenceEquals(_editingElement, editingElement))
            {
                _editingElement = editingElement;
            }
        }

        static void UpdateFormatter(string text, Cell cell, Type cacheValueType)
        {
            object obj2 = null;
            GeneralFormatter preferredDisplayFormatter = new GeneralFormatter().GetPreferredDisplayFormatter(text, out obj2) as GeneralFormatter;
            object obj3 = obj2;
            if (((cell.ActualFormatter != null) && (obj2 != null)) && ((cell.ActualFormatter is AutoFormatter) && !preferredDisplayFormatter.FormatString.Equals("General")))
            {
                cell.Formatter = new AutoFormatter(preferredDisplayFormatter);
            }
            else if (cell.ActualFormatter == null)
            {
                cell.Formatter = new AutoFormatter(preferredDisplayFormatter);
            }
            cell.Value = obj3;
        }

        public bool CanUserEditFormula { get; set; }

        public Cell DataContext
        {
            get { return  _dataContext; }
            set { _dataContext = value; }
        }

        public double ZoomFactor
        {
            get { return  _zoomFactor; }
            set { _zoomFactor = value; }
        }
    }
}

