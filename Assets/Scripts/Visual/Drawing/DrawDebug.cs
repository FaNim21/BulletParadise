using BulletParadise.World;
using System.Collections.Generic;
using UnityEngine;

namespace BulletParadise.Visual.Drawing
{
    public class DrawDebug : MonoBehaviour
    {
        private readonly List<IDrawable> _drawables = new();
        private IReadOnlyList<IDrawable> DrawablesReadOnly => _drawables;

        private readonly List<IDrawable> _sceneDrawables = new();
        private IReadOnlyList<IDrawable> SceneDrawablesReadOnly => _sceneDrawables;


        [Header("Values")]
        [SerializeField] private Material material;
        [SerializeField] private bool isDebugMode;

        [Header("Debug")]
        [SerializeField, ReadOnly] private int drawableCount;


        private void OnPostRender()
        {
            if (!isDebugMode || _drawables == null || _drawables.Count == 0) return;

            Draw();
        }
        private void Draw()
        {
            material.SetPass(0);

            for (int i = 0; i < DrawablesReadOnly.Count; i++)
            {
                var current = DrawablesReadOnly[i];
                if (!current.CanDraw) continue;

                current.Draw();
            }

            for (int i = 0; i < SceneDrawablesReadOnly.Count; i++)
            {
                var current = SceneDrawablesReadOnly[i];
                if (!current.CanDraw) continue;

                current.Draw();
            }
        }

        public void AddGlobalDrawable(IDrawable drawable)
        {
            _drawables.Add(drawable);
            UpdateDrawableCount();
        }
        public void RemoveGlobalDrawable(IDrawable drawable)
        {
            _drawables.Remove(drawable);
            UpdateDrawableCount();
        }

        public void AddSceneDrawable(IDrawable drawable)
        {
            _sceneDrawables.Add(drawable);
            UpdateDrawableCount();
        }
        public void RemoveSceneDrawable(IDrawable drawable)
        {
            _sceneDrawables.Remove(drawable);
            UpdateDrawableCount();
        }

        private void UpdateDrawableCount()
        {
            drawableCount = SceneDrawablesReadOnly.Count + DrawablesReadOnly.Count;
        }

        public void SwitchDebugMode()
        {
            isDebugMode = !isDebugMode;
            if (isDebugMode) GameManager.Instance.worldManager.RunCheated();
        }
        public bool AreDebugLinesVisible() => isDebugMode;
    }
}