using UnityEngine;
using System.Collections;

namespace World
{
    public class WorldBehaviour : MonoBehaviour
    {
        [HeaderAttribute("Prefabs")]
        public GameObject PlayerPrefab;

        [HeaderAttribute("Settings")]
        public float SampleSetting = 0.0f;

        public void Start()
        {
            World.Behaviour = this;
            Player.Data player = new Player.Data("name");
            player.GameObject.transform.position = new Vector3(0, 2, 0);
        }
    }

    public static class World
    {
        public static WorldBehaviour Behaviour;
    }
}
