using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour, IGameStateListener
{
    public static PanelManager Instance;

    public List<IPanel> panels;

    private void Awake()
    {
        Instance = this;

        panels = GetComponentsInChildren<IPanel>(true).ToList();
    }

    public T GetPanel<T>() where T : IPanel
    {
        Debug.Log(typeof(T));
        IPanel panel = panels.Find(x => x.GetType() == typeof(T));

        return (T)panel;
    }

    public T ChangePanel<T>() where T : IPanel
    {
        IPanel panel = GetPanel<T>();

        if (panel == null)
        {
            Debug.LogError($"There is no panel : {typeof(T)}");

            return default;
        }

        List<IPanel> otherPanels = panels.FindAll(x => x != panel);

        foreach (IPanel p in otherPanels)
        {
            p.SetPanel(false);
        }

        panel.SetPanel(true);

        return (T)panel;
    }

    public void OnGameStateChanged(GameState from, GameState to)
    {
        switch (to)
        {
            case GameState.Null:
                break;
            case GameState.Initial:
                {
                    ChangePanel<InitialPanel>();
                }
                break;
            case GameState.Playing:
                {
                    ChangePanel<PlayingPanel>();
                }
                break;
            case GameState.Success:
                {
                    ChangePanel<CompletedPanel>();
                }
                break;
            case GameState.Failed:
                {
                    ChangePanel<FailedPanel>();
                }
                break;
            default:
                break;
        }
    }
}
