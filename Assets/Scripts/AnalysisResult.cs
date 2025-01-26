using System;
using System.Xml.Serialization;
using System.Collections.Generic;

[Serializable]
public class CubeInfo
{
    public string Name;
    public string FullName;
    public int LOC;

    public CubeInfo(string name, string fullName, int loc)
    {
        Name = name;
        FullName = fullName;
        LOC = loc;
    }
}


[XmlRoot(ElementName = "Plugin")]
public class Plugin
{
    [XmlAttribute(AttributeName = "id")]
    public string Id { get; set; }
    [XmlAttribute(AttributeName = "enabled")]
    public string Enabled { get; set; }
}

[XmlRoot(ElementName = "Project")]
public class Project
{
    [XmlElement(ElementName = "Jar")]
    public string Jar { get; set; }
    [XmlElement(ElementName = "Plugin")]
    public Plugin Plugin { get; set; }
    [XmlAttribute(AttributeName = "projectName")]
    public string ProjectName { get; set; }
}

[XmlRoot(ElementName = "SourceLine")]
public class SourceLine
{
    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }
    [XmlAttribute(AttributeName = "classname")]
    public string Classname { get; set; }
    [XmlAttribute(AttributeName = "start")]
    public int Start { get; set; }
    [XmlAttribute(AttributeName = "end")]
    public int End { get; set; }
    [XmlAttribute(AttributeName = "sourcefile")]
    public string Sourcefile { get; set; }
    [XmlAttribute(AttributeName = "sourcepath")]
    public string Sourcepath { get; set; }
    [XmlAttribute(AttributeName = "startBytecode")]
    public string StartBytecode { get; set; }
    [XmlAttribute(AttributeName = "endBytecode")]
    public string EndBytecode { get; set; }
    [XmlAttribute(AttributeName = "primary")]
    public string Primary { get; set; }
    [XmlAttribute(AttributeName = "role")]
    public string Role { get; set; }
    [XmlAttribute(AttributeName = "synthetic")]
    public string Synthetic { get; set; }
}

[XmlRoot(ElementName = "Class")]
public class Class
{
    [XmlElement(ElementName = "SourceLine")]
    public SourceLine SourceLine { get; set; }
    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }
    [XmlAttribute(AttributeName = "classname")]
    public string Classname { get; set; }
    [XmlAttribute(AttributeName = "primary")]
    public string Primary { get; set; }
}

[XmlRoot(ElementName = "Method")]
public class Method
{
    [XmlElement(ElementName = "SourceLine")]
    public SourceLine SourceLine { get; set; }
    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }
    [XmlAttribute(AttributeName = "classname")]
    public string Classname { get; set; }
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "signature")]
    public string Signature { get; set; }
    [XmlAttribute(AttributeName = "isStatic")]
    public string IsStatic { get; set; }
    [XmlAttribute(AttributeName = "primary")]
    public string Primary { get; set; }
}

[XmlRoot(ElementName = "BugInstance")]
public class BugInstance
{
    [XmlElement(ElementName = "ShortMessage")]
    public string ShortMessage { get; set; }
    [XmlElement(ElementName = "LongMessage")]
    public string LongMessage { get; set; }
    [XmlElement(ElementName = "Class")]
    public Class Class { get; set; }
    [XmlElement(ElementName = "Method")]
    public Method Method { get; set; }
    [XmlElement(ElementName = "SourceLine")]
    public List<SourceLine> SourceLine { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }
    [XmlAttribute(AttributeName = "priority")]
    public int Priority { get; set; }
    [XmlAttribute(AttributeName = "rank")]
    public string Rank { get; set; }
    [XmlAttribute(AttributeName = "abbrev")]
    public string Abbrev { get; set; }
    [XmlAttribute(AttributeName = "category")]
    public string Category { get; set; }
    [XmlAttribute(AttributeName = "instanceHash")]
    public string InstanceHash { get; set; }
    [XmlAttribute(AttributeName = "instanceOccurrenceNum")]
    public string InstanceOccurrenceNum { get; set; }
    [XmlAttribute(AttributeName = "instanceOccurrenceMax")]
    public string InstanceOccurrenceMax { get; set; }
    [XmlAttribute(AttributeName = "cweid")]
    public string Cweid { get; set; }
    [XmlElement(ElementName = "String")]
    public List<String> String { get; set; }
}

