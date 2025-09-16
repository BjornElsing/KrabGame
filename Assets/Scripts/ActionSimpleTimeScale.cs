namespace GameCreator.Core
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Variables;

	[AddComponentMenu("")]
	public class ActionSimpleTimeScale : IAction
	{
        public NumberProperty timeScale = new NumberProperty(1.0f);

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {
            float timeScaleValue = this.timeScale.GetValue(target);
            Time.timeScale = timeScaleValue;
            return true;
        }

        #if UNITY_EDITOR
        public static new string NAME = "General/Simple Time Scale (Krabba)";
		#endif
	}
}
