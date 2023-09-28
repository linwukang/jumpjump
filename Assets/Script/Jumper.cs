using System;
using UnityEngine;

namespace Assets.Script
{
    public class Jumper : MonoBehaviour
    {
        /// <summary>
        /// 玩家起跳触发该事件
        /// </summary>
        public event Action StartJump;


        /// <summary>
        /// 跳跃方向
        /// </summary>
        private Vector3 _direction;

        public void ToLeft()
        {
            _direction = Vector3.left;
        }

        public void ToRight()
        {
            _direction = Vector3.forward;
        }

        /// <summary>
        /// 跳跃蓄力
        /// </summary>
        private float _power;

        public float Power => _power;

        /// <summary>
        /// 最大蓄力值
        /// </summary>
        public float PowerMax = 1.0f;

        /// <summary>
        /// 蓄力速度 (每秒增量)
        /// </summary>
        public float PowerDelta = 0.3f;

        private void Start()
        {
            _power = 0;
        }

        /// <summary>
        /// 空格键是否按下(长按)
        /// </summary>
        private bool _spaceKeyDown = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsJumping)
            {
                // 按下空格键并且不在跳跃状态中时
                _spaceKeyDown = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                // 放开空格键时
                Jump();
                _power = 0.0f;
                _spaceKeyDown = false;
            }

            if (!_spaceKeyDown) return;
        
            // 蓄力
            if (_power < PowerMax)
            {
                _power += PowerDelta * Time.deltaTime;
            }
            else
            {
                _power = PowerMax;
            }
        }

        /// <summary>
        /// 向斜上方跳跃
        /// </summary>
        private void Jump()
        {
            StartJump?.Invoke();

            var jumpForce = _power * 0.5f * (_direction.normalized + Vector3.up);

            GetComponent<Rigidbody>().AddForce(jumpForce, ForceMode.Impulse);
        }

        public bool IsJumping => !GetComponent<GroundedChecker>().IsGrounded;
    }
}
