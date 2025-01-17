#region 文件描述
/******************************************************************************
* 创建: Daoting
* 摘要: 
* 日志: 2014-07-02 创建
******************************************************************************/
#endregion

#region 引用命名
using System;
using Windows.ApplicationModel.Resources;
#endregion

namespace Dt.Cells.Data
{
    public class ResourceStrings
    {
        static ResourceLoader _loader;

        public static string AnotherWorksheetWithTheSameNameError
        {
            get { return  Loader.GetString("AnotherWorksheetWithTheSameNameError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Array format is illegal.
        /// </summary>
        public static string ArrayFormatIsIllegal
        {
            get { return  Loader.GetString("ArrayFormatIsIllegal"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of the merged cell.
        /// </summary>
        public static string AutoFillChangedPartOfMergedCell
        {
            get { return  Loader.GetString("AutoFillChangedPartOfMergedCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to BorderlineLayout engine only support horizontal or vertical direction at the current version.
        /// </summary>
        public static string BorderLineLayoutNotSupportDirectionError
        {
            get { return  "BorderLineLayoutNotSupportDirectionError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot set a member of NamedStyleCollection to null -- call RemoveAt instead.
        /// </summary>
        public static string CannotSetNullToStyleInfo
        {
            get { return  Loader.GetString("CannotSetNullToStyleInfo"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid {0} {1} index: {2} (must be between -1 and {3}).
        /// </summary>
        public static string CheckArgumentsInvalidRowColumn
        {
            get { return  Loader.GetString("CheckArgumentsInvalidRowColumn"); }
        }

        /// <summary>
        /// Looks up a localized string similar to {0} scale must be between 0 and 1.
        /// </summary>
        public static string ConditionalFormatDataBarRuleScaleOutOfRangeError
        {
            get { return  "ConditionalFormatDataBarRuleScaleOutOfRangeError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to A theme with the same name already exists.
        /// </summary>
        public static string CouldnotAddThemeWithSameName
        {
            get { return  Loader.GetString("CouldnotAddThemeWithSameName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The operation is not supported in the corner area.
        /// </summary>
        public static string CouldnotChangeConnerHeaderCell
        {
            get { return  Loader.GetString("CouldnotChangeConnerHeaderCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The sheet area cannot be found.
        /// </summary>
        public static string CouldnotFindSpecifiedSheetArea
        {
            get { return  Loader.GetString("CouldnotFindSpecifiedSheetArea"); }
        }

        /// <summary>
        /// Looks up a localized string similar to There is only one {0} viewport!.
        /// </summary>
        public static string CouldnotRemoveTheLastViewport
        {
            get { return  Loader.GetString("CouldnotRemoveTheLastViewport"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Specified sheet is not in the collection.
        /// </summary>
        public static string CouldnotSetActiveSheetToNonExistingSheet
        {
            get { return  Loader.GetString("CouldnotSetActiveSheetToNonExistingSheet"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The theme {0} cannot be found in the workbook.
        /// </summary>
        public static string CouldnotSetCurrentThemeToNonExistingTheme
        {
            get { return  Loader.GetString("CouldnotSetCurrentThemeToNonExistingTheme"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column width must be greater than 0.
        /// </summary>
        public static string CouldnotSetNegetiveColumnWidth
        {
            get { return  Loader.GetString("CouldnotSetNegetiveColumnWidth"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row height must be greater than 0.
        /// </summary>
        public static string CouldnotSetNegetiveRowHeight
        {
            get { return  Loader.GetString("CouldnotSetNegetiveRowHeight"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The current worksheet already exists in a table with the same name.
        /// </summary>
        public static string CurrentWorksheetHasTheSameTableError
        {
            get { return  Loader.GetString("CurrentWorksheetHasTheSameTableError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The type of descriptor was added.
        /// </summary>
        public static string CustomNumberFormatNotSupportAddPartError
        {
            get { return  Loader.GetString("CustomNumberFormatNotSupportAddPartError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Does not support this operation in this verison.
        /// </summary>
        public static string DataBindingNotSupport
        {
            get { return  Loader.GetString("DataBindingNotSupport"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Spread does not support data binding of the data source.
        /// </summary>
        public static string DataBindingNullConnection
        {
            get { return  Loader.GetString("DataBindingNullConnection"); }
        }

        /// <summary>
        /// Looks up a localized string similar to It is already bound to a datasource, please unbind first.
        /// </summary>
        public static string DataBindingRebindError
        {
            get { return  Loader.GetString("DataBindingRebindError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot set sheet when it has been bound.
        /// </summary>
        public static string DataBindingSetSheetWhenAlreadyBound
        {
            get { return  Loader.GetString("DataBindingSetSheetWhenAlreadyBound"); }
        }

        public static string DefaultAxisTitle
        {
            get { return  Loader.GetString("DefaultAxisTitle"); }
        }

        public static string DefaultChartTitle
        {
            get { return  Loader.GetString("DefaultChartTitle"); }
        }

        public static string DefaultDataSeriesName
        {
            get { return  Loader.GetString("DefaultDataSeriesName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Does not support setting the style name of the default style.
        /// </summary>
        public static string DonotAllowSetTheNameOfTheDefaultStyle
        {
            get { return  Loader.GetString("DonotAllowSetTheNameOfTheDefaultStyle"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table name cannot be empty.
        /// </summary>
        public static string EmptyTableNameError
        {
            get { return  Loader.GetString("EmptyTableNameError"); }
        }

        public static string ExcelAddChartError
        {
            get { return  Loader.GetString("ExcelAddChartError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Failed to update control status.
        /// </summary>
        public static string excelReaderFinish
        {
            get { return  Loader.GetString("excelReaderFinish"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Failed to set Global NamedRange {0} to control.
        /// </summary>
        public static string ExcelSetGlobalNameError
        {
            get { return  Loader.GetString("ExcelSetGlobalNameError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Failed to set Local NamedRange {0}, Sheet: {1} to control.
        /// </summary>
        public static string ExcelSetNamedCellRangeError
        {
            get { return  Loader.GetString("ExcelSetNamedCellRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Failed to process conditional format.
        /// </summary>
        public static string ExcelWriterWriteConditionalFormatError
        {
            get { return  Loader.GetString("ExcelWriterWriteConditionalFormatError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The worksheet '{0}' cannot be found.
        /// </summary>
        public static string FailedFoundWorksheetError
        {
            get { return  Loader.GetString("FailedFoundWorksheetError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Target range should not have merged cells.
        /// </summary>
        public static string FillRangeHasMergedCell
        {
            get { return  Loader.GetString("FillRangeHasMergedCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to This operation requires the merged cells to be identically sized.
        /// </summary>
        public static string FillRangeHaveDifferentSize
        {
            get { return  Loader.GetString("FillRangeHaveDifferentSize"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The filter should be added in a specified sheet.
        /// </summary>
        public static string FilterSheetNullError
        {
            get { return  Loader.GetString("FilterSheetNullError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to char is illegal.
        /// </summary>
        public static string FormatIllegalCharError
        {
            get { return  "FormatIllegalCharError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to format is illegal.
        /// </summary>
        public static string FormatIllegalFormatError
        {
            get { return  "FormatIllegalFormatError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to string is illegal.
        /// </summary>
        public static string FormatIllegalStringError
        {
            get { return  "FormatIllegalStringError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to The type of descriptor was added.
        /// </summary>
        public static string FormatterCustomNumberFormatAddPartError
        {
            get { return  "FormatterCustomNumberFormatAddPartError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to color is not a valid color name.
        /// </summary>
        public static string FormatterIllegalColorNameError
        {
            get { return  "FormatterIllegalColorNameError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to value is illegal.
        /// </summary>
        public static string FormatterIllegalValueError
        {
            get { return  "FormatterIllegalValueError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to token is illegal.
        /// </summary>
        public static string FormatterIllegaTokenError
        {
            get { return  "FormatterIllegaTokenError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to The color is not a valid color name.
        /// </summary>
        public static string FormatterInvalidColorNameError
        {
            get { return  Loader.GetString("FormatterInvalidColorNameError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to the '\' can't be evaluated.
        /// </summary>
        public static string FormatterTransformEscapeSymbolError
        {
            get { return  "FormatterTransformEscapeSymbolError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to You cannot change part of an array.
        /// </summary>
        public static string FormulaChangePartOfArrayFormulaError
        {
            get { return  "FormulaChangePartOfArrayFormulaError"; }
        }

        /// <summary>
        /// Looks up a localized string similar to GrapeCity.Windows.SpreadSheet.Data.
        /// </summary>
        public static string GrapeCityWindowsSpreadSheetDataAssemblyName
        {
            get { return  Loader.GetString("GrapeCityWindowsSpreadSheetDataAssemblyName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Does not support the type of sheet area.
        /// </summary>
        public static string HtmlGetCellInvalidSheetAreaError
        {
            get { return  Loader.GetString("HtmlGetCellInvalidSheetAreaError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The character is illegal.
        /// </summary>
        public static string IllegalCharError
        {
            get { return  Loader.GetString("IllegalCharError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The format is illegal.
        /// </summary>
        public static string IllegalFormatError
        {
            get { return  Loader.GetString("IllegalFormatError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The token is illegal.
        /// </summary>
        public static string IllegalTokenError
        {
            get { return  Loader.GetString("IllegalTokenError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The tables cannot be intersected.
        /// </summary>
        public static string IntersectTableError
        {
            get { return  Loader.GetString("IntersectTableError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid row index: {0} (must be between -1 and {1}).
        /// </summary>
        public static string InvaildRowIndexWithAllowedRangeBehind
        {
            get { return  Loader.GetString("InvaildRowIndexWithAllowedRangeBehind"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column count: {0} (must be between -1 and {1}.
        /// </summary>
        public static string InvalidColumnCount0MustBeBetween1And1
        {
            get { return  Loader.GetString("InvalidColumnCount0MustBeBetween1And1"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column count: {0} (must be between -1 and {1}).
        /// </summary>
        public static string InvalidColumnCountWithAllowedColumnCountBehind
        {
            get { return  Loader.GetString("InvalidColumnCountWithAllowedColumnCountBehind"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column index: .
        /// </summary>
        public static string InvalidColumnIndex
        {
            get { return  Loader.GetString("InvalidColumnIndex"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column index:{0}(must be between -1 and {1}).
        /// </summary>
        public static string InvalidColumnIndex0MustBeBetween1And1
        {
            get { return  Loader.GetString("InvalidColumnIndex0MustBeBetween1And1"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column index: {0} (must be between -1 and {1}).
        /// </summary>
        public static string InvalidColumnIndexWithAllowedRangeBehind
        {
            get { return  Loader.GetString("InvalidColumnIndexWithAllowedRangeBehind"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Width must be between -1 and 9999999.
        /// </summary>
        public static string InvalidColumnWidth
        {
            get { return  Loader.GetString("InvalidColumnWidth"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid {0} count: {1} (must be between 1 and {2}.
        /// </summary>
        public static string InvalidRowColumntCount2
        {
            get { return  Loader.GetString("InvalidRowColumntCount2"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid row count: {0} (must be between -1 and {1}).
        /// </summary>
        public static string InvalidRowCountWithAllowedRowCountBehind
        {
            get { return  Loader.GetString("InvalidRowCountWithAllowedRowCountBehind"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid row index: {0} (must be between -1 and {1}).
        /// </summary>
        public static string InvalidRowIndex0MustBeBetween1And1
        {
            get { return  Loader.GetString("InvalidRowIndex0MustBeBetween1And1"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid sheet area: {0}.
        /// </summary>
        public static string InvalidSheetArea
        {
            get { return  Loader.GetString("InvalidSheetArea"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid column index: {0} (must be between 0 and {1}.
        /// </summary>
        public static string InvalidTableLocationColumnIndex
        {
            get { return  Loader.GetString("InvalidTableLocationColumnIndex"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid row index: {0} (must be between 0 and {1}).
        /// </summary>
        public static string InvalidTableLocationRowIndex
        {
            get { return  Loader.GetString("InvalidTableLocationRowIndex"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The theme color is invalid.
        /// </summary>
        public static string InvalidThemeColor
        {
            get { return  Loader.GetString("InvalidThemeColor"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The collection is read-only.
        /// </summary>
        public static string ModifyReadonlyCollectionError
        {
            get { return  Loader.GetString("ModifyReadonlyCollectionError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The name that you set is not valid.
        /// </summary>
        public static string NamedStyleInfoInvalidNameError
        {
            get { return  Loader.GetString("NamedStyleInfoInvalidNameError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to item.Name cannot be null.
        /// </summary>
        public static string NamedStyleInfoNameNullError
        {
            get { return  Loader.GetString("NamedStyleInfoNameNullError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The reference is not valid. References for titles, values, or sizes must be a single cell, row, or column.
        /// </summary>
        public static string NeedSingleCellRowColumn
        {
            get { return  Loader.GetString("NeedSingleCellRowColumn"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The '\' cannot be evaluated.
        /// </summary>
        public static string NumberFormatValidEscapeError
        {
            get { return  Loader.GetString("NumberFormatValidEscapeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Only works for numbers.
        /// </summary>
        public static string NumberSourceOnlyWorkedWithNumbers
        {
            get { return  Loader.GetString("NumberSourceOnlyWorkedWithNumbers"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Transformation is not invertible.
        /// </summary>
        public static string PdfInvertError
        {
            get { return  Loader.GetString("PdfInvertError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to A value between 1 and 5 is valid.
        /// </summary>
        public static string PdfSetNumberOfCopiesError
        {
            get { return  Loader.GetString("PdfSetNumberOfCopiesError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Index is missing.
        /// </summary>
        public static string RangeGroupSerializeError
        {
            get { return  Loader.GetString("RangeGroupSerializeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The range should not have a merged cell.
        /// </summary>
        public static string RangeShouldNotHaveMergedCell
        {
            get { return  Loader.GetString("RangeShouldNotHaveMergedCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to {0} end index must be less than or equal to the start index.
        /// </summary>
        public static string ReportingGcSheetSectionEndIndexError
        {
            get { return  Loader.GetString("ReportingGcSheetSectionEndIndexError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The repeat {0} end index must be less than or equal to the start index.
        /// </summary>
        public static string ReportingGcSheetSectionRepeatEndIndexError
        {
            get { return  Loader.GetString("ReportingGcSheetSectionRepeatEndIndexError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Margins do not fit the page size.
        /// </summary>
        public static string ReportingMarginError
        {
            get { return  Loader.GetString("ReportingMarginError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid page range.
        /// </summary>
        public static string ReportingPageRangeError
        {
            get { return  Loader.GetString("ReportingPageRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The height must be greater than 0.
        /// </summary>
        public static string ReportingPaperSizeHightError
        {
            get { return  Loader.GetString("ReportingPaperSizeHightError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The width must be greater than 0.
        /// </summary>
        public static string ReportingPaperSizeWidthError
        {
            get { return  Loader.GetString("ReportingPaperSizeWidthError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Value must be greater than or equal to -1.
        /// </summary>
        public static string ReportingPrintInfoRepeatColumnError
        {
            get { return  Loader.GetString("ReportingPrintInfoRepeatColumnError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid row index: {0}  or {1}.
        /// </summary>
        public static string RowIndexerOutOfRangeError
        {
            get { return  Loader.GetString("RowIndexerOutOfRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to There should be at least one worksheet when saving to Excel.
        /// </summary>
        public static string SaveEmptyWorkbookToExcelError
        {
            get { return  Loader.GetString("SaveEmptyWorkbookToExcelError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The specified startSheetIndex or endSheetIndex is out of range.
        /// </summary>
        public static string SearchArgumentOutOfRange
        {
            get { return  Loader.GetString("SearchArgumentOutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row, Column, RowCount, or ColumnCount is missing.
        /// </summary>
        public static string SerializationError
        {
            get { return  Loader.GetString("SerializationError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot create an instance for the type of array.
        /// </summary>
        public static string SerializeCannotCreateTypeOfArray
        {
            get { return  Loader.GetString("SerializeCannotCreateTypeOfArray"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The index is missing.
        /// </summary>
        public static string SerializeDeserializerArrayError
        {
            get { return  Loader.GetString("SerializeDeserializerArrayError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The type cannot be found.
        /// </summary>
        public static string SerializeDeserializerCellError
        {
            get { return  Loader.GetString("SerializeDeserializerCellError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The current version does not support a serialized image.
        /// </summary>
        public static string SerializeImageError
        {
            get { return  Loader.GetString("SerializeImageError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The matrix data is invalid.
        /// </summary>
        public static string SerializerDeserializeMatrixError
        {
            get { return  Loader.GetString("SerializerDeserializeMatrixError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Array data format is illegal.
        /// </summary>
        public static string SerializerDeserializerIllegalArrayFormat
        {
            get { return  Loader.GetString("SerializerDeserializerIllegalArrayFormat"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Does not support this case.
        /// </summary>
        public static string SerializerInvalidCastError
        {
            get { return  Loader.GetString("SerializerInvalidCastError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The type '{0}' cannot be formatted.
        /// </summary>
        public static string SerializerNotSupportError
        {
            get { return  Loader.GetString("SerializerNotSupportError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The type does not match.
        /// </summary>
        public static string SerializerParseTypeNotMatchError
        {
            get { return  Loader.GetString("SerializerParseTypeNotMatchError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Spread does not support setting the built-in borders (GridLine or NoBorder).
        /// </summary>
        public static string SetBuiltInBorderError
        {
            get { return  Loader.GetString("SetBuiltInBorderError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid object type specified: {0} (must be StyleInfo).
        /// </summary>
        public static string SetOtherTypeToStyleInfoCollectionError
        {
            get { return  Loader.GetString("SetOtherTypeToStyleInfoCollectionError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Setting the span to area {0} is not supported.
        /// </summary>
        public static string SetSpanToNotSupportAreaError
        {
            get { return  Loader.GetString("SetSpanToNotSupportAreaError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Could not set the tag to a nonexistent cell.
        /// </summary>
        public static string SetTagToNonExistingCell
        {
            get { return  Loader.GetString("SetTagToNonExistingCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Sheet name can not be null or empty.
        /// </summary>
        public static string SheetNameCannotBeNullOrEmpty
        {
            get { return  Loader.GetString("SheetNameCannotBeNullOrEmpty"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The object must implement the IComparable interface.
        /// </summary>
        public static string SortCompareError
        {
            get { return  Loader.GetString("SortCompareError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to This operation will cause overlapping spans.
        /// </summary>
        public static string SpanModelOverlappingError
        {
            get { return  Loader.GetString("SpanModelOverlappingError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to "Value must be greater than or equal to -1.".
        /// </summary>
        public static string SpreadReporting_Msg
        {
            get { return  Loader.GetString("SpreadReporting_Msg"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The string is illegal.
        /// </summary>
        public static string StringIsIllegal
        {
            get { return  Loader.GetString("StringIsIllegal"); }
        }

        /// <summary>
        /// Looks up a localized string similar to and.
        /// </summary>
        public static string StyleInfoAnd
        {
            get { return  Loader.GetString("StyleInfoAnd"); }
        }

        /// <summary>
        /// Looks up a localized string similar to This collection is read-only.
        /// </summary>
        public static string StyleInfoChangeReadOnlyCollectionError
        {
            get { return  Loader.GetString("StyleInfoChangeReadOnlyCollectionError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Specified array has insufficient length (array length is {0}, length required is at least {1}).
        /// </summary>
        public static string StyleInfoCopyToArrayLengthError
        {
            get { return  Loader.GetString("StyleInfoCopyToArrayLengthError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Specified array must have rank of 1 (specified array has rank='{0}').
        /// </summary>
        public static string StyleInfoCopyToArrayRankGreaterThanOneError
        {
            get { return  Loader.GetString("StyleInfoCopyToArrayRankGreaterThanOneError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Specified array argument is null (Nothing).
        /// </summary>
        public static string StyleInfoCopyToDestionationNullError
        {
            get { return  Loader.GetString("StyleInfoCopyToDestionationNullError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to are expected.
        /// </summary>
        public static string StyleInfoexpected
        {
            get { return  Loader.GetString("StyleInfoexpected"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Specified index is invalid: {0} (must be greater than 0).
        /// </summary>
        public static string StyleInfoOperationIndexOutOfRangeError
        {
            get { return  Loader.GetString("StyleInfoOperationIndexOutOfRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Invalid index specified: {0} (should be between 0 and {1}).
        /// </summary>
        public static string StyleInfoOperationIndexOutOfRangeWithAllowedRangeBehind
        {
            get { return  Loader.GetString("StyleInfoOperationIndexOutOfRangeWithAllowedRangeBehind"); }
        }

        /// <summary>
        /// Looks up a localized string similar to TextIndent only accepts non-negative values.
        /// </summary>
        public static string StyleInfoTextIndentMustBePositiveValue
        {
            get { return  Loader.GetString("StyleInfoTextIndentMustBePositiveValue"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table {0} already exists in worksheet {1}.
        /// </summary>
        public static string TableAlreayExistInOtherWorksheet
        {
            get { return  Loader.GetString("TableAlreayExistInOtherWorksheet"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table '{0}' already exists in the sheet.
        /// </summary>
        public static string TableCollectionAddTableError
        {
            get { return  Loader.GetString("TableCollectionAddTableError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The column '{0}' is out of the sheet range.
        /// </summary>
        public static string TableColumnDestinationOutOfRange
        {
            get { return  Loader.GetString("TableColumnDestinationOutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to dataSource.
        /// </summary>
        public static string TableDataSourceCannotBeNull
        {
            get { return  Loader.GetString("TableDataSourceCannotBeNull"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table cannot be moved out of the sheet.
        /// </summary>
        public static string TableMoveDestinationOutOfRange
        {
            get { return  Loader.GetString("TableMoveDestinationOutOfRange"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table cannot be found.
        /// </summary>
        public static string TableNotFoundError
        {
            get { return  Loader.GetString("TableNotFoundError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Does not support the type of data source.
        /// </summary>
        public static string TableNotSupportDataSouceError
        {
            get { return  Loader.GetString("TableNotSupportDataSouceError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table must be added in a sheet.
        /// </summary>
        public static string TableOwnerNullError
        {
            get { return  Loader.GetString("TableOwnerNullError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The table must have at least a {0}.
        /// </summary>
        public static string TableResizeOutOfRangeError
        {
            get { return  Loader.GetString("TableResizeOutOfRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Setting a range in the table is not supported, please resize the table instead.
        /// </summary>
        public static string TableResizeRangeError
        {
            get { return  Loader.GetString("TableResizeRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The row '{0}' is out of the sheet range.
        /// </summary>
        public static string TableRowDestinationOutOfRangeError
        {
            get { return  Loader.GetString("TableRowDestinationOutOfRangeError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The row count {0} is out of the sheet range.
        /// </summary>
        public static string TableShowFooterError
        {
            get { return  Loader.GetString("TableShowFooterError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to There are no more rows for the header.
        /// </summary>
        public static string TableShowHeaderError
        {
            get { return  Loader.GetString("TableShowHeaderError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The style '{0}' already exists in the styles.
        /// </summary>
        public static string TableStyleAddCustomStyleError
        {
            get { return  Loader.GetString("TableStyleAddCustomStyleError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The value is illegal.
        /// </summary>
        public static string ValueIsIllegal
        {
            get { return  Loader.GetString("ValueIsIllegal"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The ColumnCount must &gt;= 0.
        /// </summary>
        public static string WorksheetColumnCountMsg
        {
            get { return  Loader.GetString("WorksheetColumnCountMsg"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Internal Error: If you want to support additional argument types for the CreateExpression method, please add it here.
        /// </summary>
        public static string WorksheetCreateExpressionError
        {
            get { return  Loader.GetString("WorksheetCreateExpressionError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The selection model cannot be null.
        /// </summary>
        public static string WorksheetEmptySelection
        {
            get { return  Loader.GetString("WorksheetEmptySelection"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Value was out of the range: 0 to {0}.
        /// </summary>
        public static string WorksheetInvalidRowHeaderColumnCount
        {
            get { return  Loader.GetString("WorksheetInvalidRowHeaderColumnCount"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The RowCount must &gt;= 0.
        /// </summary>
        public static string WorksheetRowCountMsg
        {
            get { return  Loader.GetString("WorksheetRowCountMsg"); }
        }

        /// <summary>
        /// Looks up a localized string similar to ZoomFactor must be between 0.1 and 4.
        /// </summary>
        public static string ZoomFactorOutOfRange
        {
            get { return  Loader.GetString("ZoomFactorOutOfRange"); }
        }
        
        public static string addFloatingObj
        {
            get { return Loader.GetString("addFloatingObj"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cancel.
        /// </summary>
        public static string Cancel
        {
            get { return Loader.GetString("Cancel"); }
        }

        public static string ChangeChartType
        {
            get { return Loader.GetString("ChangeChartType"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Width: {0} pixels.
        /// </summary>
        public static string ColumnResize
        {
            get { return Loader.GetString("ColumnResize"); }
        }

        public static string copyFloatingObj
        {
            get { return Loader.GetString("copyFloatingObj"); }
        }

        public static string deleteFloatingObj
        {
            get { return Loader.GetString("deleteFloatingObj"); }
        }

        public static string dragFloatingObj
        {
            get { return Loader.GetString("dragFloatingObj"); }
        }

        public static string Filter_Blanks
        {
            get { return Loader.GetString("Filter_Blanks"); }
        }

        /// <summary>
        /// Looks up a localized string similar to (Select All).
        /// </summary>
        public static string Filter_SelectAll
        {
            get { return Loader.GetString("Filter_SelectAll"); }
        }

        /// <summary>
        /// Looks up a localized string similar to (FormulaBar_FunctionInformation).
        /// </summary>
        public static string FormulaBar_FunctionInformation
        {
            get { return Loader.GetString("FormulaBar_FunctionInformation"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The file  {0} cannot be found..
        /// </summary>
        public static string gcSpreadExcelOpenExcelFileNotFound
        {
            get { return Loader.GetString("gcSpreadExcelOpenExcelFileNotFound"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The horizontal position is invalid..
        /// </summary>
        public static string gcSpreadInvalidHorizontalPosition
        {
            get { return Loader.GetString("gcSpreadInvalidHorizontalPosition"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The vertical position is invalid..
        /// </summary>
        public static string gcSpreadInvalidVerticalPosition
        {
            get { return Loader.GetString("gcSpreadInvalidVerticalPosition"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The filename cannot be empty..
        /// </summary>
        public static string gcSpreadSaveXMLInvalidFileName
        {
            get { return Loader.GetString("gcSpreadSaveXMLInvalidFileName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to /GrapeCity.Silverlight.SpreadSheet.UI.
        /// </summary>
        public static string GrapeCitySilverlightSpreadSheetUIAssemblyName
        {
            get { return Loader.GetString("GrapeCitySilverlightSpreadSheetUIAssemblyName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to /GrapeCity.WPF.SpreadSheet.UI.
        /// </summary>
        public static string GrapeCityWPFSpreadSheetUIAssemblyName
        {
            get { return Loader.GetString("GrapeCityWPFSpreadSheetUIAssemblyName"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column: {0}.
        /// </summary>
        public static string HorizentalScroll
        {
            get { return Loader.GetString("HorizentalScroll"); }
        }

        public static string moveFloatingObj
        {
            get { return Loader.GetString("moveFloatingObj"); }
        }

        /// <summary>
        /// Looks up a localized string similar to borderIndex error..
        /// </summary>
        public static string NotSupportExceptionBorderIndexError
        {
            get { return Loader.GetString("NotSupportExceptionBorderIndexError"); }
        }

        public static string OK
        {
            get { return Loader.GetString("OK"); }
        }

        public static string resizeFloatingObj
        {
            get { return Loader.GetString("resizeFloatingObj"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Height: {0} pixels.
        /// </summary>
        public static string RowResize
        {
            get { return Loader.GetString("RowResize"); }
        }

        /// <summary>
        /// Looks up a localized string similar to range.
        /// </summary>
        public static string SheetViewClipboardArgumentException
        {
            get { return Loader.GetString("SheetViewClipboardArgumentException"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of an array..
        /// </summary>
        public static string SheetViewDragDropChangePartOChangePartOfAnArray
        {
            get { return Loader.GetString("SheetViewDragDropChangePartOChangePartOfAnArray"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of a merged cell..
        /// </summary>
        public static string SheetViewDragDropChangePartOfMergedCell
        {
            get { return Loader.GetString("SheetViewDragDropChangePartOfMergedCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot complete operation: You are attempting to change a portion of a table row or column in a way that is not allowed..
        /// </summary>
        public static string SheetViewDragDropChangePartOfTable
        {
            get { return Loader.GetString("SheetViewDragDropChangePartOfTable"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The cell you are trying to change is protected and therefore read-only..
        /// </summary>
        public static string SheetViewDragDropChangeProtectCell
        {
            get { return Loader.GetString("SheetViewDragDropChangeProtectCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The column you are trying to change is protected and therefore read-only..
        /// </summary>
        public static string SheetViewDragDropChangeProtectColumn
        {
            get { return Loader.GetString("SheetViewDragDropChangeProtectColumn"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The row you are trying to change is protected and therefore read-only..
        /// </summary>
        public static string SheetViewDragDropChangeProtectRow
        {
            get { return Loader.GetString("SheetViewDragDropChangeProtectRow"); }
        }

        /// <summary>
        /// Looks up a localized string similar to This operation is not allowed. The operation is attempting to shift cells in a table on your worksheet..
        /// </summary>
        public static string SheetViewDragDropShiftTableCell
        {
            get { return Loader.GetString("SheetViewDragDropShiftTableCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot fill a range that contains a merged cell..
        /// </summary>
        public static string SheetViewDragFillChangePartOfMergeCell
        {
            get { return Loader.GetString("SheetViewDragFillChangePartOfMergeCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The cells you are trying to fill are protected and therefore read-only..
        /// </summary>
        public static string SheetViewDragFillChangeProtectCell
        {
            get { return Loader.GetString("SheetViewDragFillChangeProtectCell"); }
        }

        public static string SheetViewDragFillInvalidOperation
        {
            get { return Loader.GetString("SheetViewDragFillInvalidOperation"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of a merged cell..
        /// </summary>
        public static string SheetViewPasteChangeMergeCell
        {
            get { return Loader.GetString("SheetViewPasteChangeMergeCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of an array..
        /// </summary>
        public static string SheetViewPasteChangePartOfArrayFormula
        {
            get { return Loader.GetString("SheetViewPasteChangePartOfArrayFormula"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The cell you are trying to change is protected and therefore read-only..
        /// </summary>
        public static string SheetViewPasteDestinationSheetCellsAreLocked
        {
            get { return Loader.GetString("SheetViewPasteDestinationSheetCellsAreLocked"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Source sheet's cells are locked..
        /// </summary>
        public static string SheetViewPasteSouceSheetCellsAreLocked
        {
            get { return Loader.GetString("SheetViewPasteSouceSheetCellsAreLocked"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The copy and paste areas are not the same size..
        /// </summary>
        public static string SheetViewTheCopyAreaAndPasteAreaAreNotTheSameSize
        {
            get { return Loader.GetString("SheetViewTheCopyAreaAndPasteAreaAreNotTheSameSize"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Cannot change part of an array..
        /// </summary>
        public static string SortCommandInvalidOperation
        {
            get { return Loader.GetString("SortCommandInvalidOperation"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Sort Ascend.
        /// </summary>
        public static string SortDropdownItemSortAscend
        {
            get { return Loader.GetString("SortDropdownItemSortAscend"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Sort Descend.
        /// </summary>
        public static string SortDropdownItemSortDescend
        {
            get { return Loader.GetString("SortDropdownItemSortDescend"); }
        }

        /// <summary>
        /// Looks up a localized string similar to That command cannot be used on multiple selections..
        /// </summary>
        public static string spreadActionCopyMultiplySelection
        {
            get { return Loader.GetString("spreadActionCopyMultiplySelection"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The command you chose cannot be performed with multiple selections.
        /// Select a single range and click the command again..
        /// </summary>
        public static string spreadActionCutMultipleSelections
        {
            get { return Loader.GetString("spreadActionCutMultipleSelections"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The pasted area should have the same size as the copy or cut area..
        /// </summary>
        public static string spreadActionPasteSizeDifferent
        {
            get { return Loader.GetString("spreadActionPasteSizeDifferent"); }
        }

        /// <summary>
        /// Looks up a localized string similar to New....
        /// </summary>
        public static string TabStrip_NewSheet
        {
            get { return Loader.GetString("TabStrip_NewSheet"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Copy Cells.
        /// </summary>
        public static string UIFill_CopyCells
        {
            get { return Loader.GetString("UIFill_CopyCells"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Fill Formatting Only.
        /// </summary>
        public static string UIFill_FillFormattingOnly
        {
            get { return Loader.GetString("UIFill_FillFormattingOnly"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Fill Series.
        /// </summary>
        public static string UIFill_FillSeries
        {
            get { return Loader.GetString("UIFill_FillSeries"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Fill Without Formatting.
        /// </summary>
        public static string UIFill_FillWithOutFormatting
        {
            get { return Loader.GetString("UIFill_FillWithOutFormatting"); }
        }

        /// <summary>
        /// Looks up a localized string similar to None.
        /// </summary>
        public static string UIFill_None
        {
            get { return Loader.GetString("UIFill_None"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Action failed..
        /// </summary>
        public static string undoActionActionFailed
        {
            get { return Loader.GetString("undoActionActionFailed"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row: {0}.
        /// </summary>
        public static string undoActionArrayFormula
        {
            get { return Loader.GetString("undoActionArrayFormula"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Auto Fill.
        /// </summary>
        public static string undoActionAutoFill
        {
            get { return Loader.GetString("undoActionAutoFill"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The formula '{0}' cannot be applied..
        /// </summary>
        public static string undoActionCannotApplyFormula
        {
            get { return Loader.GetString("undoActionCannotApplyFormula"); }
        }

        /// <summary>
        /// Looks up a localized string similar to The value cannot be applied..
        /// </summary>
        public static string undoActionCannotApplyValue
        {
            get { return Loader.GetString("undoActionCannotApplyValue"); }
        }

        /// <summary>
        /// Looks up a localized string similar to This validation flag is not supported..
        /// </summary>
        public static string undoActionCellEditInvalidValidationFlag
        {
            get { return Loader.GetString("undoActionCellEditInvalidValidationFlag"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Clear.
        /// </summary>
        public static string undoActionClear
        {
            get { return Loader.GetString("undoActionClear"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Clipboard Paste.
        /// </summary>
        public static string undoActionClipboardPaste
        {
            get { return Loader.GetString("undoActionClipboardPaste"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column AutoFit.
        /// </summary>
        public static string undoActionColumnAutoFit
        {
            get { return Loader.GetString("undoActionColumnAutoFit"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column Group.
        /// </summary>
        public static string undoActionColumnGroup
        {
            get { return Loader.GetString("undoActionColumnGroup"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column Group Expand.
        /// </summary>
        public static string undoActionColumnGroupExpand
        {
            get { return Loader.GetString("undoActionColumnGroupExpand"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column Group Header Expand.
        /// </summary>
        public static string undoActionColumnGroupHeaderExpand
        {
            get { return Loader.GetString("undoActionColumnGroupHeaderExpand"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column Resize.
        /// </summary>
        public static string undoActionColumnResize
        {
            get { return Loader.GetString("undoActionColumnResize"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Column Ungroup.
        /// </summary>
        public static string undoActionColumnUngroup
        {
            get { return Loader.GetString("undoActionColumnUngroup"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Drag Drop.
        /// </summary>
        public static string undoActionDragDrop
        {
            get { return Loader.GetString("undoActionDragDrop"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Editing Cell.
        /// </summary>
        public static string undoActionEditingCell
        {
            get { return Loader.GetString("undoActionEditingCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Target ranges cannot be null or empty..
        /// </summary>
        public static string undoActionPasteTargetEmptyError
        {
            get { return Loader.GetString("undoActionPasteTargetEmptyError"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row AutoFit.
        /// </summary>
        public static string undoActionRowAutoFit
        {
            get { return Loader.GetString("undoActionRowAutoFit"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row Group.
        /// </summary>
        public static string undoActionRowGroup
        {
            get { return Loader.GetString("undoActionRowGroup"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row Group Expand.
        /// </summary>
        public static string undoActionRowGroupExpand
        {
            get { return Loader.GetString("undoActionRowGroupExpand"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row Group Header Expand.
        /// </summary>
        public static string undoActionRowGroupHeaderExpand
        {
            get { return Loader.GetString("undoActionRowGroupHeaderExpand"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row Resize.
        /// </summary>
        public static string undoActionRowResize
        {
            get { return Loader.GetString("undoActionRowResize"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row Ungroup.
        /// </summary>
        public static string undoActionRowUngroup
        {
            get { return Loader.GetString("undoActionRowUngroup"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Sheet Rename.
        /// </summary>
        public static string undoActionSheetRename
        {
            get { return Loader.GetString("undoActionSheetRename"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Typing '{0}'  in {1}.
        /// </summary>
        public static string undoActionTypingInCell
        {
            get { return Loader.GetString("undoActionTypingInCell"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Zoom.
        /// </summary>
        public static string undoActionZoom
        {
            get { return Loader.GetString("undoActionZoom"); }
        }

        /// <summary>
        /// Looks up a localized string similar to Row: {0}.
        /// </summary>
        public static string VerticalScroll
        {
            get { return Loader.GetString("VerticalScroll"); }
        }

        public static ResourceLoader Loader
        {
            get
            {
                if (_loader == null)
                {
                    // WinUI
                    _loader = ResourceLoader.GetForViewIndependentUse("Dt.Cells.Data/Resources");
                }
                return _loader;
            }
        }
    }
}

