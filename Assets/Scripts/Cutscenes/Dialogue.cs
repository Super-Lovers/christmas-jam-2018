using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue", order = 1)]
public class Dialogue : ScriptableObject {
    public Passage[] passages;
}