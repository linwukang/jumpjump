using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Script
{
    /// <summary>
    /// 触地检测
    /// 在挂载的游戏对象的最底部中心位置，定义一个向下的射线。
    /// 判断射线是否与被被标记为"地面"的物体相交。
    /// </summary>
    public class GroundedChecker : MonoBehaviour
    {
        // 定义射线的长度
        public float RayDistance = 1.2f;

        /// <summary>
        /// 标记为"地面"的 tag
        /// </summary>
        public List<string> GroundTags = new();

        private Vector3 _foot;

        private void Start()
        {

            var max = GetComponent<Transform>().InverseTransformPoint(GetComponent<Collider>().bounds.max);
            var min = GetComponent<Transform>().InverseTransformPoint(GetComponent<Collider>().bounds.min);

            var footX = (max.x + min.x) / 2;
            var footZ = (max.z + min.z) / 2;
            var footY = min.y + 0.2f;

            _foot = new Vector3(footX, footY, footZ);
            // Debug.Log(_foot);
        }

        // 触地检测
        public bool IsGrounded
        {
            get
            {
                var ground = Ground;
                Debug.Log("IsGrounded: " + (ground != null));
                return ground != null;
            }
        }

        [CanBeNull]
        public Collider Ground
        {
            get
            {
                // 设置射线的起点和方向
                var rayStart = GetComponent<Transform>().TransformPoint(_foot);
                var rayDirection = Vector3.down;
                // 进行射线检测
                if (Physics.Raycast(rayStart, rayDirection, out var hit, RayDistance) &&
                    GroundTags.Contains(hit.collider.tag))
                {
                    return hit.collider;
                }

                return null;
            }
        }
    }
}
