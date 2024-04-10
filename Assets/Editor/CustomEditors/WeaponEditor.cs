#if UNITY_EDITOR

using BulletParadise.Shooting;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(Weapon), true)]
public class WeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        Weapon weapon = (Weapon)target;

        if (EditorApplication.isPlaying && GUILayout.Button("UPDATE"))
            weapon.Initialize();

        EditorUtility.SetDirty(weapon);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif