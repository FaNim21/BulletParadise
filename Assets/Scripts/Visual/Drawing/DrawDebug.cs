using BulletParadise.World;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Visual.Drawing
{
    public class DrawDebug : MonoBehaviour
    {
        private readonly List<IDrawable> _drawables = new();
        private IReadOnlyList<IDrawable> _drawablesReadOnly => _drawables;

        private readonly List<IDrawable> _sceneDrawables = new();
        private IReadOnlyList<IDrawable> _sceneDrawablesReadOnly => _sceneDrawables;


        [Header("Values")]
        [SerializeField] private Material material;

        [Header("Debug")]
        [SerializeField] private bool isDebugMode;


        private void OnPostRender()
        {
            if (!isDebugMode || _drawables == null || _drawables.Count == 0) return;

            Draw();
        }

        private void Draw()
        {
            material.SetPass(0);

            for (int i = 0; i < _drawablesReadOnly.Count; i++)
            {
                var current = _drawablesReadOnly[i];
                if (!current.CanDraw) continue;

                current.Draw();
            }

            for (int i = 0; i < _sceneDrawablesReadOnly.Count; i++)
            {
                var current = _sceneDrawablesReadOnly[i];
                if (!current.CanDraw) continue;

                current.Draw();
            }
        }

        public void AddGlobalDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
        }
        public void RemoveGlobalDrawable(IDrawable drawable)
        {
            _drawables.Remove(drawable);
        }

        public void AddSceneDrawable(IDrawable drawable)
        {
            _sceneDrawables.Add(drawable);
        }
        public void RemoveSceneDrawable(IDrawable drawable)
        {
            _sceneDrawables.Remove(drawable);
        }

        public void SwitchDebugMode()
        {
            isDebugMode = !isDebugMode;
        }
    }
}