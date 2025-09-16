namespace GameCreator.Core
{
    using GameCreator.Characters;
    using UnityEngine;

    [AddComponentMenu("")]
    public class CharacterIsMovingCondition : ICondition
    {
        public TargetCharacter character = new TargetCharacter();
        public bool Value;

        public override bool Check(GameObject target)
        {
            Character charTarget = this.character.GetCharacter(target);
            var motion = charTarget.GetCharacterMotion();
            return motion > 0 == Value;
        }

#if UNITY_EDITOR
        public static new string NAME = "Custom/Character Is Moving";
        public const string NODE_TITLE_TRUE = "Character is moving";
        public const string NODE_TITLE_FALSE = "Character is not moving";

        public override string GetNodeTitle()
        {
            if (Value)
            {
                return NODE_TITLE_TRUE;
            }
            else
            {
                return NODE_TITLE_FALSE;
            }
        }
#endif
    }
}
