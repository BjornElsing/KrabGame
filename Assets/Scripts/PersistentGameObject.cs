namespace GameCreator.Characters
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.AI;
	using UnityEngine.SceneManagement;
	using GameCreator.Core;
	using System;

	public class PersistentGameObject : GlobalID, IGameSave
	{
		[Serializable]
		public class SaveData
		{
			public Vector3 position = Vector3.zero;
			public Quaternion rotation = Quaternion.identity;
			public bool enabled = true;
		}

		[Serializable]
		public class OnLoadSceneData
		{
			public bool active { get; private set; }
			public Vector3 position { get; private set; }
			public Quaternion rotation { get; private set; }
			public bool enabled { get; private set; }


			public OnLoadSceneData(Vector3 position, Quaternion rotation)
			{
				this.active = true;
				this.position = position;
				this.rotation = rotation;
				this.enabled = true;
			}

			public void Consume()
			{
				this.active = false;
			}
		}
		
		public bool savePosition, saveRotation, saveEnabled;
		protected SaveData initSaveData = new SaveData();

		// INITIALIZERS: --------------------------------------------------------------------------

		protected override void Awake()
		{
			base.Awake();

			if (!Application.isPlaying) return;

			this.initSaveData = new SaveData()
			{
				position = transform.position,
				rotation = transform.rotation,
				enabled = this.enabled
			};

			if (this.savePosition || this.saveRotation || this.saveEnabled)
			{
				SaveLoadManager.Instance.Initialize(this);
			}
		}

		protected void OnDestroy()
		{
			this.OnDestroyGID();
			if (!Application.isPlaying) return;

			if ((this.savePosition || this.saveRotation || this.saveEnabled) && !this.exitingApplication)
			{
				if(SaveLoadManager.Instance!=null)
					SaveLoadManager.Instance.OnDestroyIGameSave(this);
			}
		}

		// GAME SAVE: -----------------------------------------------------------------------------

		public string GetUniqueName()
		{
			string uniqueName = string.Format(
				"GameObject:{0}",
				this.GetUniqueObjectID()
			);

			return uniqueName;
		}

		protected virtual string GetUniqueObjectID()
		{
			return this.GetID();
		}

		public Type GetSaveDataType()
		{
			return typeof(SaveData);
		}

		public object GetSaveData()
		{
			Debug.Log("GET " + name);
			return new SaveData()
			{
				position = transform.position,
				rotation = transform.rotation,
				enabled = gameObject.activeSelf
			};

		}

		public void ResetData()
		{
			transform.position = this.initSaveData.position;
			transform.rotation = this.initSaveData.rotation;
		}

		public void OnLoad(object generic)
		{
			SaveData container = generic as SaveData;
			if (container == null) return;

			Debug.Log("LOAD " + name);

			if (savePosition)
				transform.position = container.position;
			if(saveRotation)
				transform.rotation = container.rotation;
			if (saveEnabled)
				gameObject.SetActive(container.enabled);
		}
	}
}
