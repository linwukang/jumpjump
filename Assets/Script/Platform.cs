using UnityEngine;

namespace Assets.Script
{
    public class Platform : MonoBehaviour
    {
        private Vector3 _center;

        private void Start()
        {
            tag = "Platform";
            var max = GetComponent<Collider>().bounds.max;
            var min = GetComponent<Collider>().bounds.min;
            _center = new Vector3(
                (max.x + min.x) / 2.0f,
                max.y,
                (max.z + min.z) / 2.0f);
        }

        /// <summary>
        /// 获取中心点的世界坐标
        /// </summary>
        public Vector3 Center => GetComponent<Transform>().TransformPoint(_center);
    }
}