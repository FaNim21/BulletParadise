#if UNITY_EDITOR
using BulletParadise.Player;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(PlayerController))]
public class PlayerControllerEditor : Editor
{
    private bool _gameMaster = false;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();

        PlayerController player = (PlayerController)target;

        if (EditorApplication.isPlaying && GUILayout.Button("Switch GM"))
        {
            _gameMaster = !_gameMaster;
            player.SwitchGM();
        }

        EditorUtility.SetDirty(player);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif