using BulletParadise.Player;

namespace BulletParadise.Constants
{
    public static class Consts
    {
        private static bool _isFocusedOnUI = false;
        public static bool IsFocusedOnUI
        {
            get { return _isFocusedOnUI; }
            set
            {
                _isFocusedOnUI = value;

                if (_isFocusedOnUI)
                    PlayerController.Instance.StopMovement();
            }
        }

        private static bool _isFocusedOnMainMenu = false;
        public static bool IsFocusedOnMainMenu
        {
            get { return _isFocusedOnMainMenu; }
            set
            {
                _isFocusedOnMainMenu = value;
            }
        }

        public const string GameVersion = "0.3.0a";
    }
}