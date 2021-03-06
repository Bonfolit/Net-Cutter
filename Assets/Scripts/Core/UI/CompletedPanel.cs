using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedPanel : MonoBehaviour, IPanel
{
    private bool state;
    public bool State => state;

    public void SetPanel(bool active)
    {
        state = active;

        gameObject.SetActive(active);
    }
}
