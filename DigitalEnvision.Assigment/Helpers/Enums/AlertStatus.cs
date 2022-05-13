using System.ComponentModel;

namespace DigitalEnvision.Assigment.Helpers.Enums
{
    public enum AlertStatus
    {
        [Description("New")]
        New = 1,
        [Description("Process")]
        Process,
        [Description("Success")]
        Success,
        [Description("Failed")]
        Failed
    }
}
