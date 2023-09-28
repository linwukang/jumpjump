using UnityEngine;

namespace Assets.Script
{
    /// <summary>
    /// 蓄力动画
    /// </summary>
    public class ChargedAnimation : MonoBehaviour
    {
        /// <summary>
        /// 最小缩小比例
        /// </summary>
        public float _minReduction;

        public GameObject Player;

        private Jumper _jumper;
        private float _originalScaleY;

        private void Start()
        {
            _jumper = Player.GetComponent<Jumper>();
            _originalScaleY = transform.localScale.y;
        }

        public void Update()
        {
            var scaleY = (_jumper.PowerMax - _jumper.Power) / _jumper.PowerMax;
            scaleY = scaleY >= _minReduction ? scaleY : _minReduction;
            scaleY = scaleY <= 1.0f ? scaleY : 1.0f;

            var localScale = transform.localScale;
            localScale.y = scaleY * _originalScaleY;

            transform.localScale = localScale;
        }

    }
}