namespace GameCreator.Variables
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
    using GameCreator.Core;

	[AddComponentMenu("")]
	public class ConditionListVariableCountKrabba : ICondition
	{
        public enum Comparison
        {
            Equals,
            Different,
            LessThan,
            GreaterThan
        }

        public HelperListVariable listVariable = new HelperListVariable();

        [Space]
        public Comparison comparison = Comparison.Equals;
        [VariableFilter(Variable.DataType.Number)]
        public VariableProperty variable;
        private string variableString;
        private int count = 0;

        public override bool Check(GameObject target)
		{
            variableString = variable.Get().ToString();
            count = int.Parse(variableString);

            ListVariables list = this.listVariable.GetListVariables(target);
            if (list == null) return false;
            int listCount = list.variables.Count;

            switch (this.comparison)
            {
                case Comparison.Equals: return listCount == this.count;
                case Comparison.Different: return listCount != this.count;
                case Comparison.LessThan: return listCount < this.count;
                case Comparison.GreaterThan: return listCount > this.count;
            }

            return false;
        }
        
		#if UNITY_EDITOR
        public static new string NAME = "Variables/List Variables Count Krabba";

        private const string NODE_TITLE = "List Variable {0} {1} {2}";

       public override string GetNodeTitle()
        {
            return string.Format(
                NODE_TITLE,
                this.listVariable,
                this.comparison,
                this.variable
            );
        }

        #endif
    }
}
