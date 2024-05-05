#if UNITY_EDITOR
using BulletParadise.Shooting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ProjectileBehaviorData))]
public class ProjectileBehaviorDataDrawer : PropertyDrawer
{
    private const int _headersAmount = 4;

    private int logicFieldsAmount;
    private int physicsFieldsAmount;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();
        Rect foldoutRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label);

        GUIStyle headerStyle = new()
        {
            fontStyle = FontStyle.Bold,
        };
        headerStyle.normal.textColor = Color.white;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            Rect singleLineRect = new(position.x, position.y + 10, position.width, EditorGUIUtility.singleLineHeight);

            singleLineRect.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(singleLineRect, "Components", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            SerializedProperty dataProperty = property.FindPropertyRelative("data");
            string propertyName = dataProperty.objectReferenceValue != null ? dataProperty.objectReferenceValue.name : null;
            EditorGUI.PropertyField(singleLineRect, dataProperty);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            #region Data Multipliers
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            EditorGUI.LabelField(singleLineRect, "Multipliers", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            SerializedProperty dataMultiplierProperty = property.FindPropertyRelative("dataMultiplier");
            Rect dataFoldoutRect = new(singleLineRect.x, singleLineRect.y, singleLineRect.width, EditorGUIUtility.singleLineHeight);
            dataMultiplierProperty.isExpanded = EditorGUI.Foldout(dataFoldoutRect, dataMultiplierProperty.isExpanded, dataMultiplierProperty.displayName);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
            if (dataMultiplierProperty.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(singleLineRect, dataMultiplierProperty.FindPropertyRelative("damageMultiplier"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                EditorGUI.PropertyField(singleLineRect, dataMultiplierProperty.FindPropertyRelative("lifeTimeMultiplier"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                EditorGUI.PropertyField(singleLineRect, dataMultiplierProperty.FindPropertyRelative("speedMultiplier"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                EditorGUI.indentLevel--;
            }
            #endregion

            SerializedProperty additionalDataProperty = property.FindPropertyRelative("additionalData");

            #region Logic Updates
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            EditorGUI.LabelField(singleLineRect, "Logic", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            SerializedProperty logicType = property.FindPropertyRelative("logicType");
            EditorGUI.PropertyField(singleLineRect, logicType, true);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("angle"));
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            if ((ProjectileLogicType)logicType.boxedValue == ProjectileLogicType.Rotating)
            {
                EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("rotationSpeed"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
            }
            #endregion

            #region Physics Updates
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            EditorGUI.LabelField(singleLineRect, "Physics", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            SerializedProperty physicsType = property.FindPropertyRelative("physicsType");
            EditorGUI.PropertyField(singleLineRect, physicsType, true);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            if ((ProjectilePhysicsType)physicsType.boxedValue == ProjectilePhysicsType.Wave)
            {
                EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("frequency"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("amplitude"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("magnitude"));
                singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
            }
            #endregion

            /*SerializedProperty physicsUpdate = property.FindPropertyRelative("physicsUpdate");
            var scriptableObject = physicsUpdate.objectReferenceValue as ScriptableObject;
            if (scriptableObject != null && scriptableObject is IProjectileUpdater)
            {
                var serializedObject = new SerializedObject(scriptableObject);
                var iterator = serializedObject.GetIterator();
                physicsFieldsAmount = 0;

                while (iterator.NextVisible(true))
                {
                    if (iterator.propertyPath == "m_Script" || iterator.propertyType == SerializedPropertyType.Generic)
                        continue;

                    EditorGUI.PropertyField(singleLineRect, iterator, true);
                    singleLineRect.y += EditorGUI.GetPropertyHeight(iterator, true) * 1.1f;
                    physicsFieldsAmount++;
                }
                serializedObject.ApplyModifiedProperties();
            }*/

            EditorGUI.indentLevel--;

            var nameProperty = property.FindPropertyRelative("name");
            int index = int.Parse(property.propertyPath[^2].ToString()) + 1;
            nameProperty.stringValue = propertyName + $" - {index}";
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
        float height = _headersAmount + 1.1f + (_headersAmount * 0.5f) + 6;

        if (property.FindPropertyRelative("dataMultiplier").isExpanded)
            height += 3; // damage, lifetime, speed

        height += logicFieldsAmount + physicsFieldsAmount;

        SerializedProperty logicType = property.FindPropertyRelative("logicType");
        if ((ProjectileLogicType)logicType.boxedValue == ProjectileLogicType.Rotating)
            height += 1; // rotationSpeed

        SerializedProperty physicsType = property.FindPropertyRelative("physicsType");
        if ((ProjectilePhysicsType)physicsType.boxedValue == ProjectilePhysicsType.Wave)
            height += 3; // frequency, amplitde, magnitude

        return EditorGUIUtility.singleLineHeight * height;
    }
}
#endif