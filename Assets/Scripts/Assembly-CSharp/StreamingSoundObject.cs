using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StreamingSoundObject : SingleSoundObject
{
	protected bool waitForLoading;

	public override void Play(float delay, float fadeIn = 0f)
	{
		waitForLoading = true;
		isPaused = true;
		if (base.IsMusic)
		{
			isPaused = true;
		}
		SingletonMonoBehaviour<SoundsManager>.Instance.StreamingAudioBuffer.LoadClip(base.SubItem.Name, base.SubItem.clipref.FullPath, delegate(AudioClip loadedAudioClip)
		{
			if (loadedAudioClip != null)
			{
				waitForLoading = false;
				source.clip = loadedAudioClip;
				_003CPlay_003E__BaseCallProxy0(delay, fadeIn);
			}
			else
			{
				CustomDebug.LogWarning("Loaded audio clip is null. Name : " + base.SubItem.clipref.name);
			}
		});
	}

	internal override bool IsFinish()
	{
		bool flag = base.IsFinish();
		if (flag && base.Callback != null)
		{
			base.Callback();
		}
		return flag;
	}

	[CompilerGenerated]
	[DebuggerHidden]
	private void _003CPlay_003E__BaseCallProxy0(float delay, float fadeIn)
	{
		base.Play(delay, fadeIn);
	}
}
