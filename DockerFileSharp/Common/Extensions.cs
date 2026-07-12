using System.Runtime.InteropServices;
using System.Text;

namespace DockerFileSharp.Common;

public static class Extensions 
{
    public static bool IsEmpty(this string? value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Extension method used to create a new line seperated string based on the provided list of instructions.
    /// </summary>
    public static string Build(this List<IDockerInstruction> instructions) {
        var builder = new StringBuilder();
        instructions.ForEach(i => builder.AppendLine(i.Build()));
        return builder.ToString();
    }
}
