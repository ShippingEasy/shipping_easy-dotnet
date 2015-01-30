using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
// ReSharper disable CheckNamespace

[assembly: AssemblyTitle("ShippingEasyDotNet")]
[assembly: AssemblyDescription("ShippingEasy API Client")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ShippingEasy")]
[assembly: AssemblyProduct("ShippingEasy")]
[assembly: AssemblyCopyright("Copyright ©ShippingEasy 2015")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: Guid("ea7b2ec4-35c4-4c63-b7cf-ab2a5591cc54")]

// AssemblyVersion should be manually updated for an official release
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

internal static class AssemblyInfo
{
    private static readonly Assembly _assembly;

    static AssemblyInfo()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public static string GetAssemblyName()
    {
        return _assembly.GetName().Name;
    }

    public static string GetAssemblyVersion()
    {
        return _assembly.GetName().Version.ToString();
    }

    public static string GetFileVersion()
    {
        return GetAssemblyAttributeValue<AssemblyFileVersionAttribute>(a => a.Version);
    }

    public static string GetCommitIdentifier()
    {
        return GetAssemblyAttributeValue<AssemblyTrademarkAttribute>(a => a.Trademark);
    }

    private static string GetAssemblyAttributeValue<T>(Func<T, string> property)
    {
        var attr = _assembly.GetCustomAttributes(typeof(T), false)
            .Cast<T>()
            .FirstOrDefault();
        return attr != null ? property(attr) : null;
    }
}