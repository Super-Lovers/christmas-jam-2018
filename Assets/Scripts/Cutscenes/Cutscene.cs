using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Cutscene", order = 1)]
public class Cutscene : ScriptableObject {
    public string stage_name;
    public Dialogue dialogue;
}