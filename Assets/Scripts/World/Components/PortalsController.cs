using BulletParadise.Entities;
using UnityEngine;

namespace BulletParadise.World.Components
{
    public class PortalsController : MonoBehaviour
    {
        private EnterInformationWindow _portalWindow;


        private void Awake()
        {
            _portalWindow = GetComponentInChildren<EnterInformationWindow>();
        }

        public void EnterPortal()
        {
            _portalWindow.OnEnter();
        }
        public void ShowPortalWindow(Portal portal)
        {
            _portalWindow.Setup(portal);
        }

        public void HidePortalWindow()
        {
            _portalWindow.Exit();
        }
    }
}
