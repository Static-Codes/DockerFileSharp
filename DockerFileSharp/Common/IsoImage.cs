namespace DockerFileSharp.Common;
using System.Runtime.InteropServices;
public class IsoImage(string FullName, string Version, string[] SupportedArchitectures)
{
    /// <summary> Represents the image used in a FROM command. </summary>
    /// <param name="FullName">
    ///     The fullname of the image. <br/>
    ///     Example: Debian 13.3 will be converted to "debian:13.3" in a FROM command.
    /// </param>
    /// <param name="Version">
    ///     The version for the image. <br/>
    ///     Example: If FullName is specified as "Debian 13.3", the version should be "13.3"
    /// </param>
    /// <param name="SupportedArchitectures">
    ///     An array of architectures the image supports. <br/>
    ///     Commonly supported architectures include: <br/>
    ///     - linux/amd64   <br/>
    ///     - linux/arm64   <br/>
    ///     - linux/arm/v7  <br/>
    ///     - linux/arm/v6  <br/> 
    ///     - linux/ppc64le <br/>
    ///     - linux/s390x   <br/>
    ///     - linux/riscv64 <br/>
    ///     - linux/386"    <br/>
    /// </param>
    /// 
    public string FullName { get; set; } = FullName;

    public string Version { get; set; } = Version;

    public string[] SupportedArchitectures { get; set; } = SupportedArchitectures;
}
