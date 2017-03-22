using System.Linq;
using UnityEngine;
using UnityEditor;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;

[CustomActionEditor(typeof(SetProvider))]
public class CustomActionEditorTest : CustomActionEditor
{
    private SetProvider action;
    private string[] providerTitles;
    private OnlineMapsProvider.MapType activeType;

    public override void OnEnable()
    {
        action = target as SetProvider;
        providerTitles = OnlineMapsProvider.GetProvidersTitle();
        activeType = OnlineMapsProvider.FindMapType(action.mapType.Value);
    }

    public override bool OnGUI()
    {
        bool isDirty = false;

        EditorGUI.BeginChangeCheck();
        int index = EditorGUILayout.Popup("Provider", activeType.provider.index, providerTitles);
        if (EditorGUI.EndChangeCheck())
        {
            activeType = OnlineMapsProvider.GetProviders()[index].types[0];
            action.mapType.Value = activeType.fullID;
            isDirty = true;
        }

        if (activeType.provider.types.Length > 1)
        {
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup("Type", activeType.index, activeType.provider.types.Select(t => t.title).ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                activeType = activeType.provider.types[index];
                action.mapType.Value = activeType.fullID;
                isDirty = true;
            }
        }

        if (activeType.isCustom) EditField("customPattern");

        return isDirty || GUI.changed;
    }
}