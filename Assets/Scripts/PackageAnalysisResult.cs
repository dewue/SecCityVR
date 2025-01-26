using System;
using UnityEngine;

[Serializable]
public class PackageAnalysisResult
{
    public PackageInfo[] packages;
}

[Serializable]
public class TreeMethodInfo
{
    public string name;
    public string signature;
}

[Serializable]
public class MethodInfo
{
    public string name;
    public int linesOfCode;
    public int startLine;
    public int endLine;
    public TreeMethodInfo[] callsFrom;
    public TreeMethodInfo[] callsTo;
}

[Serializable]
public class ClassInfo
{
    public string name;
    public MethodInfo[] methods;
    public int linesOfCode;
    public int startLine;
    public int endLine;
}

[Serializable]
public class PackageInfo
{
    public string name;
    public ClassInfo[] classes;
    public int linesOfCode;
}
