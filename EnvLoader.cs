namespace Filmoteka;

public static class EnvLoader
{
    public static void Load(string fileName = ".env")
    {
        string? path = FindFile(fileName);
        if (path is null)
        {
            return;
        }

        foreach (string line in File.ReadAllLines(path))
        {
            string trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
            {
                continue;
            }

            int separatorIndex = trimmed.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            string key = trimmed[..separatorIndex].Trim();
            string value = trimmed[(separatorIndex + 1)..].Trim().Trim('"');
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    private static string? FindFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            return Path.GetFullPath(fileName);
        }

        DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            string candidate = Path.Combine(directory.FullName, fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        return null;
    }
}
