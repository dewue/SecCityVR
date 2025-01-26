using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using System.Collections.Generic;
using System;
using Fusion;
using TMPro;

public class CityMethod
{
    public string name { get; set; }
    public string className { get; set; }
    public GameObject gameObject { get; set; }
    public List<string> outgoingCalls { get; set; }
    public List<string> incomingCalls { get; set; }
    public float width { get; set; }
    public float length { get; set; }
}

public class AnalysisReader : MonoBehaviour
{
    [SerializeField]
    private TextAsset jsonFile;

    [SerializeField]
    private TextAsset xmlFile;

    [SerializeField]
    private TextAsset packageAnalysisJson;

    [SerializeField]
    private GameObject buildingPrefab;

    [SerializeField]
    private GameObject linePrefab;

    [SerializeField]
    private float scaleMultiplier = 0.005f;

    [SerializeField]
    private float spacingFactor = 0.8f;

    [SerializeField]
    private float maxHops = 0f;

    [SerializeField]
    private float flyingSpeed = 5f;

    [SerializeField]
    private bool onlyPackages = false;
   
    [SerializeField]
    private bool onlyClasses= false;

    [SerializeField]
    public bool guidedReview = false;

    [SerializeField]
    public bool offlineMode = false;

    [SerializeField]
    public string ownPackageName = "fr.christophetd.log4shell.vulnerableapp";

    [SerializeField]
    public GameObject cameraGo;


    List<CityMethod> calls = new List<CityMethod>();

    [HideInInspector]
    public BugCollection bugCollection;

    [HideInInspector]
    public CubeInfo currentlySeclected;

    int cityWidth = 0;
    int cityHeight = 0;

    GameObject testLine;
    GameObject parentMesh;
    List<GameObject> childMeshes = new List<GameObject>();

    void Awake()
    {
        testLine = Instantiate(linePrefab, transform.position, Quaternion.identity);
        testLine.SetActive(false);

        generateCityWithPackageAnalysis();

        StaticBatchingUtility.Combine(childMeshes.ToArray(), parentMesh);
    }

    void Start()
    {
        if (!offlineMode)
        {
            NetworkManager manager = GameObject.Find("Manager").GetComponent<NetworkManager>();
            manager.JoinOrCreate();
        }
        else
        {
            GameObject connectedTextGameObject = GameObject.Find("OnlineLabel").gameObject;

            TextMeshProUGUI tmpObject = connectedTextGameObject.GetComponent<TextMeshProUGUI>();
            tmpObject.text = "Offline Mode";
        }
    }

    void Update()
    {
        //https://nbpsblog.b-cdn.net/wp-content/uploads/2021/01/rXDS464cIp.png
        Vector3 cityCenter = new Vector3(cityWidth / 2, 15, cityHeight / 2);

        // Y Button to teleport to other user
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            GameObject otherPlayer = Array.Find(GameObject.FindGameObjectsWithTag("Player"), p => p.GetComponent<NetworkObject>().StateAuthority != NetworkManager.Instance.Runner.LocalPlayer);
            if (otherPlayer != null)
            {
                cameraGo.transform.position = otherPlayer.transform.position - otherPlayer.transform.forward * 2;
                cameraGo.transform.LookAt(otherPlayer.transform.position);
            }
        }

