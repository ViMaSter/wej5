using UnityEngine;
using System.Collections;

namespace Player
{
    public class MonoBehaviour : UnityEngine.MonoBehaviour
    {
        public Player.Data PlayerData;
        public void Start()
        {
            PlayerData = GetComponentInParent<PlayerBehaviour>().PlayerData;
        }
    }
}