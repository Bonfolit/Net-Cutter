using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    public BaloonColor color;

    public Node node;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            Bucket bucket = other.GetComponent<Bucket>();

            if (!bucket)
            {
                Debug.LogWarning("No Bucket!");

                return;
            }

            if (bucket.TryConsume(this))
            {
                node.baloon = null;

                LevelManager.Instance.onBaloonConsume?.Invoke(this);
            }
        }
    }

    private void Update()
    {
        if (node)
        {
            transform.position = (Vector3)node.position;
        }
    }
}

public enum BaloonColor
{
    Red,
    Green,
    Blue
}
