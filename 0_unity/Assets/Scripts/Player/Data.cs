using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player
{
    public class Data
    {
        #region Static
        private static int _NextID = -1;
        public static int NextID
        {
            get
            {
                _NextID += 1;
                return _NextID;
            }
        }

        public static Dictionary<int, Player.Data> Current;
        private static void Init_Current()
        {
            if (Player.Data.Current == null)
            {
                Player.Data.Current = new Dictionary<int, Player.Data>();
            }
        }
        #endregion

        #region References
        public GameObject GameObject;
        #endregion

        #region Members
        public int ID;
        public string Name;
        public State State;
        #endregion

        #region Constructor
        private void Construct(string name, GameObject gameObject)
        {
            // Setup statics
            Player.Data.Init_Current();

            State = new Player.State();

            // Setup references
            GameObject = gameObject;

            // Setup member
            ID = NextID;
            Name = name;

            // Initialize references
            GameObject.GetComponent<Player.PlayerBehaviour>().PlayerData = this;
            Player.Data.Current.Add(ID, this);
        }

        public Data(string name, GameObject gameObject)
        {
            Construct(name, gameObject);
        }
        public Data(string name)
        {
            Construct(name, (GameObject)GameObject.Instantiate(World.World.Behaviour.PlayerPrefab, Vector3.zero, Quaternion.identity));
        }
        #endregion
    }
}