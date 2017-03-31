using Windows.ApplicationModel.Resources.Core;

namespace SoftwareKobo.Social.SinaWeibo.Helpers
{
    internal static class DeviceFamilyHelper
    {
        internal static bool IsMobile
        {
            get
            {
                var qualifiers = ResourceContext.GetForCurrentView().QualifierValues;
                return qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile";
            }
        }
    }
}