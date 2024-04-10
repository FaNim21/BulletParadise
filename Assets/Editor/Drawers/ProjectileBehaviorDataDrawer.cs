#if UNITY_EDITOR
using BulletParadise.Shooting;
using UnityEditor;
using UnityEngine;
using BulletParadise.Shooting.Logic;

[CustomPropertyDrawer(typeof(ProjectileBehaviorData))]
public class ProjectileBehaviorDataDrawer : PropertyDrawer
{
    private const int _headersAmount = 3;

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

        var targetObject = property.boxedValue as ProjectileBehaviorData;

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

            /*SerializedProperty behaviorFactoryProperty = property.FindPropertyRelative("behaviorFactory");
            propertyName += $" ({behaviorFactoryProperty.objectReferenceValue?.name})";
            EditorGUI.PropertyField(singleLineRect, behaviorFactoryProperty);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            /*SerializedProperty logic = property.FindPropertyRelative("logicUpdate");
            Object obj = EditorGUI.ObjectField(singleLineRect, new GUIContent("logic"), logic.objectReferenceValue, typeof(Object), !EditorUtility.IsPersistent(logic.serializedObject.targetObject));
            if (EditorGUI.EndChangeCheck())
            {
                if (obj == null)
                    logic.objectReferenceValue = null;
                else if (typeof(IProjectileUpdater).IsAssignableFrom(obj.GetType()))
                    logic.objectReferenceValue = obj;
                else if (obj is GameObject gameObject)
                {
                    MonoBehaviour component = (MonoBehaviour)gameObject.GetComponent(typeof(IProjectileUpdater));
                    if (component != null)
                        logic.objectReferenceValue = component;
                }
            }
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            SerializedProperty physics = property.FindPropertyRelative("physicsUpdate");
            Object obj2 = EditorGUI.ObjectField(singleLineRect, new GUIContent("physics"), physics.objectReferenceValue, typeof(Object), !EditorUtility.IsPersistent(physics.serializedObject.targetObject));
            if (EditorGUI.EndChangeCheck())
            {
                if (obj2 == null)
                    physics.objectReferenceValue = null;
                else if (typeof(IProjectileUpdater).IsAssignableFrom(obj2.GetType()))
                    physics.objectReferenceValue = obj2;
                else if (obj2 is GameObject gameObject)
                {
                    MonoBehaviour component = (MonoBehaviour)gameObject.GetComponent(typeof(IProjectileUpdater));
                    if (component != null)
                        physics.objectReferenceValue = component;
                }
            }
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;*/

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

            singleLineRect.y += EditorGUIUtility.singleLineHeight * 0.5f;
            EditorGUI.LabelField(singleLineRect, "Data", headerStyle);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1f;

            /*SerializedProperty physicsUpdate = property.FindPropertyRelative("physicsUpdate");
            var scriptableObject = physicsUpdate.objectReferenceValue as ScriptableObject;
            if (scriptableObject != null && scriptableObject is IProjectileUpdater)
            {
                var fields = scriptableObject.GetType().GetFields();

                foreach (var field in fields)
                {
                    var value = field.GetValue(scriptableObject);

                    EditorGUI.PropertyField(singleLineRect, null);
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }
            }*/

            SerializedProperty logicType = property.FindPropertyRelative("logicType");
            SerializedProperty logicUpdate = property.FindPropertyRelative("logicUpdate");
            EditorGUI.PropertyField(singleLineRect, logicType, true);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            /*if (logicType.serializedObject.ApplyModifiedProperties())
            {
                targetObject.UpdateLogicFromEnum();
            }*/

            SerializedProperty physicsType = property.FindPropertyRelative("physicsType");
            SerializedProperty physicsUpdate = property.FindPropertyRelative("physicsUpdate");
            EditorGUI.PropertyField(singleLineRect, physicsType, true);
            singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

            /*if (physicsType.serializedObject.ApplyModifiedProperties())
            {
                targetObject.UpdatePhysicsFromEnum();
            }*/

            /*if (physicsType.serializedObject.ApplyModifiedProperties())
            {
                // Pobierz wartość enum.
                ProjectilePhysicsType enumValue = (ProjectilePhysicsType)physicsType.enumValueIndex;

                // Zależnie od wartości enum, wykonaj odpowiednie działania.
                switch (enumValue)
                {
                    case ProjectilePhysicsType.Straight:
                        obj = new ProjectileStraightPhysics();
                        //logicUpdate.objectReferenceValue = new 
                        break;
                    case ProjectilePhysicsType.Wave:
                        obj = new ProjectileWavePhysics();
                        // Wykonaj odpowiednie działania dla Value2.
                        break;
                        // Dodaj przypadki dla innych wartości enum, jeśli jest to konieczne.
                }
            }*/

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

            /*IProjectileUpdater physicsUpdate = (IProjectileUpdater)property.FindPropertyRelative("physicsUpdate").objectReferenceValue;
            if (physicsUpdate != null)
            {
                if (property.serializedObject.targetObject is WeaponArc)
                {
                    EditorGUI.PropertyField(singleLineRect, property.FindPropertyRelative("angle"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }

                SerializedProperty additionalDataProperty = property.FindPropertyRelative("additionalData");

                if (physicsUpdate is ProjectileWavePhysics)
                {
                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("frequency"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("amplitude"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;

                    EditorGUI.PropertyField(singleLineRect, additionalDataProperty.FindPropertyRelative("magnitude"));
                    singleLineRect.y += EditorGUIUtility.singleLineHeight * 1.1f;
                }
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
        float height = _headersAmount + 1.1f + (_headersAmount * 0.5f) + 5;

        if (property.FindPropertyRelative("dataMultiplier").isExpanded)
            height += 3; // damage, lifetime, speed

        height += logicFieldsAmount + physicsFieldsAmount;
        /*SerializedProperty physicsProperty = property.FindPropertyRelative("physicsUpdate");
        if (physicsProperty.objectReferenceValue != null)
        {
            IProjectileUpdater behaviorFactory = (IProjectileUpdater)physicsProperty.objectReferenceValue;
            if (behaviorFactory is ProjectileWavePhysics)
            {
                if (property.serializedObject.targetObject is WeaponArc)
                    height += 1; // angle

                height += 3; // frequency, amplitude, magnitude
            }
            else if (behaviorFactory != null)
                height += 1; // angle
        }*/

        return EditorGUIUtility.singleLineHeight * height;
    }
}
#endif