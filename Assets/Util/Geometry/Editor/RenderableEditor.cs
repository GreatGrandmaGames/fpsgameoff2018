using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Renderable), true)]
public class RenderableEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Render"))
        {
            ((Renderable)target).Render();
        }
    }
}
