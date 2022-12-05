using System;
using System.IO;
using System.Reflection;

namespace AdventOfCodeTests;

public static class FileHelper
{
    public static string ReadFromFile(string folder, string filename)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var embeddedResourceName = $"AdventOfCodeTests.{folder}.{filename}";
        using var stream = assembly.GetManifestResourceStream(embeddedResourceName);
        if (stream is null)
        {
            throw new Exception($"Resource not found {filename}");
        }
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}