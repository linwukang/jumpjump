using UnityEngine;

namespace Assets.Script
{
    public class RestartButton : MonoBehaviour
    {
        public GameObject GameManager;

        public void OnClick()
        {
            GameManager.GetComponent<GameManager>().Restart();
        }
    }
}