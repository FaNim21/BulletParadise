using BulletParadise.Visual.Drawing;
using UnityEngine;

namespace BulletParadise.World
{
    public class SceneObject : MonoBehaviour, IDrawable
    {
        public BoxCollider2D boxCollider;

        public bool CanDraw { get => IsVisible(); }


        public virtual void Draw()
        {
            if (boxCollider == null) return;

            GLDraw.DrawBox((Vector2)transform.position + boxCollider.offset, boxCollider.size * transform.localScale, Color.green, 0.01f);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }
    }
}