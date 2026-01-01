#if UNITY_EDITOR
using UnityEditor;
using Project.Editor.ManagedRef;
using Project.Features.Dialogue.SO;


[CustomEditor(typeof(DialogueScriptAsset))]
public class DialogueScriptAssetEditor : Editor
{
    private ManagedRefListDrawer _nodesDrawer;

    void OnEnable()
    {
        var nodesProp = serializedObject.FindProperty("Nodes");
        _nodesDrawer = new ManagedRefListDrawer(
            target,
            serializedObject,
            nodesProp,
            typeof(DialogueScriptAsset.NodeDef),
            "Nodes"
        );
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // draw Id normally
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Id"));

        // draw Nodes with type dropdown
        _nodesDrawer.DoLayout();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