[XmlRoot(ElementName = "String")]
public class String
{
    [XmlElement(ElementName = "Message")]
    public string Message { get; set; }
    [XmlAttribute(AttributeName = "value")]
    public string Value { get; set; }
    [XmlAttribute(AttributeName = "role")]
    public string Role { get; set; }
}

[XmlRoot(ElementName = "Error")]
public class Error
{
    [XmlElement(ElementName = "ErrorMessage")]
    public string ErrorMessage { get; set; }
    [XmlElement(ElementName = "Exception")]
    public string Exception { get; set; }
    [XmlElement(ElementName = "StackTrace")]
    public List<string> StackTrace { get; set; }
}

[XmlRoot(ElementName = "Errors")]
public class ErrorsWrapper
{
    [XmlElement(ElementName = "Error")]
    public Error Error { get; set; }
    [XmlElement(ElementName = "MissingClass")]
    public List<string> MissingClass { get; set; }
    [XmlAttribute(AttributeName = "errors")]
    public string Errors { get; set; }
    [XmlAttribute(AttributeName = "missingClasses")]
    public string MissingClasses { get; set; }
}

[XmlRoot(ElementName = "ClassStats")]
public class ClassStats
{
    [XmlAttribute(AttributeName = "class")]
    public string Class { get; set; }
    [XmlAttribute(AttributeName = "sourceFile")]
    public string SourceFile { get; set; }
    [XmlAttribute(AttributeName = "interface")]
    public string Interface { get; set; }
    [XmlAttribute(AttributeName = "size")]
    public int Size { get; set; }
    [XmlAttribute(AttributeName = "bugs")]
    public int Bugs { get; set; }
    [XmlAttribute(AttributeName = "priority_2")]
    public string Priority_2 { get; set; }
    [XmlAttribute(AttributeName = "priority_1")]
    public string Priority_1 { get; set; }
}

[XmlRoot(ElementName = "PackageStats")]
public class PackageStats
{
    [XmlElement(ElementName = "ClassStats")]
    public List<ClassStats> ClassStats { get; set; }
    [XmlAttribute(AttributeName = "package")]
    public string Package { get; set; }
    [XmlAttribute(AttributeName = "total_bugs")]
    public int Total_bugs { get; set; }
    [XmlAttribute(AttributeName = "total_types")]
    public int Total_types { get; set; }
    [XmlAttribute(AttributeName = "total_size")]
    public int Total_size { get; set; }
    [XmlAttribute(AttributeName = "priority_2")]
    public string Priority_2 { get; set; }
    [XmlAttribute(AttributeName = "priority_1")]
    public string Priority_1 { get; set; }
}

[XmlRoot(ElementName = "ClassProfile")]
public class ClassProfile
{
    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }
    [XmlAttribute(AttributeName = "totalMilliseconds")]
    public string TotalMilliseconds { get; set; }
    [XmlAttribute(AttributeName = "invocations")]
    public string Invocations { get; set; }
    [XmlAttribute(AttributeName = "avgMicrosecondsPerInvocation")]
    public string AvgMicrosecondsPerInvocation { get; set; }
    [XmlAttribute(AttributeName = "maxMicrosecondsPerInvocation")]
    public string MaxMicrosecondsPerInvocation { get; set; }
    [XmlAttribute(AttributeName = "standardDeviationMicrosecondsPerInvocation")]
    public string StandardDeviationMicrosecondsPerInvocation { get; set; }
}

[XmlRoot(ElementName = "FindBugsProfile")]
public class FindBugsProfile
{
    [XmlElement(ElementName = "ClassProfile")]
    public List<ClassProfile> ClassProfile { get; set; }
}

