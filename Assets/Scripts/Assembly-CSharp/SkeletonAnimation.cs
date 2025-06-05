using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using Spine;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Spine/SkeletonAnimation")]
public class SkeletonAnimation : SkeletonRenderer, ISkeletonAnimation
{
	public float timeScale = 1f;

	public bool loop;

	public Spine.AnimationState state;

	public bool ignoreMonoUpdate;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private UpdateBonesDelegate m__UpdateLocal;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private UpdateBonesDelegate m__UpdateWorld;

	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private UpdateBonesDelegate m__UpdateComplete;

	[SerializeField]
	private string _animationName;

	public Skeleton Skeleton
	{
		get
		{
			CheckValid();
			return skeleton;
		}
	}

	public Spine.AnimationState State
	{
		get
		{
			CheckValid();
			return state;
		}
	}

	public string AnimationName
	{
		get
		{
			CheckValid();
			TrackEntry current = state.GetCurrent(0);
			return (current != null) ? current.Animation.Name : null;
		}
		set
		{
			CheckValid();
			if (_animationName == value)
			{
				return;
			}
			_animationName = value;
			if (state != null)
			{
				if (value == null || value.Length == 0)
				{
					state.ClearTrack(0);
				}
				else
				{
					state.SetAnimation(0, value, loop);
				}
			}
		}
	}

	public event UpdateBonesDelegate UpdateLocal
	{
		add
		{
			_UpdateLocal += value;
		}
		remove
		{
			_UpdateLocal -= value;
		}
	}

	public event UpdateBonesDelegate UpdateWorld
	{
		add
		{
			_UpdateWorld += value;
		}
		remove
		{
			_UpdateWorld -= value;
		}
	}

	public event UpdateBonesDelegate UpdateComplete
	{
		add
		{
			_UpdateComplete += value;
		}
		remove
		{
			_UpdateComplete -= value;
		}
	}

	protected event UpdateBonesDelegate _UpdateLocal
	{
		add
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateLocal;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateLocal, (UpdateBonesDelegate)Delegate.Combine(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
		remove
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateLocal;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateLocal, (UpdateBonesDelegate)Delegate.Remove(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
	}

	protected event UpdateBonesDelegate _UpdateWorld
	{
		add
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateWorld;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateWorld, (UpdateBonesDelegate)Delegate.Combine(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
		remove
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateWorld;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateWorld, (UpdateBonesDelegate)Delegate.Remove(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
	}

	protected event UpdateBonesDelegate _UpdateComplete
	{
		add
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateComplete;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateComplete, (UpdateBonesDelegate)Delegate.Combine(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
		remove
		{
			UpdateBonesDelegate updateBonesDelegate = this.m__UpdateComplete;
			UpdateBonesDelegate updateBonesDelegate2;
			do
			{
				updateBonesDelegate2 = updateBonesDelegate;
				updateBonesDelegate = Interlocked.CompareExchange(ref this.m__UpdateComplete, (UpdateBonesDelegate)Delegate.Remove(updateBonesDelegate2, value), updateBonesDelegate);
			}
			while (updateBonesDelegate != updateBonesDelegate2);
		}
	}

	public override void Reset()
	{
		base.Reset();
		if (valid)
		{
			state = new Spine.AnimationState(base.SkeletonDataAsset.GetAnimationStateData());
			if (_animationName != null && _animationName.Length > 0)
			{
				state.SetAnimation(0, _animationName, loop);
				Update(0f);
			}
		}
	}

	public virtual void Update()
	{
		if (!ignoreMonoUpdate)
		{
			Update(Time.deltaTime);
		}
	}

	public virtual void Update(float deltaTime)
	{
		if (valid)
		{
			deltaTime *= timeScale;
			skeleton.Update(deltaTime);
			state.Update(deltaTime);
			state.Apply(skeleton);
			if (this._UpdateLocal != null)
			{
				this._UpdateLocal(this);
			}
			skeleton.UpdateWorldTransform();
			if (this._UpdateWorld != null)
			{
				this._UpdateWorld(this);
				skeleton.UpdateWorldTransform();
			}
			if (this._UpdateComplete != null)
			{
				this._UpdateComplete(this);
			}
		}
	}

	public override void LateUpdate()
	{
		if (!valid)
		{
			return;
		}
		if (state != null)
		{
			bool flag = false;
			ExposedList<TrackEntry>.Enumerator enumerator = state.tracks.GetEnumerator();
			while (enumerator.MoveNext())
			{
				TrackEntry current = enumerator.Current;
				if (current != null)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		base.LateUpdate();
	}

	public void ForceUpdate()
	{
		base.LateUpdate();
	}

	private void CheckValid()
	{
		if (!valid && (bool)base.SkeletonDataAsset)
		{
			Reset();
			Initialize();
		}
	}
}
