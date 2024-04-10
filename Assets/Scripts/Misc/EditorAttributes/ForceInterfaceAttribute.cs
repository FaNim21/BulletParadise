using System;
using UnityEngine;

namespace BulletParadise.Misc.EditorAttributes
{
    public class ForceInterfaceAttribute : PropertyAttribute
    {
        public readonly Type interfaceType;

        public ForceInterfaceAttribute(Type interfaceType)
        {
            this.interfaceType = interfaceType;
        }
    }
}
