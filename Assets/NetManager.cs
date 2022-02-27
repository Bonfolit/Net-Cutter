using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class NetManager : MonoBehaviour
{
    public static NetManager Instance;

    private InputHandler inputHandler;

    public Transform pivot;

    public Material lineMaterial;

    public GameObject
        nodeSample,
        lineSample,
        heavyObj;

    public int iterationCount;

    public Vector2Int netSize;

    public Vector3 lineColorRGB;

    public float
        lineWidth,
        internodeDistance,
        startDelay,
        cutRadius;

    [Range(0f, 1f)]
    public float heavyNodeInertia;

    public bool simulate;

    public List<Node> nodes;

    public List<Line> lines;

    private int[] order;

    private float simulationStart;

    public LineRenderer lineRenderer;

    public Transform debugObj;

    private List<Line> linesToCut;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();

        Instance = this;

        simulationStart = 0f;
    }

    private void Start()
    {
        Initialize();

        simulationStart = Time.time;

        inputHandler.onMouseSwipe += CutBetween;
        inputHandler.onMouseHold += Cut;

        linesToCut = new List<Line>();

    }

    private void Update()
    {
        if (simulate)
        {
            if (simulationStart + startDelay < Time.time)
            {
                Simulate();
            }

            CreateOrderArray();

            lineRenderer.SetPosition(0, debugA);
            lineRenderer.SetPosition(1, debugB);

            //Debug.Log(lineRenderer.GetPosition(0) + " -- " + lineRenderer.GetPosition(1));
        }
    }

    private void Initialize()
    {
        nodes = new List<Node>();

        lines = new List<Line>();

        for (int i = 0; i < netSize.x; i++)
        {
            for (int j = 0; j < netSize.y; j++)
            {
                Vector2 pos = new Vector2(pivot.position.x + i * internodeDistance, pivot.position.y + j * internodeDistance);

                Node node = new Node(pos, new Vector2Int(i, j), (j == netSize.y - 1) ? true : false);

                if (node.isHeavy)
                {
                    node.moveability = 1f - heavyNodeInertia;
                }

                nodes.Add(node);

                if (i != 0)
                {
                    Line line = GameObject.Instantiate(lineSample, (Vector3)pos, Quaternion.identity, pivot).GetComponent<Line>();

                    line.Initialize(NodeAt(i - 1, j), NodeAt(i, j));

                    lines.Add(line);
                }

                if (j != 0)
                {
                    Line line = GameObject.Instantiate(lineSample, (Vector3)pos, Quaternion.identity, pivot).GetComponent<Line>();

                    line.Initialize(NodeAt(i, j - 1), NodeAt(i, j));

                    lines.Add(line);
                }
            }
        }

        CreateOrderArray();
    }

    private Node NodeAt(int posX, int posY)
    {
        return nodes.Find(a => a.coordinate.x == posX && a.coordinate.y == posY);
    }

    private Node[] GetHeavyNodes()
    {
        return nodes.FindAll(x => x.isHeavy).ToArray();
    }

    private void Simulate()
    {
        foreach (Node node in nodes)
        {
            if (!node.isLocked)
            {
                Vector2 prevPos = node.position;

                node.position += node.MoveStep;
                node.position += node.GravityStep;

                node.prevPosition = prevPos;
            }

            if (node.isHeavy)
            {
                debugObj.position = node.position;
            }
        }

        for (int i = 0; i < iterationCount; i++)
        {
            for (int j = 0; j < lines.Count; j++)
            {
                Line line = lines[order[j]];

                Vector2 center = line.Center;
                Vector2 direction = line.Direction;

                float length = line.CurrentLength;

                float stretchRatio = length / line.defaultLength;

                if (stretchRatio > 1f)
                {
                    if (!line.nodeA.isLocked)
                    {
                        Vector2 newPos = center + (direction * line.defaultLength / 2f);

                        if (line.nodeA.isHeavy)
                        {
                            line.nodeA.position = Vector2.Lerp(line.nodeA.position, newPos, 1f - heavyNodeInertia);
                        }
                        else
                        {
                            line.nodeA.position = newPos;
                        }

                        //line.nodeA.position = center + (direction * line.defaultLength / 2f);
                    }

                    if (!line.nodeB.isLocked)
                    {
                        Vector2 newPos = center - (direction * line.defaultLength / 2f);

                        if (line.nodeB.isHeavy)
                        {
                            line.nodeB.position = Vector2.Lerp(line.nodeB.position, newPos, 1f - heavyNodeInertia);
                        }
                        else
                        {
                            line.nodeB.position = newPos;
                        }

                        //line.nodeB.position = center - (direction * line.defaultLength / 2f);
                    }
                }
            }
        }

        if (linesToCut.Count > 0)
        {
            while (linesToCut.Count > 0)
            {
                lines.Remove(linesToCut[0]);

                Destroy(linesToCut[0].gameObject);

                linesToCut.RemoveAt(0);
                linesToCut.TrimExcess();
                lines.TrimExcess();
            }
        }
    }
    public Vector3 debugA, debugB;
    private void CutBetween(Vector3 from, Vector3 to)
    {
        //debugA = from;
        //debugB = to;

        //Vector3 pt1 = Camera.main.ScreenToWorldPoint(from);
        //Vector3 pt2 = Camera.main.ScreenToWorldPoint(to);

        ////Debug.Log("from: " + from + " , to: " + to);


        //for (int i = 0; i < lines.Count; i++)
        //{
        //    if (BonLibrary.CheckIntersection(from, to, lines[i].nodeA.position, lines[i].nodeB.position))
        //    {
        //        Debug.Log("before: " + lines.Count);
        //        lines.Remove(lines[i]);
        //        lines.TrimExcess();
        //        Debug.Log("after: " + lines.Count);

        //    }
        //}
    }

    private void Cut(Vector3 point)
    {
        linesToCut = new List<Line>();

        for (int i = 0; i < lines.Count; i++)
        {
            if (Vector3.Distance(point, lines[i].Center) < cutRadius)
            {
                linesToCut.Add(lines[i]);
            }
        }

        
    }

    protected void CreateOrderArray()
    {
        order = new int[lines.Count];

        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }

        order = BonLibrary.ShuffleArray(order, new System.Random());
    }

    //private void OnDrawGizmos()
    //{
    //    if (nodes != null)
    //    {
    //        for (int i = 0; i < nodes.Count; i++)
    //        {
    //            if (nodes[i].isLocked)
    //            {
    //                Gizmos.color = new Color(lineColorRGB.x, lineColorRGB.y, lineColorRGB.z);
    //            }
    //            else if (nodes[i].coordinate == new Vector2Int(2, 3))
    //            {
    //                Gizmos.color = Color.blue;
    //            }
    //            else
    //            {
                    
    //                Gizmos.color = Color.green;
    //            }

    //            Gizmos.DrawSphere(new Vector3(nodes[i].position.x, nodes[i].position.y, 0f), .03f);

    //            Gizmos.DrawLine(debugA, debugB);
    //        }

    //    }
    //}
}
