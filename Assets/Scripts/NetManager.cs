using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
public class NetManager : MonoBehaviour
{
    public static NetManager Instance;

    private InputHandler inputHandler;

    public Transform 
        pivot,
        cutoffLevel;

    public Material lineMaterial;

    public GameObject
        nodeSample,
        lineSample;

    public int iterationCount;

    public Vector2Int netSize;

    public float
        lineWidth,
        internodeDistance,
        startDelay,
        cutRadius;

    public bool simulate;

    public List<Node> nodes;

    public List<Line> lines;

    private int[] order;

    private float simulationStart;

    private List<Line> linesToCut;

    public List<BaloonGroup> baloonGroups;

    public List<Baloon> baloons;

    public List<Bucket> buckets;

    public Action<Baloon> onBaloonConsume;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();

        Instance = this;

        simulationStart = 0f;
    }

    private void Start()
    {
        Initialize();

        PlaceBaloons();

        simulationStart = Time.time;

        inputHandler.onMouseHold += Cut;

        onBaloonConsume += OnBaloonConsume;

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

                Node node = GameObject.Instantiate(nodeSample, (Vector3)pos, Quaternion.identity, pivot).GetComponent<Node>();

                node.Initialize(pos, new Vector2Int(i, j), (j == netSize.y - 1) ? true : false);

                node.moveability = 1f;

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

    private void PlaceBaloons()
    {
        if (baloonGroups.Count == 0)
        {
            return;
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            foreach (BaloonGroup group in baloonGroups)
            {
                if (group.coordinates.Count > 0)
                {
                    foreach (Vector2Int coord in group.coordinates)
                    {
                        if (coord == nodes[i].coordinate)
                        {
                            nodes[i].moveability = 1f - Mathf.Pow(group.inertia, .01f);

                            Baloon baloon = GameObject.Instantiate(group.prefab, nodes[i].position, Quaternion.identity, pivot).GetComponent<Baloon>();

                            baloons.Add(baloon);

                            nodes[i].baloon = baloon;

                            baloon.node = nodes[i];
                        }
                    }
                }
            }
        }
    }

    private Node NodeAt(int posX, int posY)
    {
        return nodes.Find(a => a.coordinate.x == posX && a.coordinate.y == posY);
    }

    private void OnBaloonConsume(Baloon baloon)
    {
        if (!baloons.Contains(baloon))
        {
            Debug.LogWarning("Trying to remove a baloon that was already removed!");

            return;
        }

        RemoveBaloon(baloon);

        if (CheckCompletion())
        {
            GameManager.Instance.SetState(GameState.Success);
        }
    }

    private void RemoveBaloon(Baloon baloon)
    {
        baloons.Remove(baloon);

        Destroy(baloon.gameObject);

        baloons.TrimExcess();
    }

    private bool CheckCompletion()
    {
        foreach (Bucket bucket in buckets)
        {
            if (bucket.requirement > 0)
            {
                return false;
            }
        }

        return true;
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

                        line.nodeA.MoveTowards(newPos);
                    }

                    if (!line.nodeB.isLocked)
                    {
                        Vector2 newPos = center - (direction * line.defaultLength / 2f);

                        line.nodeB.MoveTowards(newPos);
                    }
                }
            }
        }

        List<Line> cutoffLines = GetCutoffLines();

        for (int i = 0; i < cutoffLines.Count; i++)
        {
            if (!linesToCut.Contains(cutoffLines[i]))
            {
                linesToCut.Add(cutoffLines[i]);
            }
        }

        if (linesToCut.Count > 0)
        {
            while (linesToCut.Count > 0)
            {
                lines.Remove(linesToCut[0]);

                if (linesToCut[0].nodeA.baloon)
                {
                    RemoveBaloon(linesToCut[0].nodeA.baloon);
                }

                if (linesToCut[0].nodeB.baloon)
                {
                    RemoveBaloon(linesToCut[0].nodeB.baloon);
                }

                Destroy(linesToCut[0].gameObject);

                linesToCut.RemoveAt(0);
                linesToCut.TrimExcess();
                lines.TrimExcess();
            }
        }
    }

    private List<Line> GetCutoffLines()
    {
        return lines.FindAll(x => x.Center.y < cutoffLevel.position.y);
    }

    private void Cut(Vector3 point)
    {
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
}

[System.Serializable]
public class BaloonGroup
{
    public GameObject prefab;

    [Range(0f, 1f)]
    public float inertia;

    public List<Vector2Int> coordinates;
}