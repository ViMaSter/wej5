using UnityEngine;
using System.Collections;

namespace Player
{
    public class State
    {
        #region Condition
        public enum Condition
        {
            Alive,
            Dead
        }

        public class ConditionChangedEventArgs : System.EventArgs
        {
            public Condition Condition { get; set; }
            public ConditionChangedEventArgs(Condition condition)
            {
                Condition = condition;
            }
        }
        public delegate void ConditionChangedHandler(object sender, ConditionChangedEventArgs e);
        public event ConditionChangedHandler CurrentConditionChanged;
        private Condition _CurrentCondition;
        public Condition CurrentCondition
        {
            get
            {
                return _CurrentCondition;
            }
            set
            {
                if (CurrentConditionChanged != null)
                {
                    CurrentConditionChanged(this, new ConditionChangedEventArgs(value));
                }
                _CurrentCondition = value;
            }
        }
        #endregion
    }
}