using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingPanel : MonoBehaviour, IPanel
{
    private bool state;
    public bool State => state;

    public void SetPanel(bool active)
    {
        state = active;

        gameObject.SetActive(active);
    }
}
