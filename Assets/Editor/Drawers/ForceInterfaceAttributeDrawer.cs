#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using BulletParadise.Misc.EditorAttributes;

[CustomPropertyDrawer(typeof(ForceInterfaceAttribute))]
public class ForceInterfaceAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            ForceInterfaceAttribute forceAttribute = (ForceInterfaceAttribute)attribute;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            Object obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), !EditorUtility.IsPersistent(property.serializedObject.targetObject));
            if (EditorGUI.EndChangeCheck())
            {
                if (obj == null)
                    property.objectReferenceValue = null;
                else if (forceAttribute.interfaceType.IsAssignableFrom(obj.GetType()))
                    property.objectReferenceValue = obj;
                else if (obj is GameObject gameObject)
                {
                    MonoBehaviour component = (MonoBehaviour)gameObject.GetComponent(forceAttribute.interfaceType);
                    if (component != null)
                        property.objectReferenceValue = component;
                }
            }
            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.LabelField(position, "Use ForceInterfaceAttribute on Object");
        }
    }
}
#endif