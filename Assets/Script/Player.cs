using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace Assets.Script
{
    /// <summary>
    /// 玩家组件
    /// 实现得分数、游戏结束、新平台创建等功能
    /// </summary>
    public class Player : MonoBehaviour
    {

        /// <summary>
        /// 玩家跳到下一个平台则触发该事件
        /// </summary>
        public event Action<GameObject> ToNextPlatform;

        /// <summary>
        /// 游戏结束触发该事件
        /// </summary>
        public event Action GameOver;

        /// <summary>
        /// 玩家起跳触发该事件
        /// </summary>
        public event Action StartJump;

        /// <summary>
        /// 地面对象
        /// 玩家对象碰撞到地面时游戏结束
        /// </summary>
        public GameObject Ground;

        private void Start()
        {
            GetComponent<Jumper>().StartJump += () => StartJump?.Invoke();
        }

        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("发生碰撞, collider: " + collision.gameObject.name);

            if (collision.gameObject == Ground)
            {
                // 玩家落地, 游戏结束
                GameOver?.Invoke();
                return;
            }

            if (collision.gameObject.GetComponent<Platform>() == null) return;
            
            // 玩家接触到平台
            // if (!GetComponent<GroundedChecker>().IsGrounded)
            if (GetComponent<Jumper>().IsJumping)
            {
                // 玩家落在平台边缘
                GetComponent<Rigidbody>().freezeRotation = false;
                return;
            }

            ToNextPlatform?.Invoke(collision.gameObject);
        }
        
    }
}