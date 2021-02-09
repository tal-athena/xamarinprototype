using System.ComponentModel;


namespace DecisionsMobile.Constants
{
    /// <summary>
    /// Action Type 
    /// </summary>
    /// <remarks>
    /// FIXME should depend on DecisionsFramework.ServiceLayer.Actions.EntityActionType
    /// from a DLL instead of duplicicating that code here
    /// </remarks>
    public enum EntityActionType
    {
        [Description("Silverlight")]
        Silverlight = 0,
        [Description("ASP.NET Webform")]
        WebForm = 2,
        [Description("Command Line")]
        CLI = 3,
        [Description("Windows Form")]
        WindowsFormsOffline = 4,
        [Description("HTML")]
        Mvc = 5,
        [Description("HTML Mobile")]
        MvcMobile = 6,
        [Description("HTML Tablet")]
        MvcTablet = 7,
    }
}
