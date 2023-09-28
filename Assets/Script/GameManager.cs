using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script
{
    public class GameManager : MonoBehaviour
    {
        public uint TotalScore => _totalScore;

        /// <summary>
        /// 第一个平台
        /// </summary>
        public GameObject FirstPlatform;

        /// <summary>
        /// 现存的所有平台的队列
        /// </summary>
        private readonly Queue<GameObject> _platforms = new();

        /// <summary>
        /// 玩家对象
        /// </summary>
        public GameObject Player;

        /// <summary>
        /// 玩家视角镜头
        /// </summary>
        public Camera PlayerCamera;

        public uint MinScore = 1u;
        public uint MaxScore = 1u;

        /// <summary>
        /// 下一个平台的最小距离
        /// </summary>
        public float MinNextPlatformDistance = 5.0f;

        /// <summary>
        /// 下一个平台的最大距离
        /// </summary>
        public float MaxNextPlatformDistance = 10.0f;

        /// <summary>
        /// 平台预设体
        /// </summary>
        public List<GameObject> PlatformPrefabs = new();

        /// <summary>
        /// 地面对象
        /// 玩家对象碰撞到地面时游戏结束
        /// </summary>
        public GameObject Ground;

        /// <summary>
        /// 游戏结束 UI
        /// </summary>
        public GameObject GameOverCanvas;


        /// <summary>
        /// 玩家当前总得分
        /// </summary>
        private uint _totalScore = 0u;

        /// <summary>
        /// 当前玩家所在平台
        /// </summary>
        private GameObject _currentPlatform;

        /// <summary>
        /// 玩家下一个所要到达的平台
        /// </summary>
        private GameObject _nextPlatform;

        /// <summary>
        /// 上一个平台
        /// </summary>
        private GameObject _lastPlatform;

        private TimeSpan _jumpingDuration;
        private bool _isJumping;

        private void Start()
        {
            _totalScore = 0u;
            _currentPlatform = FirstPlatform;
            _platforms.Enqueue(_currentPlatform);
            _jumpingDuration = TimeSpan.Zero;
            _isJumping = false;

            Player.GetComponent<Player>().ToNextPlatform += OnToNextPlatform;
            Player.GetComponent<Player>().GameOver += OnGameOver;
            Player.GetComponent<Player>().StartJump += () =>
            {
                _isJumping = true;
            };


            NewPlatform();
        }

        private void Update()
        {
            if (_isJumping)
            {
                _jumpingDuration += TimeSpan.FromSeconds(Time.deltaTime);
            }

            if (_jumpingDuration.TotalSeconds >= 4.0)
            {
                GameOver();
            }
        }

        private void OnGameOver()
        {
            // 玩家落地, 游戏结束
            Debug.Log($"游戏结束, 总得分: {TotalScore}");
            GameOver();
        }

        private void OnToNextPlatform(GameObject nextPlatform)
        {
            if (_currentPlatform == nextPlatform) return;

            _lastPlatform = _currentPlatform;
            _currentPlatform = nextPlatform;

            var score = GiveScore();
            Debug.Log($"得分: +{score}");

            GameObject.Find("ScoreText").GetComponent<Text>().text = TotalScore.ToString();
            
            _isJumping = false;
            _jumpingDuration = TimeSpan.Zero;
            
            // 下一阶段
            NextStage();
        }

        private uint GiveScore()
        {
            _totalScore += MinScore;
            return MinScore;
        }

        private void NextStage()
        {
            // 生成新平台
            NewPlatform();
            // 移动镜头到玩家所在平台
            MoveCameraToCurrentPlatform();

            // 当平台数量高于 10 的时候, 销毁一个最先创建的平台
            if (_platforms.Count >= 10)
            {
                Destroy(_platforms.Dequeue());
            }
        }

        private readonly System.Random _random = new();
        /// <summary>
        /// 生成新平台
        /// </summary>
        private void NewPlatform()
        {
            // 随机选择平台预设体
            var platformPrefab = PlatformPrefabs[_random.Next(0, PlatformPrefabs.Count)];
            // 随机选择距离
            var distance = (float)_random.NextDouble() * (MaxNextPlatformDistance - MinNextPlatformDistance) + MinNextPlatformDistance;

            var nextPlatformPosition = _currentPlatform.GetComponent<Transform>().position;
            var nextPlatformRotation = _currentPlatform.GetComponent<Transform>().rotation;
            // 随机选择方向
            if (_random.Next(2) % 2 == 0)
            {
                // 向左
                Debug.Log("向左");
                nextPlatformPosition.x -= distance;
                Player.GetComponent<Jumper>().ToLeft();
            }
            else
            {
                // 向右
                Debug.Log("向右");
                nextPlatformPosition.z += distance;
                Player.GetComponent<Jumper>().ToRight();
            }

            _nextPlatform = Instantiate(platformPrefab, nextPlatformPosition, nextPlatformRotation);
            _platforms.Enqueue(_nextPlatform);
        }

        /// <summary>
        /// 将摄像头移动至当前平台
        /// </summary>
        private void MoveCameraToCurrentPlatform()
        {
            var transformPosition = _currentPlatform.transform.position - _lastPlatform.transform.position;

            StartCoroutine(MoveCamera(PlayerCamera.transform.position + transformPosition, TimeSpan.FromSeconds(0.5)));
        }

        private IEnumerator MoveCamera(Vector3 targetPosition, TimeSpan duration)
        {
            if (duration.TotalSeconds == 0)
            {
                PlayerCamera.transform.position = targetPosition;
                yield break;
            }

            var startPosition = PlayerCamera.transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < duration.TotalSeconds)
            {
                PlayerCamera.transform.position = Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    (float)(elapsedTime / duration.TotalSeconds));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            PlayerCamera.transform.position = targetPosition;
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void GameOver()
        {
            // 显示 UI
            GameOverCanvas.SetActive(true);

            // todo
            // 显示总得分文本
            GameObject.Find("TotalScore").GetComponent<Text>().text = "总得分: " + TotalScore;
            
        }

        /// <summary>
        /// 重新加载场景
        /// </summary>
        public void Restart()
        {
            Debug.Log("重新加载场景");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}