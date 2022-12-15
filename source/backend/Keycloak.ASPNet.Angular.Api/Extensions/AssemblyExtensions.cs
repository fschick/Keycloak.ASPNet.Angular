using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Keycloak.ASPNet.Angular.Api.Extensions;

/// <summary>
/// Extensions methods for type <see cref="Assembly"></see>
/// </summary>
internal static class AssemblyExtensions
{
    #region Path
    /// <summary>
    /// Full path of the current executable.
    /// </summary>
    public static string GetProgramPath()
        => Assembly.GetEntryAssembly().GetAssemblyPath();

    /// <summary>
    /// Full path of the assembly calling this method.
    /// </summary>
    public static string GetLibraryPath()
        => Assembly.GetCallingAssembly().GetAssemblyPath();

    /// <summary>
    /// Gets the full path of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the full path for.</param>
    public static string GetAssemblyPath(this Assembly assembly)
        => assembly.Location;
    #endregion

    #region Directory
    /// <summary>
    /// Gets the directory of the current executable.
    /// </summary>
    public static string GetProgramDirectory()
        => Assembly.GetEntryAssembly().GetAssemblyDirectory();

    /// <summary>
    /// Gets the directory of the assembly calling this method.
    /// </summary>
    public static string GetLibraryDirectory()
        => Assembly.GetCallingAssembly().GetAssemblyDirectory();

    /// <summary>
    /// Gets the directory of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the directory for</param>
    public static string GetAssemblyDirectory(this Assembly assembly)
        => Path.GetDirectoryName(assembly.Location);
    #endregion

    #region Title
    /// <summary>
    /// Gets the title of the current executable.
    /// </summary>
    /// <returns>The title of the current executable.</returns>
    public static string GetProgramTitle()
        => Assembly.GetEntryAssembly().GetAssemblyTitle();

    /// <summary>
    /// Gets the title of the assembly calling this method.
    /// </summary>
    /// <returns>The title of the assembly calling this method</returns>
    public static string GetLibraryTitle()
        => Assembly.GetCallingAssembly().GetAssemblyTitle();

