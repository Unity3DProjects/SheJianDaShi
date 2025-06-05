using UnityEngine;

public class SingleSoundObject : SoundObject
{
	protected AudioSource source;

	protected AudioSource Source
	{
		get
		{
			return base.gameObject.GetRequiredComponent(ref source);
		}
	}

	protected override void ApplyMuteChange()
	{
		Source.mute = base.Mute;
	}

	protected override void ApplyLoopChange()
	{
		Source.loop = base.Loop;
	}

	protected override void ApplyPriorityChange()
	{
		Source.priority = base.Priority;
	}

	protected override void ApplyVolumeChange()
	{
		Source.volume = base.TotalVolume;
	}

	protected override void ApplyPitchChange()
	{
		Source.pitch = base.Pitch;
	}

	protected override void ApplyPanChange()
	{
		Source.panStereo = base.Pan;
	}

	protected override void ApplySpatialBlendChange()
	{
		Source.spatialBlend = base.SpatialBlend;
	}

	protected override void ApplySpreadChange()
	{
		Source.spread = base.Spread;
	}

	protected override void ApplyMinDistanceChange()
	{
		Source.minDistance = base.MinDistance;
	}

	protected override void ApplyMaxDistanceChange()
	{
		Source.maxDistance = base.MaxDistance;
	}

	protected override void ApplySubItem()
	{
		Source.clip = base.SubItem.clip;
	}

	protected override void Update()
	{
		if (Source != null)
		{
			isPlaying = Source.isPlaying;
		}
		base.Update();
	}

	public override void OnPlay()
	{
		float num = 0f;
		if (num > 0f)
		{
			Source.PlayDelayed(num);
		}
		else
		{
			Source.Play();
		}
	}

	protected override void ApplyPause()
	{
		if (base.IsPaused)
		{
			Source.Pause();
		}
		else
		{
			Source.UnPause();
		}
	}

	internal override void StopSound()
	{
		if ((bool)Source)
		{
			Source.clip = null;
			Source.Stop();
		}
		base.StopSound();
	}
}