        // X Button to toggle guided review
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            guidedReview = !guidedReview;
        }
        if (guidedReview)
        {
            GameObject otherPlayer = Array.Find(GameObject.FindGameObjectsWithTag("Player"), p => p.GetComponent<NetworkObject>().StateAuthority != NetworkManager.Instance.Runner.LocalPlayer);
            if (otherPlayer != null)
            {
                cameraGo.transform.position = otherPlayer.transform.position - otherPlayer.transform.forward * 2;
                return;
            }
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5f)
        {
            cameraGo.transform.position = new Vector3(cameraGo.transform.position.x, cameraGo.transform.position.y + flyingSpeed * Time.deltaTime, cameraGo.transform.position.z);
        }
        else if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f && cameraGo.transform.position.y > 2)
        {
            cameraGo.transform.position = new Vector3(cameraGo.transform.position.x, cameraGo.transform.position.y - flyingSpeed * Time.deltaTime, cameraGo.transform.position.z);
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0f)
        {
            cameraGo.transform.position += Camera.main.transform.forward * flyingSpeed * Time.deltaTime;
        }
        else if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            cameraGo.transform.position -= Camera.main.transform.forward * flyingSpeed / 2 * Time.deltaTime;
        }
    }

    private void generateCityWithPackageAnalysis()
    {
        TextReader sr = new StringReader(xmlFile.text);
        XmlSerializer serializer = new XmlSerializer(typeof(BugCollection));
        bugCollection = (BugCollection)serializer.Deserialize(sr);

        PackageAnalysisResult result = JsonUtility.FromJson<PackageAnalysisResult>(packageAnalysisJson.text);
        PackageInfo[] packages = result.packages;
        int packagesum = packages.Sum(p => p.linesOfCode);
        cityWidth = (int)Math.Round(packagesum * scaleMultiplier / 100);
        cityHeight = (int)Math.Round(packagesum * scaleMultiplier / 100);
        GameObject camera = GameObject.Find("OVRCameraRig");
        camera.transform.position = new Vector3(cityWidth / 2, 10, cityHeight / 2);

        GameObject basePlate = GameObject.Find("BasePlate");
        basePlate.name = $"CityCanvas-" + bugCollection.Project.Jar;
        basePlate.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f);
        basePlate.transform.localScale = new Vector3(cityWidth, 0.5f, cityHeight);
        basePlate.transform.position = new Vector3(cityWidth / 2, 0.25f, cityHeight / 2);
        parentMesh = basePlate;

        IEnumerable<PackageInfo> sortedPackages = packages.OrderByDescending(x => x.linesOfCode).Where(c => c.linesOfCode > 0);
        double[] sortedPackageSizes = sortedPackages
         .Select(x => (double)x.linesOfCode)
         .ToArray();
        LayoutRectangle[] packagesRectangles = TreeMap.GetRectangles(sortedPackageSizes, cityWidth, cityHeight);
        int pkgCount = sortedPackages.Count();

        for (int i = 0; i < pkgCount; i++)
        {
            PackageInfo pkg = sortedPackages.ElementAt(i);
            LayoutRectangle pkgRect = packagesRectangles[i];
            Color pkgColor = new Color(0.75f, 0.75f, 0.75f);

            if (pkg.name == ownPackageName)
            {
                pkgColor = new Color(1, 0f, 1);
            }

            GameObject packageContainer = RenderCube(
                pkg.linesOfCode,
                $"Package-{i}",
                pkg.name,
                pkg.name,
                pkgColor,
                new Vector3(pkgRect.X, 0.75f, pkgRect.Y),
                new Vector3(pkgRect.Width * spacingFactor, 0.5f, pkgRect.Length * spacingFactor),
                basePlate
            );

            if (onlyPackages)
            {
                continue;
            }

            IEnumerable<ClassInfo> sortedClasses = pkg.classes.OrderByDescending(c => c.linesOfCode).Where(c => c.linesOfCode > 0);
            double[] sortedClassSizes = sortedClasses
             .Select(c => (double)c.linesOfCode)
             .ToArray();

            LayoutRectangle[] classRectangles = TreeMap.GetRectangles(sortedClassSizes, pkgRect.Width * spacingFactor, pkgRect.Length * spacingFactor);

            for (int j = 0; j < classRectangles.Count(); j++)
            {
                ClassInfo cls = sortedClasses.ElementAt(j);
                LayoutRectangle clsRect = classRectangles[j];

                float clsX = clsRect.X + pkgRect.X;
                float clsY = clsRect.Y + pkgRect.Y;
                Color classColor = new Color(0.75f, 0.75f, 0.75f);

                GameObject classContainer = RenderCube(
                    cls.linesOfCode,
                    $"Class-{j}",
                    cls.name,
                    cls.name,
                    classColor,
                    new Vector3(clsX, cls.linesOfCode / 2 * scaleMultiplier + 1, clsY),
                    new Vector3(clsRect.Width * spacingFactor, cls.linesOfCode * scaleMultiplier + 0.005f, clsRect.Length * spacingFactor),
                    packageContainer,
                    true,
                    null,
                    true
                ); ;

                if (onlyClasses)
                {
                    continue;
                }
                // für jede method in class -> add to calls array
                for (int k = 0; k < cls.methods.Count(); k++)
                {
                    MethodInfo methodFromHierarchy = cls.methods[k];
                    int classStart = cls.startLine;
                    int classEnd = cls.endLine;
                    int classHeight = 1 + classEnd - classStart;
                    float sizeToLocRatio = cls.linesOfCode / (float)classHeight;

                    int methodStart = methodFromHierarchy.startLine;
                    int methodEnd = methodFromHierarchy.endLine;
                    float methodHeight = 1 + methodEnd - methodStart;
                    float mappedMethodHeight = methodHeight * sizeToLocRatio;
                    float methodWidth = clsRect.Width * spacingFactor * 1.1f;
                    float methodLength = clsRect.Length * spacingFactor * 1.1f;
                    Color methodColor = new Color(0f, 0f, 1f);
                    bool renderCubeActive = false;

                    // render in red if bug
                    IEnumerable<BugInstance> bugs = bugCollection.BugInstance.FindAll(b => b.Class.Classname == cls.name);
                    Method bugMethod = bugs.ToList().Find(b => b.Method?.Name == methodFromHierarchy.name)?.Method;
                    if (bugMethod != null)
                    {

                        renderCubeActive = true;
                        methodColor = new Color(1.0f, 0.64f, 0.0f);

                        if (bugs.Select(b => b.Priority).Max() == 1)
                        {
                            methodColor = new Color(1, 0, 0);
                        }
                        //if (methodFromHierarchy?.callsTo?.Length > 0 || methodFromHierarchy?.callsFrom?.Length > 0)
                        //{
                        //    methodColor = new Color(0f, 0f, 1f);
                        //}
                    }

                    GameObject methodContainer = RenderCube(
                        (int)mappedMethodHeight,
                            $"Method-{k}",
                            methodFromHierarchy.name,
                            $"{cls.name}.{methodFromHierarchy.name}",
                            methodColor,
                            new Vector3(clsX - (clsRect.Width * spacingFactor * 0.05f), ((methodStart - classStart) * sizeToLocRatio + mappedMethodHeight / 2) * scaleMultiplier + 1, clsY - (clsRect.Length * spacingFactor * 0.05f)),
                            new Vector3(methodWidth, mappedMethodHeight * scaleMultiplier, methodLength),
                            classContainer,
                            renderCubeActive,
                            bugs.ToArray(),
                            true
                        );

                    // add to calls array
                    CityMethod cm = new CityMethod()
                    {
                        name = $"{cls.name}.{methodFromHierarchy.name}",
                        className = cls.name,
                        gameObject = methodContainer,
                        width = methodWidth,
                        length = methodLength,
                        incomingCalls = new List<string>(),
                        outgoingCalls = new List<string>()
                    };

                    if (methodFromHierarchy.callsTo?.Length > 0)
                    {
                        cm.outgoingCalls = methodFromHierarchy.callsTo.Select(c => signatureToPath2(c)).ToList();
                    }
                    if (methodFromHierarchy?.callsFrom?.Length > 0)
                    {
                        cm.incomingCalls = methodFromHierarchy.callsFrom.Select(c => signatureToPath2(c)).ToList();
                    }
                    calls.Add(cm);
                }

            }
        }
    }

    private void DrawLineFromMethodToMethod(CityMethod from, CityMethod to)
    {
        if (from?.gameObject?.transform?.position != null && to?.gameObject?.transform?.position != null)
        {
            from.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            to.gameObject.transform.GetChild(0).gameObject.SetActive(true);

            Vector3 arcFrom = from.gameObject.transform.position + new Vector3(from.width / 2, 0, from.length / 2);
            Vector3 arcTo = to.gameObject.transform.position + new Vector3(to.width / 2, 0, to.length / 2);
            Vector3 midPoint = arcFrom + (arcTo - arcFrom) / 2;
            if (from.className == to.className)
            {
                midPoint.x += 1;
            }
            else
            {
                midPoint.y = Mathf.Max(arcFrom.y, arcTo.y) + 25;
            }

            GameObject testLine = Instantiate(linePrefab, arcFrom, Quaternion.identity);
            LineRenderer lineRenderer = testLine.transform.Find("LineComponentRenderer").GetComponent<LineRenderer>();

            DrawQuadraticBezierCurve(lineRenderer, arcFrom, midPoint, arcTo);
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.white;
            testLine.transform.parent = from.gameObject.transform;
            testLine.transform.position = new Vector3(0, 0, 0);
        }
    }

    //https://www.codinblack.com/how-to-draw-lines-circles-or-anything-else-using-linerenderer/
    void DrawQuadraticBezierCurve(LineRenderer lineRenderer, Vector3 point0, Vector3 point1, Vector3 point2)
    {
        //lineRenderer.positionCount = 3;
        //lineRenderer.SetPositions(positions);
        lineRenderer.positionCount = 200;
        float t = 0f;
        Vector3 B = new Vector3(0, 0, 0);
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
            lineRenderer.SetPosition(i, B);
            t += (1 / (float)lineRenderer.positionCount);
        }
    }

    private void RenderAllCallsForMethod(CityMethod method)
    {
        foreach (string call in method.outgoingCalls)
        {
            CityMethod to = calls.Find(m => m.name == call);
            if (to != null) DrawLineFromMethodToMethod(method, to);
        }
        foreach (string call in method.incomingCalls)
        {
            CityMethod from = calls.Find(m => m.name == call);
            if (from != null) DrawLineFromMethodToMethod(from, method);
        }
    }

    // called via RPC
    public void RenderCallsForMethodName(string fromName)
    {
        CityMethod call = calls.Find(c => c.name == fromName);
        if (call != null)
        {
            //RenderCallsForMethodRecursively(call, 0);
            RenderAllCallsForMethod(call);
        }
        else
        {
            Debug.Log("nope");
        }
    }

    // called via RPC
    public void RenderCallsForLastSelected()
    {
        CityMethod call = calls.Find(c => c.name == currentlySeclected.FullName);
        if (call != null)
        {
            //RenderCallsForMethodRecursively(call, 0);
            RenderAllCallsForMethod(call);
        }
        else
        {
            Debug.Log("nope");
        }
    }


    public void RenderCallsForMethodRecursively(CityMethod method, int hops)
    {
        if (hops >= maxHops)
        {
            return;
        }
        foreach (string call in method.outgoingCalls)
        {
            CityMethod to = calls.Find(m => m.name == call);
            if (to != null) DrawLineFromMethodToMethod(method, to);
            RenderCallsForMethodRecursively(to, hops + 1);
        }
        foreach (string call in method.incomingCalls)
        {
            CityMethod from = calls.Find(m => m.name == call);
            if (from != null) DrawLineFromMethodToMethod(from, method);
            RenderCallsForMethodRecursively(from, hops + 1);
        }
    }

    private GameObject RenderCube(int loc, string prefix, string name, string fullName, Color color, Vector3 position, Vector3 sizeScaling, GameObject parent, bool active = true, BugInstance[] bugs = null, bool withLOD = false)
    {
        GameObject classContainer = new GameObject();
        classContainer.name = $"{prefix}-{name}";
        GameObject clsBuilding = Instantiate(buildingPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        clsBuilding.SetActive(active);
        clsBuilding.GetComponent<SelectTest>().method = new CubeInfo(name, fullName, loc);
        clsBuilding.GetComponent<SelectTest>().bugs = bugs ?? new BugInstance[0];
        clsBuilding.GetComponent<Renderer>().material.color = color;
        clsBuilding.GetComponent<LODGroup>().enabled = withLOD;
        clsBuilding.transform.parent = classContainer.transform;
        clsBuilding.transform.position = new Vector3(0.5f, 0, 0.5f);
        if (active)
        {
            childMeshes.Add(clsBuilding);
        }

        classContainer.transform.position = position;
        classContainer.transform.localScale = sizeScaling;
        classContainer.transform.parent = parent.transform;

        return classContainer;
    }

    private string signatureToPath2(TreeMethodInfo m)
    {
        string fullPkg = m.signature.Substring(1, m.signature.IndexOf(":") - 1);
        return fullPkg + "." + m.name;
    }
}

[Serializable]
class CallHierarchyAnalysis
{
    public CallHierarchyMethod[] callHierarchy;
}

[Serializable]
class CallHierarchyMethod : CallHierarchySignature
{
    public CallHierarchySignature[] callsTo;
    public CallHierarchySignature[] callsFrom;
}

[Serializable]
class CallHierarchySignature
{
    public string signature;
    public string method;
}