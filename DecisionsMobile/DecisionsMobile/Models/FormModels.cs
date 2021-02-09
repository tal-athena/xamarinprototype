using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace DecisionsMobile.Models.FormService
{

    public partial class StandAloneFormSessionInfo
    {
        [JsonProperty("__type")]
        public string ServerType { get; set; }

        [JsonProperty("documents")]
        public object[] Documents { get; set; }

        [JsonProperty("formSessionInfoID")]
        public string FormSessionInfoId { get; set; }

        [JsonProperty("formSurface")]
        public FormSurface FormSurface { get; set; }

        [JsonProperty("formRules")]
        public object[] FormRules { get; set; }
        [JsonProperty("initialControlsData")]
        public DataPair[] InitialControlsData { get; set; }
        [JsonProperty("requiredFields", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> RequiredFields { get; set; } = new Dictionary<string, object>();
    }

    public partial class FormSurface
    {
        [JsonProperty("__type")]
        public string ServerType { get; set; }

        [JsonProperty("allowRuntimeCSSFileName")]
        public bool AllowRuntimeCssFileName { get; set; }

        [JsonProperty("cssDocumentIds")]
        public object[] CssDocumentIds { get; set; }

        [JsonProperty("runOutcomeRulesAtStartup")]
        public object[] RunOutcomeRulesAtStartup { get; set; }

        [JsonProperty("rootContainer")]
        public ComponentContainer RootContainer { get; set; }
    }

    public partial class ChildElement
    {
        [JsonProperty("$type")]
        public string ServerType { get; set; }

        [JsonProperty("column")]
        public int Column { get; set; }

        [JsonProperty("columnSpan")]
        public int ColumnSpan { get; set; }

        [JsonProperty("row")]
        public int Row { get; set; }

        [JsonProperty("rowSpan")]
        public int RowSpan { get; set; }

        [JsonProperty("componentId")]
        public string ComponentId { get; set; }

        [JsonProperty("child", NullValueHandling = NullValueHandling.Ignore)]
        public ComponentContainer Child { get; set; }
    }

    public partial class ComponentContainer
    {
        [JsonProperty("__type")]
        public string ServerType { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("overrideBackgroundColor", NullValueHandling = NullValueHandling.Ignore)]
        public bool? OverrideBackgroundColor { get; set; }

        [JsonProperty("dataName", NullValueHandling = NullValueHandling.Ignore)]
        public string DataName { get; set; }

        [JsonProperty("dataNameRequired", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DataNameRequired { get; set; }

        [JsonProperty("specifyVisibility", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SpecifyVisibility { get; set; }

        [JsonProperty("visiblityDataName", NullValueHandling = NullValueHandling.Ignore)]
        public string VisiblityDataName { get; set; }

        [JsonProperty("textFont", NullValueHandling = NullValueHandling.Ignore)]
        public Attributes TextFont { get; set; }

        [JsonProperty("needsConfirm", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NeedsConfirm { get; set; }

        [JsonProperty("confirmationMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmationMessage { get; set; }

        [JsonProperty("isConfirmMessageFromDataName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsConfirmMessageFromDataName { get; set; }

        [JsonProperty("confirmMessageDataName", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmMessageDataName { get; set; }

        [JsonProperty("confirmDialogYesButtonText", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmDialogYesButtonText { get; set; }

        [JsonProperty("isYesButtonTextFromDataName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsYesButtonTextFromDataName { get; set; }

        [JsonProperty("confirmDialogNoButtonText", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfirmDialogNoButtonText { get; set; }

        [JsonProperty("isNoButtonTextFromDataName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsNoButtonTextFromDataName { get; set; }

        [JsonProperty("displayText", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayText { get; set; }

        [JsonProperty("considerVisibility", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ConsiderVisibility { get; set; }

        [JsonProperty("isButtonVisibleBasedOnDataName", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsButtonVisibleBasedOnDataName { get; set; }

        [JsonProperty("componentName")]
        public string ComponentName { get; set; }

        [JsonProperty("outcomePathName", NullValueHandling = NullValueHandling.Ignore)]
        public string OutcomePathName { get; set; }

        [JsonProperty("syncTextAndPath", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SyncTextAndPath { get; set; }

        [JsonProperty("overrideRequiredMessage", NullValueHandling = NullValueHandling.Ignore)]
        public bool OverrideRequiredMessage { get; set; }
        [JsonProperty("requiredMessage", NullValueHandling = NullValueHandling.Ignore)]
        public string RequiredMessage { get; set; }

        [JsonProperty("allDataNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] AllDataNames { get; set; }

        [JsonProperty("componentId")]
        public string ComponentId { get; set; }

        [JsonProperty("tabOrder")]
        public long TabOrder { get; set; }

        [JsonProperty("parentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid ParentId { get; set; }

        [JsonProperty("requestedMinHeight")]
        public double RequestedMinHeight { get; set; }

        [JsonProperty("requestedMinWidth")]
        public double RequestedMinWidth { get; set; }

        [JsonProperty("requestedMaxHeight")]
        public double RequestedMaxHeight { get; set; }

        [JsonProperty("requestedMaxWidth")]
        public double RequestedMaxWidth { get; set; }

        [JsonProperty("requestedHeight")]
        public double RequestedHeight { get; set; }

        [JsonProperty("requestedWidth")]
        public double RequestedWidth { get; set; }

        [JsonProperty("initiallyVisible")]
        public bool InitiallyVisible { get; set; }

        [JsonProperty("initiallyEnabled")]
        public bool InitiallyEnabled { get; set; }

        [JsonProperty("isInDesignMode")]
        public bool IsInDesignMode { get; set; }

        [JsonProperty("isViewOnlyMode")]
        public bool IsViewOnlyMode { get; set; }

        [JsonProperty("staticInput", NullValueHandling = NullValueHandling.Ignore)]
        public bool? StaticInput { get; set; }

        [JsonProperty("maximumNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long? MaximumNumber { get; set; }

        [JsonProperty("autoSelectOnFocus", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AutoSelectOnFocus { get; set; }

        [JsonProperty("watermarkText", NullValueHandling = NullValueHandling.Ignore)]
        public string WatermarkText { get; set; }

        [JsonProperty("labelAttributes", NullValueHandling = NullValueHandling.Ignore)]
        public Attributes LabelAttributes { get; set; }

        [JsonProperty("raiseValueChangeEventOnlyOnExit", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RaiseValueChangeEventOnlyOnExit { get; set; }

        [JsonProperty("requiredOnOutputs", NullValueHandling = NullValueHandling.Ignore)]
        public string[] RequiredOnOutputs { get; set; } = new string[0];

        [JsonProperty("outcomeDataNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] OutcomeDataNames { get; set; }

        [JsonProperty("syncableDataNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] SyncableDataNames { get; set; }

        [JsonProperty("textBehaviour", NullValueHandling = NullValueHandling.Ignore)]
        public long? TextBehaviour { get; set; }

        [JsonProperty("isCopyable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCopyable { get; set; }

        [JsonProperty("validationSourceComponentId", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ValidationSourceComponentId { get; set; }

        [JsonProperty("labelAsterisk", NullValueHandling = NullValueHandling.Ignore)]
        public long? LabelAsterisk { get; set; }

        [JsonProperty("asteriskPosition", NullValueHandling = NullValueHandling.Ignore)]
        public long? AsteriskPosition { get; set; }

        [JsonProperty("attributes", NullValueHandling = NullValueHandling.Ignore)]
        public Attributes Attributes { get; set; }

        [JsonProperty("labelAlignment", NullValueHandling = NullValueHandling.Ignore)]
        public long? LabelAlignment { get; set; }

        [JsonProperty("labelVerticalAlignment", NullValueHandling = NullValueHandling.Ignore)]
        public long? LabelVerticalAlignment { get; set; }

        [JsonProperty("defaultValue", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultValue { get; set; }

        [JsonProperty("isCaseSensitive", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCaseSensitive { get; set; }

        [JsonProperty("isUsingTypingTextInFlowInput", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsUsingTypingTextInFlowInput { get; set; }

        [JsonProperty("dependentDataNames", NullValueHandling = NullValueHandling.Ignore)]
        public object[] DependentDataNames { get; set; }

        [JsonProperty("allowExternalValuesToBeSelected", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowExternalValuesToBeSelected { get; set; }

        [JsonProperty("selectedItemDataName", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedItemDataName { get; set; }

        [JsonProperty("selectedItemDataNameKey", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedItemDataNameKey { get; set; }

        [JsonProperty("selectedExternalItemDataNameKey", NullValueHandling = NullValueHandling.Ignore)]
        public string SelectedExternalItemDataNameKey { get; set; }

        [JsonProperty("turnOffAllSuggestions", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TurnOffAllSuggestions { get; set; }

        [JsonProperty("turnOffDatePicker", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TurnOffDatePicker { get; set; }

        [JsonProperty("dayNamesMin", NullValueHandling = NullValueHandling.Ignore)]
        public string[] DayNamesMin { get; set; }

        [JsonProperty("dayNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] DayNames { get; set; }

        [JsonProperty("monthNames", NullValueHandling = NullValueHandling.Ignore)]
        public string[] MonthNames { get; set; }

        [JsonProperty("monthNamesShort", NullValueHandling = NullValueHandling.Ignore)]
        public string[] MonthNamesShort { get; set; }

        [JsonProperty("todayLinkText", NullValueHandling = NullValueHandling.Ignore)]
        public string TodayLinkText { get; set; }

        [JsonProperty("clearLinkText", NullValueHandling = NullValueHandling.Ignore)]
        public string ClearLinkText { get; set; }

        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }

        [JsonProperty("imageInfo", NullValueHandling = NullValueHandling.Ignore)]
        public ImageInfo ImageInfo { get; set; }

        [JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
        public double? Min { get; set; }

        [JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
        public double? Max { get; set; }

        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public ChildElement[] Children { get; set; }

        [JsonProperty("clientChildrenInfo", NullValueHandling =NullValueHandling.Ignore)]
        public ChildElement[] ClientChildrenInfo { get; set; }

        [JsonProperty("columns", NullValueHandling = NullValueHandling.Ignore)]
        public GridLayoutPart[] Columns { get; set; }

        [JsonProperty("rows", NullValueHandling = NullValueHandling.Ignore)]
        public GridLayoutPart[] Rows { get; set; }

        [JsonProperty("rowGap", NullValueHandling = NullValueHandling.Ignore)]
        public long RowGap { get; set; }

        [JsonProperty("columnGap", NullValueHandling = NullValueHandling.Ignore)]
        public long ColumnGap { get; set; }

        [JsonProperty("marginTop", NullValueHandling = NullValueHandling.Ignore)]
        public long MarginTop { get; set; }

        [JsonProperty("marginLeft", NullValueHandling = NullValueHandling.Ignore)]
        public long MarginLeft { get; set; }

        [JsonProperty("marginRight", NullValueHandling = NullValueHandling.Ignore)]
        public long MarginRight { get; set; }
   
        [JsonProperty("marginBottom", NullValueHandling = NullValueHandling.Ignore)]
        public long MarginBottom { get; set; }

        [JsonProperty("backgroundType", NullValueHandling = NullValueHandling.Ignore)]
        public long BackgroundType { get; set; }

        [JsonProperty("transparentBackground", NullValueHandling = NullValueHandling.Ignore)]
        public bool TransparentBackground { get; set; }

        [JsonProperty("backgroundAlignmentX", NullValueHandling = NullValueHandling.Ignore)]
        public long BackgroundAlignmentX { get; set; }

        [JsonProperty("backgroundAlignmentY", NullValueHandling = NullValueHandling.Ignore)]
        public long BackgroundAlignmentY { get; set; }

        [JsonProperty("backgroundStretch", NullValueHandling = NullValueHandling.Ignore)]
        public bool BackgroundStretch { get; set; }

        [JsonProperty("designerCornerRadius", NullValueHandling = NullValueHandling.Ignore)]
        public long DesignerCornerRadius { get; set; }

        [JsonProperty("borderWidth", NullValueHandling = NullValueHandling.Ignore)]
        public long BorderWidth { get; set; }

        [JsonProperty("minimum", NullValueHandling = NullValueHandling.Ignore)]
        public double Minimum { get; set; }

        [JsonProperty("maximum", NullValueHandling = NullValueHandling.Ignore)]
        public double Maximum { get; set; }

        [JsonProperty("outputType", NullValueHandling = NullValueHandling.Ignore)]
        public byte OutputType { get; set; }

    }

    public partial class Attributes
    {
        [JsonProperty("FontFamily")]
        public string FontFamily { get; set; }

        [JsonProperty("FontSize")]
        public long FontSize { get; set; }

        [JsonProperty("LabelColor")]
        public DesignerColor LabelColor { get; set; }

        [JsonProperty("IsBold")]
        public bool IsBold { get; set; }

        [JsonProperty("IsItalic")]
        public bool IsItalic { get; set; }

        [JsonProperty("IsUnderlined")]
        public bool IsUnderlined { get; set; }
    }

    public enum NumberBoxOutputType
    {
        Double = 0,
        Int = 1,
        Decimal = 2
    }

    public enum ImageInfoType
    {

        // [Description("File")]
        RawData,

        // [Description("URL")]
        Url,

        // [Description("Stored Image")]
        StoredImage,

        // [Description("Document")]
        Document
    }

    public partial class DesignerColor
    {
        [JsonProperty("Opacity")]
        public long Opacity { get; set; }

        [JsonProperty("ColorName")]
        public string ColorName { get; set; }

        [JsonProperty("GradientDirection")]
        public long GradientDirection { get; set; }

        [JsonProperty("Type")]
        public long LabelColorType { get; set; }
    }

    public partial class GridLayoutPart
    {
        [JsonProperty("$type")]
        public string ServerType { get; set; }

        [JsonProperty("minSize")]
        public double MinSize { get; set; }

        [JsonProperty("maxSize")]
        public double MaxSize { get; set; }

        [JsonProperty("requestedSize")]
        public double RequestedSize { get; set; }

        [JsonProperty("layoutType")]
        public LayoutElementType LayoutType { get; set; }
    }

    public enum RequestedMaxHeight { NaN };

    public enum LayoutElementType
    {
        Fixed,  // GridUnitType.Absolute 
        Grow,   // GridUnitType.Auto
        Resize  // GridUnitType.Star
    }

    //public class ValidationChangedEvent {

    //    [JsonProperty("__type")]
    //    public string ServerType = "ValidationChangedEvent:#DecisionsFramework.Design.Form";

    //    [JsonProperty]
    //    public string ParentComponentId { get; set; }

    //    [JsonProperty]
    //    public string SurfaceId { get; set; }

    //    public CurrentFormValidations[] CurrentFormValidations { get; set; }
    //}

    public class FormValidations
    {

        [JsonProperty]
        public string ComponentID { get; set; }

        [JsonProperty]
        public ValidationIssue[] ValidationIssues { get; set; }
    }

    public class OfflineFormSubmission
    {
        public DataPair[] ControlsData { get; set; }

        public object[] RuleSessionInfos { get; set; } // TODO rules

        public string OutcomeName { get; set; }
    }

    public class GetAllOfflineFormSurfacesResultWrapper
    {
        public Dictionary<string, IEnumerable<string>> GetAllOfflineFormSurfacesResult { get; set; } = new Dictionary<string, IEnumerable<string>>();
    }

}
