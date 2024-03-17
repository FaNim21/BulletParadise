using BulletParadise.Misc;
using UnityEngine;

namespace BulletParadise.Entities
{
    public class Portal : MonoBehaviour, IEnterable
    {
        public new string name;


        public void Enter()
        {
            Utils.Log($"Wszedl w portal: {name}");
        }

        public string GetSceneName()
        {
            return name;
        }
    }
}
