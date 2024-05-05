#if UNITY_EDITOR
using BulletParadise.Components;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(HealthManager))]
public class HealthManagerEditor : Editor
{
    private bool _invulnerability = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        HealthManager healthManager = (HealthManager)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Switch Invulnerability"))
        {
            _invulnerability = !_invulnerability;
            healthManager.SetInvulnerability(_invulnerability);
        }

        EditorUtility.SetDirty(healthManager);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif