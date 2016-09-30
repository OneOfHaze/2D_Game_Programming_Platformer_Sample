using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameProgramming2D.State
{
    public abstract class StateBase
    {
        protected Dictionary<TransitionType, StateType> TransitionStateMap { get; set; }
        public StateType State { get; protected set;}

        protected StateBase()
        {
            TransitionStateMap = new Dictionary<TransitionType, StateType> ();
        }

        public bool AddTransition(TransitionType transition, StateType targetState)
        {
            if(transition == TransitionType.Error ||targetState == StateType.Error )
            {
                return false;
            }

            if(TransitionStateMap.ContainsKey(transition))
            {
                return false;
            }

            TransitionStateMap.Add ( transition, targetState );
            return true;
        }

        public bool RemoveTransition(TransitionType transition)
        {
            return TransitionStateMap.Remove ( transition );
        }

        public StateType GetTargetStateType(TransitionType transition)
        {
            if(TransitionStateMap.ContainsKey(transition))
            {
                return TransitionStateMap[transition];
            }
            return StateType.Error;
        }

        public virtual void StateActivated () { }
        public virtual void StateDeactivating () { }
    }
}
