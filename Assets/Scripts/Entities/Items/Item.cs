﻿using UnityEngine;

namespace BulletParadise.Entities.Items
{
    public class Item : ScriptableObject
    {
        public int id = -1;
        public new string name;
        public Sprite sprite;

        public virtual void OnEnable() { }
    }
}
