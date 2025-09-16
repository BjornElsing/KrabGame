namespace GameCreator.Core
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AddComponentMenu("")]
    public class CustomTransformRotate : IAction
    {
        public TargetGameObject target = new TargetGameObject(TargetGameObject.Target.Player);

        public Space space = Space.Self;

        public NumberProperty xAxis = new NumberProperty(0f);
		public NumberProperty yAxis = new NumberProperty(0f);
		public NumberProperty zAxis = new NumberProperty(0f);
        private Vector3 inputRotation;

        public NumberProperty duration = new NumberProperty(1.0f);
        public Easing.EaseType easing = Easing.EaseType.QuadInOut;

        private bool forceStop = false;

        // EXECUTABLE: ----------------------------------------------------------------------------

        public override IEnumerator Execute(GameObject target, IAction[] actions, int index)
        {
            this.forceStop = false;
            Transform targetTrans = this.target.GetTransform(target);

			this.inputRotation = new Vector3(
                this.xAxis.GetValue(target),
                this.yAxis.GetValue(target),
                this.zAxis.GetValue(target));

            if (targetTrans != null)
            {
                Quaternion rotation1 = targetTrans.rotation;
                Quaternion rotation2 = Quaternion.identity;
                switch (this.space)
                {
                    case Space.Self:
                        rotation2 = (
                            targetTrans.rotation *
                            Quaternion.Euler(inputRotation)
                        );
                        break;

                    case Space.World:
                        rotation2 = Quaternion.Euler(inputRotation);
                        break;
                }

                float vDuration = this.duration.GetValue(target);
                float initTime = Time.time;

                while (Time.time - initTime < vDuration && !this.forceStop)
                {
                    if (targetTrans == null) break;
                    float t = (Time.time - initTime) / vDuration;
                    float easeValue = Easing.GetEase(this.easing, 0.0f, 1.0f, t);

                    targetTrans.rotation = Quaternion.Lerp(
                        rotation1,
                        rotation2,
                        easeValue
                    );

                    yield return null;
                }

                if (!this.forceStop && targetTrans != null)
                {
                    targetTrans.rotation = rotation2;
                }
            }

            yield return 0;
        }

        public override void Stop()
        {
            this.forceStop = true;
        }

        // +--------------------------------------------------------------------------------------+
        // | EDITOR                                                                               |
        // +--------------------------------------------------------------------------------------+

#if UNITY_EDITOR

        public static new string NAME = "Custom/CustomTransformRotate";
        private const string NODE_TITLE = "Rotate to ({0}, {1}, {2}) in {3} Space";

        // PROPERTIES: ----------------------------------------------------------------------------

        private SerializedProperty spTarget;
        private SerializedProperty spSpace;
		private SerializedProperty spXAxis;
		private SerializedProperty spYAxis;
		private SerializedProperty spZAxis;
        private SerializedProperty spDuration;
        private SerializedProperty spEasing;

        // INSPECTOR METHODS: ---------------------------------------------------------------------

        public override string GetNodeTitle()
        {
            return string.Format(NODE_TITLE, this.xAxis, this.yAxis, this.zAxis, this.space);
        }

        protected override void OnEnableEditorChild()
        {
            this.spTarget = serializedObject.FindProperty("target");
            this.spSpace = serializedObject.FindProperty("space");
			this.spXAxis = serializedObject.FindProperty("xAxis");
			this.spYAxis = serializedObject.FindProperty("yAxis");
			this.spZAxis = serializedObject.FindProperty("zAxis");
            this.spDuration = serializedObject.FindProperty("duration");
            this.spEasing = serializedObject.FindProperty("easing");
        }

        protected override void OnDisableEditorChild()
        {
            this.spTarget = null;
            this.spSpace = null;
			this.spXAxis = null;
			this.spYAxis = null;
			this.spZAxis = null;
            this.spDuration = null;
            this.spEasing = null;
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            EditorGUILayout.PropertyField(this.spTarget);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spSpace);
			EditorGUILayout.PropertyField(this.spXAxis);
            EditorGUILayout.PropertyField(this.spYAxis);
            EditorGUILayout.PropertyField(this.spZAxis);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(this.spDuration);
            EditorGUILayout.PropertyField(this.spEasing);

            this.serializedObject.ApplyModifiedProperties();
        }

#endif
    }
}