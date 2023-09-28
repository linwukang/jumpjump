using System;
using UnityEngine;

namespace Assets.Script
{
    public class Jumper : MonoBehaviour
    {
        /// <summary>
        /// ��������������¼�
        /// </summary>
        public event Action StartJump;


        /// <summary>
        /// ��Ծ����
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
        /// ��Ծ����
        /// </summary>
        private float _power;

        public float Power => _power;

        /// <summary>
        /// �������ֵ
        /// </summary>
        public float PowerMax = 1.0f;

        /// <summary>
        /// �����ٶ� (ÿ������)
        /// </summary>
        public float PowerDelta = 0.3f;

        private void Start()
        {
            _power = 0;
        }

        /// <summary>
        /// �ո���Ƿ���(����)
        /// </summary>
        private bool _spaceKeyDown = false;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !IsJumping)
            {
                // ���¿ո�����Ҳ�����Ծ״̬��ʱ
                _spaceKeyDown = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                // �ſ��ո��ʱ
                Jump();
                _power = 0.0f;
                _spaceKeyDown = false;
            }

            if (!_spaceKeyDown) return;
        
            // ����
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
        /// ��б�Ϸ���Ծ
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
