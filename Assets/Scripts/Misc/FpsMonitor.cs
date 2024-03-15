using TMPro;
using UnityEngine;

namespace BulletParadise.Misc.Graphs
{
    public class FpsMonitor : MonoBehaviour
    {
        [Header("Comontents")]
        public TextMeshProUGUI textFpsValue;
        public TextMeshProUGUI textMsValue;

        [Header("Values")]
        public int updateRate = 4;

        [Header("Thresholds")]
        public int goodFpsThreshold = 60;
        public Color goodFpsThresholdColor;
        public Color badFpsThresholdColor;


        [Header("Debug")]
        [ReadOnly] public ushort currentFps;
        [ReadOnly] public float currentMs;

        private float _deltaTime;
        private int _frameCount;


        private void Update()
        {
            _deltaTime += Time.unscaledDeltaTime;
            _frameCount++;

            if (_deltaTime > 1f / updateRate)
            {
                currentFps = (ushort)Mathf.RoundToInt(_frameCount / _deltaTime);
                currentMs = _deltaTime / _frameCount * 1000f;

                textFpsValue.color = currentFps >= goodFpsThreshold ? goodFpsThresholdColor : badFpsThresholdColor;
                textFpsValue.SetText(currentFps.ToString());

                textMsValue.SetText(currentMs.ToString("0.0"));

                _deltaTime = 0;
                _frameCount = 0;
            }
        }
    }
}