    /// <summary>
    /// Gets the title of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the title for</param>
    /// <returns>The title of the assembly</returns>
    public static string GetAssemblyTitle(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyTitleAttribute>()?.Title;
    #endregion

    #region Product
    /// <summary>
    /// Gets the product of the current executable.
    /// </summary>
    /// <returns>The product of the current executable.</returns>
    public static string GetProgramProduct()
        => Assembly.GetEntryAssembly().GetAssemblyProduct();

    /// <summary>
    /// Gets the product of the assembly calling this method.
    /// </summary>
    /// <returns>The product of the assembly calling this method</returns>
    public static string GetLibraryProduct()
        => Assembly.GetCallingAssembly().GetAssemblyProduct();

    /// <summary>
    /// Gets the product of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the product for</param>
    /// <returns>The product of the assembly</returns>
    public static string GetAssemblyProduct(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyProductAttribute>()?.Product;
    #endregion

    #region Description
    /// <summary>
    /// Gets the description of the current executable.
    /// </summary>
    /// <returns>The description of the current executable.</returns>
    public static string GetProgramDescription()
        => Assembly.GetEntryAssembly().GetAssemblyDescription();

    /// <summary>
    /// Gets the description of the assembly calling this method.
    /// </summary>
    /// <returns>The description of the assembly calling this method</returns>
    public static string GetLibraryDescription()
        => Assembly.GetCallingAssembly().GetAssemblyDescription();

    /// <summary>
    /// Gets the description of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the description for</param>
    /// <returns>The description of the assembly</returns>
    public static string GetAssemblyDescription(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyDescriptionAttribute>()?.Description;
    #endregion

    #region Company
    /// <summary>
    /// Gets the company of the current executable.
    /// </summary>
    /// <returns>The company of the current executable.</returns>
    public static string GetProgramCompany()
        => Assembly.GetEntryAssembly().GetAssemblyCompany();

    /// <summary>
    /// Gets the company of the assembly calling this method.
    /// </summary>
    /// <returns>The company of the assembly calling this method</returns>
    public static string GetLibraryCompany()
        => Assembly.GetCallingAssembly().GetAssemblyCompany();

    /// <summary>
    /// Gets the company of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the company for</param>
    /// <returns>The company of the assembly</returns>
    public static string GetAssemblyCompany(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyCompanyAttribute>()?.Company;
    #endregion

    #region Copyright
    /// <summary>
    /// Gets the copyright of the current executable.
    /// </summary>
    /// <returns>The copyright of the current executable.</returns>
    public static string GetProgramCopyright()
        => Assembly.GetEntryAssembly().GetAssemblyCopyright();

    /// <summary>
    /// Gets the copyright of the assembly calling this method.
    /// </summary>
    /// <returns>The copyright of the assembly calling this method</returns>
    public static string GetLibraryCopyright()
        => Assembly.GetCallingAssembly().GetAssemblyCopyright();

    /// <summary>
    /// Gets the copyright of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the copyright for</param>
    /// <returns>The copyright of the assembly</returns>
    public static string GetAssemblyCopyright(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyCopyrightAttribute>()?.Copyright;
    #endregion

    #region Trademark
    /// <summary>
    /// Gets the trademark of the current executable.
    /// </summary>
    /// <returns>The trademark of the current executable.</returns>
    public static string GetProgramTrademark()
        => Assembly.GetEntryAssembly().GetAssemblyTrademark();

    /// <summary>
    /// Gets the trademark of the assembly calling this method.
    /// </summary>
    /// <returns>The trademark of the assembly calling this method</returns>
    public static string GetLibraryTrademark()
        => Assembly.GetCallingAssembly().GetAssemblyTrademark();

    /// <summary>
    /// Gets the trademark of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the trademark for</param>
    /// <returns>The trademark of the assembly</returns>
    public static string GetAssemblyTrademark(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyTrademarkAttribute>()?.Trademark;
    #endregion

    #region Culture
    /// <summary>
    /// Gets the culture of the current executable.
    /// </summary>
    /// <returns>The culture of the current executable.</returns>
    public static string GetProgramCulture()
        => Assembly.GetEntryAssembly().GetAssemblyCulture();

    /// <summary>
    /// Gets the culture of the assembly calling this method.
    /// </summary>
    /// <returns>The culture of the assembly calling this method</returns>
    public static string GetLibraryCulture()
        => Assembly.GetCallingAssembly().GetAssemblyCulture();

    /// <summary>
    /// Gets the culture of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the culture for</param>
    /// <returns>The culture of the assembly</returns>
    public static string GetAssemblyCulture(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyCultureAttribute>()?.Culture;
    #endregion

    #region ProductVersion
    /// <summary>
    /// Gets the product version of the current executable.
    /// </summary>
    /// <returns>The product version of the current executable.</returns>
    public static string GetProgramProductVersion()
        => Assembly.GetEntryAssembly().GetAssemblyProductVersion();

    /// <summary>
    /// Gets the product version of the assembly calling this method.
    /// </summary>
    /// <returns>The product version of the assembly calling this method</returns>
    public static string GetLibraryProductVersion()
        => Assembly.GetCallingAssembly().GetAssemblyProductVersion();

    /// <summary>
    /// Gets the product version of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the product version for</param>
    /// <returns>The product version of the assembly</returns>
    public static string GetAssemblyProductVersion(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    #endregion

    #region AssemblyVersion
    /// <summary>
    /// Gets the assembly version of the current executable.
    /// </summary>
    /// <returns>The assembly version of the current executable.</returns>
    public static string GetProgramAssemblyVersion()
        => Assembly.GetEntryAssembly().GetAssemblyAssemblyVersion();

    /// <summary>
    /// Gets the assembly version of the assembly calling this method.
    /// </summary>
    /// <returns>The assembly version of the assembly calling this method</returns>
    public static string GetLibraryAssemblyVersion()
        => Assembly.GetCallingAssembly().GetAssemblyAssemblyVersion();

    /// <summary>
    /// Gets the assembly version of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the assembly version for</param>
    /// <returns>The assembly version of the assembly</returns>
    public static string GetAssemblyAssemblyVersion(this Assembly assembly)
        => assembly?.GetName().Version.ToString();
    #endregion

    #region FileVersion
    /// <summary>
    /// Gets the file version of the current executable.
    /// </summary>
    /// <returns>The file version of the current executable.</returns>
    public static string GetProgramFileVersion()
        => Assembly.GetEntryAssembly().GetAssemblyFileVersion();

    /// <summary>
    /// Gets the file version of the assembly calling this method.
    /// </summary>
    /// <returns>The file version of the assembly calling this method</returns>
    public static string GetLibraryFileVersion()
        => Assembly.GetCallingAssembly().GetAssemblyFileVersion();

    /// <summary>
    /// Gets the file version of a given assembly.
    /// </summary>
    /// <param name="assembly">The assembly to get the file version for</param>
    /// <returns>The file version of the assembly</returns>
    public static string GetAssemblyFileVersion(this Assembly assembly)
        => assembly.FirstOrDefaultAttribute<AssemblyFileVersionAttribute>()?.Version;
    #endregion

    private static TAttribute FirstOrDefaultAttribute<TAttribute>(this ICustomAttributeProvider assembly) where TAttribute : Attribute
        => assembly?
            .GetCustomAttributes(typeof(TAttribute), true)
            .OfType<TAttribute>()
            .FirstOrDefault();
}