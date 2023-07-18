using System.ComponentModel;

namespace APForums.Server.Models.Types
{
    public enum School
    {
        [Description("Computing, Technology & Games Developmenmt")]
        Computing,
        [Description("Design, Advertising, Animation & VFX")]
        Design,
        [Description("Business, Management, Marketing & Tourism")]
        Business,
        [Description("Accounting, Banking, Finance & Acturial")]
        Accounting,
        [Description("Engineering")]
        Engineering,
        [Description("Media, International Relations & Psychology")]
        Media
    }
}
