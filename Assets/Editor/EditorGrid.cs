#if UNITY_EDITOR
using src.New_Testing_Scripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridBlackListSo))]
public class EditorGridSO : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the ScriptableObject's default fields (width, height, layer count)
        DrawDefaultInspector();

        GridBlackListSo script = (GridBlackListSo)target;

        // Loop through each layer
        for (int l = 0; l < script.blacklists.Count; l++) {
            GUILayout.Space(10);
            GUILayout.Label($"Layer {l+1}", EditorStyles.boldLabel);

            var layer = script.blacklists[l];

            // ✅ Draw the LayerType enum dropdown
            layer.layerType = (LayerType)EditorGUILayout.EnumPopup("Layer Type", layer.layerType);

            // Draw the toggle grid
            for (int y = script.gridHeight - 1; y >= 0; y--) {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < script.gridWidth; x++) {

                    // ✅ Disable cell if ANY previous layer blocked it
                    bool locked = false;
                    for (int prev = 0; prev < l; prev++) {
                        if (script.blacklists[prev].cells[x, y]) {
                            locked = true;
                            break;
                        }
                    }

                    bool current = layer.cells[x, y];
                    EditorGUI.BeginDisabledGroup(locked);
                    bool newVal = GUILayout.Toggle(current, "", "Button", GUILayout.Width(25), GUILayout.Height(25));
                    EditorGUI.EndDisabledGroup();

                    if (!locked && newVal != current) {
                        layer.cells[x, y] = newVal;
                        EditorUtility.SetDirty(script);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif