using BulletParadise.World;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Visual.Drawing
{
    public class DrawDebug : MonoBehaviour
    {
        private readonly List<IDrawable> _drawables = new();

        [Header("Values")]
        public Material material;

        [Header("Debug")]
        public bool isDebugMode;


        private void OnPostRender()
        {
            if (!isDebugMode || (_drawables == null || _drawables.Count == 0)) return;

            Draw();
        }

        public void Draw()
        {
            material.SetPass(0);

            for (int i = 0; i < _drawables.Count; i++)
            {
                var current = _drawables[i];
                if (!current.CanDraw) continue;

                current.Draw();
            }
        }

        public void AddDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }
        public void RemoveDrawable(IDrawable drawable)
        {
            _drawables.Remove(drawable);
        }
    }
}