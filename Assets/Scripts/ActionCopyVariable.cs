namespace GameCreator.Core
{
    using UnityEngine;
    using GameCreator.Variables;

    [AddComponentMenu("")]
    public class ActionCopyVariable : IAction
    {
        public TargetGameObject from = new TargetGameObject(TargetGameObject.Target.GameObject);

        [Space]
        public string searchVariable = "my-variable";
        //variable on game object in from

        [Space]
        public VariableProperty to = new VariableProperty(Variable.VarType.LocalVariable);

        public override bool InstantExecute(GameObject target, IAction[] actions, int index)
        {

            GameObject fromGo = this.from.GetGameObject(target);
            if (fromGo == null) return true;

            LocalVariables[] locals = fromGo.GetComponentsInChildren<LocalVariables>();
            for (int i = 0; i < locals.Length; ++i)
            {

                if (VariablesManager.ExistsLocal(fromGo, this.searchVariable, true))
                {
                    object value = VariablesManager.GetLocal(fromGo, this.searchVariable, true);

// Debug.Log("Success:  "+ this.searchVariable );
// Debug.Log("-- search: "+this.searchVariable );
// Debug.Log("-- fromGo: "+fromGo);
// Debug.Log("-- value: "+value);
// Debug.Log("-- to: "+this.to);

                    this.to.Set(value, fromGo);
                    // this.to.Set(value, target);

                } else {
Debug.Log("Error ActionCopyVariable: missing matching variable ");
// Debug.Log("- -"+target);
// Debug.Log("-- search: "+this.searchVariable );

                }
            }

            return true;
        }

        #if UNITY_EDITOR

        public static new string NAME = "Variables/Copy Variable";
        private const string NODE_TITLE = "Copy Var {0} to {1}";

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.searchVariable, this.to);
        }

        #endif
    }
}
