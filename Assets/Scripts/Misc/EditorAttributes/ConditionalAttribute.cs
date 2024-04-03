using System;
using UnityEngine;

namespace BulletParadise.Misc.EditorAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ConditionalAttribute : PropertyAttribute
    {
        public string conditionPropertyName;
        public string type;

        public ConditionalAttribute(string conditionPropertyName, string type)
        {
            this.conditionPropertyName = conditionPropertyName;
            this.type = type;
        }
    }
}
