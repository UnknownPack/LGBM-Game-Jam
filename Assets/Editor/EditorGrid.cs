using src.New_Testing_Scripts.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridBlackList))]
public class EditorGrid : Editor
{
    public override void OnInspectorGUI()
    {
        GridBlackList script = (GridBlackList)target;

        for (int y = script.gridHeight - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < script.gridWidth; x++)
            {
                bool current = script.blacklist[x, y];
                bool newVal = GUILayout.Toggle(current, "", "Button", GUILayout.Width(25), GUILayout.Height(25));

                if (newVal != current)
                {
                    script.blacklist[x, y] = newVal;
                    EditorUtility.SetDirty(script);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
