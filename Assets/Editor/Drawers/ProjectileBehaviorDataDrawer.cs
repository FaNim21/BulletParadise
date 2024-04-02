#if UNITY_EDITOR
using BulletParadise.Shooting.Factors;
using BulletParadise.Shooting;
using UnityEditor;
using UnityEngine;
using BulletParadise.Shooting.Weapons;

[CustomPropertyDrawer(typeof(ProjectileBehaviorData))]
public class ProjectileBehaviorDataDrawer : PropertyDrawer
{
    private const int _headersAmount = 3;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
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
            string propertyName = dataProperty.objectReferenceValue?.name;
            EditorGUI.PropertyField(singleLineRect, dataProperty);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            SerializedProperty behaviorFactoryProperty = property.FindPropertyRelative("behaviorFactory");
            propertyName += $" ({behaviorFactoryProperty.objectReferenceValue?.name})";
            EditorGUI.PropertyField(singleLineRect, behaviorFactoryProperty);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

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

            singleLineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            EditorGUI.LabelField(singleLineRect, "Data", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            ProjectileBehaviorFactory behaviorFactory = (ProjectileBehaviorFactory)property.FindPropertyRelative("behaviorFactory").objectReferenceValue;
            if (behaviorFactory != null)
            {
                if (property.serializedObject.targetObject is WeaponArc)
                {
                    EditorGUI.PropertyField(singleLineRect, property.FindPropertyRelative("angle"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }
                
                SerializedProperty additionalDataProperty = property.FindPropertyRelative("additionalData");

                if (behaviorFactory is ProjectileWaveBehaviorFactory)
                {
                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("frequency"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("amplitude"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("magnitude"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }
            }
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
        float height = _headersAmount + 1.1f + (_headersAmount * 0.5f) + 4;

        if (property.FindPropertyRelative("dataMultiplier").isExpanded)
            height += 3; // damage, lifetime, speed

        SerializedProperty behaviorFactoryProperty = property.FindPropertyRelative("behaviorFactory");
        if (behaviorFactoryProperty.objectReferenceValue != null)
        {
            ProjectileBehaviorFactory behaviorFactory = (ProjectileBehaviorFactory)behaviorFactoryProperty.objectReferenceValue;
            if (behaviorFactory is ProjectileWaveBehaviorFactory)
            {
                if (property.serializedObject.targetObject is WeaponArc)
                    height += 1; // angle

                height += 3; // frequency, amplitude, magnitude
            }
            else if (behaviorFactory != null)
                height += 1; // angle
        }

        return EditorGUIUtility.singleLineHeight * height;
    }
}
#endif