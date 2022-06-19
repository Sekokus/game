using UnityEngine;

[CreateAssetMenu(menuName = "Other/Boot Data", fileName = "BootData", order = 0)]
public class BootData : ScriptableObject
{
    public int afterBootScene = SceneLoader.MenuScene;
}