[XmlRoot(ElementName = "FindBugsSummary")]
public class FindBugsSummary
{
    [XmlElement(ElementName = "PackageStats")]
    public List<PackageStats> PackageStats { get; set; }
    [XmlElement(ElementName = "FindBugsProfile")]
    public FindBugsProfile FindBugsProfile { get; set; }
    [XmlAttribute(AttributeName = "timestamp")]
    public string Timestamp { get; set; }
    [XmlAttribute(AttributeName = "total_classes")]
    public string Total_classes { get; set; }
    [XmlAttribute(AttributeName = "referenced_classes")]
    public string Referenced_classes { get; set; }
    [XmlAttribute(AttributeName = "total_bugs")]
    public int Total_bugs { get; set; }
    [XmlAttribute(AttributeName = "total_size")]
    public int Total_size { get; set; }
    [XmlAttribute(AttributeName = "num_packages")]
    public int Num_packages { get; set; }
    [XmlAttribute(AttributeName = "java_version")]
    public string Java_version { get; set; }
    [XmlAttribute(AttributeName = "vm_version")]
    public string Vm_version { get; set; }
    [XmlAttribute(AttributeName = "cpu_seconds")]
    public string Cpu_seconds { get; set; }
    [XmlAttribute(AttributeName = "clock_seconds")]
    public string Clock_seconds { get; set; }
    [XmlAttribute(AttributeName = "peak_mbytes")]
    public string Peak_mbytes { get; set; }
    [XmlAttribute(AttributeName = "alloc_mbytes")]
    public string Alloc_mbytes { get; set; }
    [XmlAttribute(AttributeName = "gc_seconds")]
    public string Gc_seconds { get; set; }
    [XmlAttribute(AttributeName = "priority_2")]
    public string Priority_2 { get; set; }
    [XmlAttribute(AttributeName = "priority_1")]
    public string Priority_1 { get; set; }
}

[XmlRoot(ElementName = "BugCategory")]
public class BugCategory
{
    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }
    [XmlAttribute(AttributeName = "category")]
    public string Category { get; set; }
}

[XmlRoot(ElementName = "BugPattern")]
public class BugPattern
{
    [XmlElement(ElementName = "ShortDescription")]
    public string ShortDescription { get; set; }
    [XmlElement(ElementName = "Details")]
    public string Details { get; set; }
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }
    [XmlAttribute(AttributeName = "abbrev")]
    public string Abbrev { get; set; }
    [XmlAttribute(AttributeName = "category")]
    public string Category { get; set; }
    [XmlAttribute(AttributeName = "cweid")]
    public string Cweid { get; set; }
}

[XmlRoot(ElementName = "BugCode")]
public class BugCode
{
    [XmlElement(ElementName = "Description")]
    public string Description { get; set; }
    [XmlAttribute(AttributeName = "abbrev")]
    public string Abbrev { get; set; }
    [XmlAttribute(AttributeName = "cweid")]
    public string Cweid { get; set; }
}

[XmlRoot(ElementName = "BugCollection")]
public class BugCollection
{
    [XmlElement(ElementName = "Project")]
    public Project Project { get; set; }
    [XmlElement(ElementName = "BugInstance")]
    public List<BugInstance> BugInstance { get; set; }
    [XmlElement(ElementName = "BugCategory")]
    public BugCategory BugCategory { get; set; }
    [XmlElement(ElementName = "BugPattern")]
    public List<BugPattern> BugPattern { get; set; }
    [XmlElement(ElementName = "BugCode")]
    public List<BugCode> BugCode { get; set; }
    [XmlElement(ElementName = "Errors")]
    public ErrorsWrapper Errors { get; set; }
    [XmlElement(ElementName = "FindBugsSummary")]
    public FindBugsSummary FindBugsSummary { get; set; }
    [XmlElement(ElementName = "ClassFeatures")]
    public string ClassFeatures { get; set; }
    [XmlElement(ElementName = "History")]
    public string History { get; set; }
    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; }
    [XmlAttribute(AttributeName = "sequence")]
    public string Sequence { get; set; }
    [XmlAttribute(AttributeName = "timestamp")]
    public string Timestamp { get; set; }
    [XmlAttribute(AttributeName = "analysisTimestamp")]
    public string AnalysisTimestamp { get; set; }
    [XmlAttribute(AttributeName = "release")]
    public string Release { get; set; }
}
