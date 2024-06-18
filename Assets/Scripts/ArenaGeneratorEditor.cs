using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ArenaGenerator))]
public class ArenaGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ArenaGenerator arenaGenerator = (ArenaGenerator)target;
        if (GUILayout.Button("Generate Arena"))
        {
            arenaGenerator.GenerateArenaFromEditor();
        }
    }
}