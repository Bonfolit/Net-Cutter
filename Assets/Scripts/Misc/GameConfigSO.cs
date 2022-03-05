using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resources", menuName = "GameConfig", order = 1)]
public class GameConfigSO : ScriptableObject
{
    public List<ColorMaterial> colorMaterials;
}

[System.Serializable]
public class ColorMaterial
{
    public BaloonColor color;

    public Material material;
}