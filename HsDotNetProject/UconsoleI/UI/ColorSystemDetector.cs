using System.Runtime.InteropServices;

namespace UconsoleI.UI;

internal static class ColorSystemDetector
{
    public static ColorSystem Detect()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows 10.0.15063 and above support true color,
            if (!GetWindowsVersionInformation(out var major, out var build)) return ColorSystem.EightBit;
            switch (major)
            {
                case 10 when build >= 15063:
                case > 10:
                    return ColorSystem.TrueColor;
            }
        }
        else
        {
            return ColorSystem.TrueColor;
        }

        return ColorSystem.EightBit;
    }

    private static bool GetWindowsVersionInformation(out int major, out int build)
    {
        major = 0;
        build = 0;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return false;

        var version = Environment.OSVersion.Version;
        major = version.Major;
        build = version.Build;
        return true;
    }
}