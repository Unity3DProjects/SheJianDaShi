using System.Collections.Generic;
using UnityEngine;

public class NativeSoundObject : SoundObject
{
	private static int globalCounter;

	protected string uniqueID = string.Empty;

	protected Vector3 lastPosition;

	private static Dictionary<string, NativeSoundObject> playingSounds;

	static NativeSoundObject()
	{
		playingSounds = new Dictionary<string, NativeSoundObject>();
		LLSoundManager.OnStoppedSound -= LLCallback;
		LLSoundManager.OnStoppedSound += LLCallback;
	}

	public override void OnPlay()
	{
		string text = base.SubItem.clipref.name;
		int num = ++globalCounter;
		uniqueID = text + "__" + num;
		string streamingAssetPath = base.SubItem.clipref.StreamingAssetPath;
		Vector3 position = base.CachedTransfrom.position;
		if (base.OverrideZAxis)
		{
			position.z = base.ZAxisPosition;
		}
		LLSoundManager.PlaySound(uniqueID, streamingAssetPath, base.TotalVolume, base.Pitch, base.Loop, position, base.MinDistance, base.MaxDistance, base.SpatialBlend);
		Register(this, uniqueID);
	}

	protected override void ApplyPause()
	{
		LLSoundManager.PauseSound(uniqueID, base.IsPaused);
	}

	internal override void StopSound()
	{
		base.StopSound();
		LLSoundManager.StopSound(uniqueID);
		Unregister(this, uniqueID);
	}

	protected override void LateUpdate()
	{
		Vector3 position = base.transform.position;
		if (lastPosition != position)
		{
			base.IsDirty = true;
			lastPosition = position;
		}
		if (base.IsDirty)
		{
			Vector3 position2 = base.transform.position;
			if (base.OverrideZAxis)
			{
				position2.z = base.ZAxisPosition;
			}
			LLSoundManager.UpdateSound(uniqueID, base.TotalVolume, base.Pitch, base.Loop, position2, base.MinDistance, base.MaxDistance, base.SpatialBlend);
		}
		base.LateUpdate();
	}

	private static void LLCallback(string uid)
	{
		NativeSoundObject value;
		if (playingSounds.TryGetValue(uid, out value))
		{
			value.isPlaying = false;
			if (value.Callback != null)
			{
				value.Callback();
			}
		}
	}

	private static void Register(NativeSoundObject so, string uniqueID)
	{
		if (!playingSounds.ContainsKey(uniqueID))
		{
			playingSounds.Add(uniqueID, so);
		}
		else
		{
			CustomDebug.LogError("register Native Sound Object twice : " + so, so);
		}
	}

	private static void Unregister(NativeSoundObject so, string uniqueID)
	{
		if (playingSounds.ContainsKey(uniqueID))
		{
			playingSounds.Remove(uniqueID);
		}
	}
}
