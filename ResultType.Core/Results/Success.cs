using System.Runtime.InteropServices;

namespace ResultType.Results;

public static partial class Result
{
    public static Success Success => new Success();

    public static Created Created => new Created();

    public static Deleted Deleted => new Deleted();

    public static Updated Updated => new Updated();
}

[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct Success;
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct Created;
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct Deleted;
[StructLayout(LayoutKind.Sequential, Size = 1)]
public readonly record struct Updated;
