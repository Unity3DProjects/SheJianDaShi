using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Spine
{
	public class TrackEntry
	{
		internal TrackEntry next;

		internal TrackEntry previous;

		internal Animation animation;

		internal bool loop;

		internal float delay;

		internal float time;

		internal float lastTime = -1f;

		internal float endTime;

		internal float timeScale = 1f;

		internal float mixTime;

		internal float mixDuration;

		internal float mix = 1f;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationState.StartEndDelegate m_Start;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationState.StartEndDelegate m_End;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationState.EventDelegate m_Event;

		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private AnimationState.CompleteDelegate m_Complete;

		public Animation Animation
		{
			get
			{
				return animation;
			}
		}

		public float Delay
		{
			get
			{
				return delay;
			}
			set
			{
				delay = value;
			}
		}

		public float Time
		{
			get
			{
				return time;
			}
			set
			{
				time = value;
			}
		}

		public float LastTime
		{
			get
			{
				return lastTime;
			}
			set
			{
				lastTime = value;
			}
		}

		public float EndTime
		{
			get
			{
				return endTime;
			}
			set
			{
				endTime = value;
			}
		}

		public float TimeScale
		{
			get
			{
				return timeScale;
			}
			set
			{
				timeScale = value;
			}
		}

		public float Mix
		{
			get
			{
				return mix;
			}
			set
			{
				mix = value;
			}
		}

		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				loop = value;
			}
		}

		public event AnimationState.StartEndDelegate Start
		{
			add
			{
				AnimationState.StartEndDelegate startEndDelegate = this.m_Start;
				AnimationState.StartEndDelegate startEndDelegate2;
				do
				{
					startEndDelegate2 = startEndDelegate;
					startEndDelegate = Interlocked.CompareExchange(ref this.m_Start, (AnimationState.StartEndDelegate)Delegate.Combine(startEndDelegate2, value), startEndDelegate);
				}
				while (startEndDelegate != startEndDelegate2);
			}
			remove
			{
				AnimationState.StartEndDelegate startEndDelegate = this.m_Start;
				AnimationState.StartEndDelegate startEndDelegate2;
				do
				{
					startEndDelegate2 = startEndDelegate;
					startEndDelegate = Interlocked.CompareExchange(ref this.m_Start, (AnimationState.StartEndDelegate)Delegate.Remove(startEndDelegate2, value), startEndDelegate);
				}
				while (startEndDelegate != startEndDelegate2);
			}
		}

		public event AnimationState.StartEndDelegate End
		{
			add
			{
				AnimationState.StartEndDelegate startEndDelegate = this.m_End;
				AnimationState.StartEndDelegate startEndDelegate2;
				do
				{
					startEndDelegate2 = startEndDelegate;
					startEndDelegate = Interlocked.CompareExchange(ref this.m_End, (AnimationState.StartEndDelegate)Delegate.Combine(startEndDelegate2, value), startEndDelegate);
				}
				while (startEndDelegate != startEndDelegate2);
			}
			remove
			{
				AnimationState.StartEndDelegate startEndDelegate = this.m_End;
				AnimationState.StartEndDelegate startEndDelegate2;
				do
				{
					startEndDelegate2 = startEndDelegate;
					startEndDelegate = Interlocked.CompareExchange(ref this.m_End, (AnimationState.StartEndDelegate)Delegate.Remove(startEndDelegate2, value), startEndDelegate);
				}
				while (startEndDelegate != startEndDelegate2);
			}
		}

		public event AnimationState.EventDelegate Event
		{
			add
			{
				AnimationState.EventDelegate eventDelegate = this.m_Event;
				AnimationState.EventDelegate eventDelegate2;
				do
				{
					eventDelegate2 = eventDelegate;
					eventDelegate = Interlocked.CompareExchange(ref this.m_Event, (AnimationState.EventDelegate)Delegate.Combine(eventDelegate2, value), eventDelegate);
				}
				while (eventDelegate != eventDelegate2);
			}
			remove
			{
				AnimationState.EventDelegate eventDelegate = this.m_Event;
				AnimationState.EventDelegate eventDelegate2;
				do
				{
					eventDelegate2 = eventDelegate;
					eventDelegate = Interlocked.CompareExchange(ref this.m_Event, (AnimationState.EventDelegate)Delegate.Remove(eventDelegate2, value), eventDelegate);
				}
				while (eventDelegate != eventDelegate2);
			}
		}

		public event AnimationState.CompleteDelegate Complete
		{
			add
			{
				AnimationState.CompleteDelegate completeDelegate = this.m_Complete;
				AnimationState.CompleteDelegate completeDelegate2;
				do
				{
					completeDelegate2 = completeDelegate;
					completeDelegate = Interlocked.CompareExchange(ref this.m_Complete, (AnimationState.CompleteDelegate)Delegate.Combine(completeDelegate2, value), completeDelegate);
				}
				while (completeDelegate != completeDelegate2);
			}
			remove
			{
				AnimationState.CompleteDelegate completeDelegate = this.m_Complete;
				AnimationState.CompleteDelegate completeDelegate2;
				do
				{
					completeDelegate2 = completeDelegate;
					completeDelegate = Interlocked.CompareExchange(ref this.m_Complete, (AnimationState.CompleteDelegate)Delegate.Remove(completeDelegate2, value), completeDelegate);
				}
				while (completeDelegate != completeDelegate2);
			}
		}

		internal void OnStart(AnimationState state, int index)
		{
			if (this.Start != null)
			{
				this.Start(state, index);
			}
		}

		internal void OnEnd(AnimationState state, int index)
		{
			if (this.End != null)
			{
				this.End(state, index);
			}
		}

		internal void OnEvent(AnimationState state, int index, Event e)
		{
			if (this.Event != null)
			{
				this.Event(state, index, e);
			}
		}

		internal void OnComplete(AnimationState state, int index, int loopCount)
		{
			if (this.Complete != null)
			{
				this.Complete(state, index, loopCount);
			}
		}

		public override string ToString()
		{
			return (animation != null) ? animation.name : "<none>";
		}
	}
}
