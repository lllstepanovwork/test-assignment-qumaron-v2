using System;
using OleksiiStepanov.Game;
using UnityEngine;

namespace OleksiiStepanov.Loading
{
	/// <summary>
	/// Loading step base class - use this to create new steps
	/// </summary>
	public abstract class LoadingStepBase
	{
		public event Action Exited;

		public abstract void Enter();

		public virtual void Retry()
        {
			Debug.Log("[LoadingStep] Retrying step: " + GetStepType().ToString());
			Enter();
        }

		public virtual void Exit()
		{
			if (Exited != null)
			{
				Exited.Invoke();
			}
		}

		public abstract LoadingStep GetStepType();
	}
}