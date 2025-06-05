using UnityEngine;
using UnityEngine.SocialPlatforms;

public class LLUserProfileGPGS : IUserProfile
{
	private string _id;

	private string _userName;

	private bool _isFriend;

	private string _avatarUrl;

	private Texture2D _image;

	public string id
	{
		get
		{
			return _id;
		}
	}

	public string userName
	{
		get
		{
			return _userName;
		}
	}

	public Texture2D image
	{
		get
		{
			return _image;
		}
	}

	public bool isFriend
	{
		get
		{
			return _isFriend;
		}
	}

	public UserState state
	{
		get
		{
			return UserState.Online;
		}
	}

	public LLUserProfileGPGS()
	{
	}

	public LLUserProfileGPGS(LLNativeUserProfile nativeUserProfile)
	{
		CopyFields(nativeUserProfile);
	}

	protected void CopyFields(LLNativeUserProfile nativeUserProfile)
	{
		_userName = nativeUserProfile.displayName;
		_id = nativeUserProfile.id;
		_avatarUrl = nativeUserProfile.iconImageUri;
	}
}
