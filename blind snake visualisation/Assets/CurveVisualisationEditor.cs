using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(CurveVisualisation))]
public class CurveVisualisationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        CurveVisualisation curveVisualisation = (CurveVisualisation)target;
        if (GUILayout.Button("Regenerate"))
        {
            curveVisualisation.BaseTexture();
        }
    }
}
