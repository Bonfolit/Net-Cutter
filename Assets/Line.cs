using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Node
        nodeA,
        nodeB;

    public float defaultLength;

    public LineRenderer lineRenderer;

    public void Initialize(Node _nodeA, Node _nodeB)
    {
        nodeA = _nodeA;
        nodeB = _nodeB;

        defaultLength = Vector3.Distance(nodeA.position, nodeB.position) * 1f;

        lineRenderer = gameObject.AddComponent<LineRenderer>();

        if (lineRenderer)
        {
            lineRenderer.material = NetManager.Instance.lineMaterial;
            
            // set width of the renderer
            lineRenderer.startWidth = NetManager.Instance.lineWidth;
            lineRenderer.endWidth = NetManager.Instance.lineWidth;
        }
        
    }

    private void Update()
    {
        if (lineRenderer)
        {
            lineRenderer.SetPosition(0, (Vector3)nodeA.position);
            lineRenderer.SetPosition(1, (Vector3)nodeB.position);

            if (nodeA.isHeavy)
            {
                //Debug.Log("heavy coords: " + nodeA.position);
            }
        }
    }

    public Vector2 Center => Vector3.Lerp(nodeA.position, nodeB.position, .5f);
    public Vector2 Direction => (nodeA.position - nodeB.position).normalized;
    public float CurrentLength => (Vector2.Distance(nodeA.position, nodeB.position));

}
