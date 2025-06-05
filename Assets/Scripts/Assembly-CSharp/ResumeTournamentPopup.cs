using System;
using UnityEngine;

public class ResumeTournamentPopup : GUIPopup
{
	[SerializeField]
	private tk2dSprite avatarSprite;

	[SerializeField]
	private tk2dUIItem newButton;

	[SerializeField]
	private tk2dUIItem resumeButton;

	[SerializeField]
	private tk2dTextMesh characterNameLabel;

	[SerializeField]
	private tk2dTextMesh rankLabel;

	public TournamentResumeType ResumeType { get; private set; }

	protected override void OnEnable()
	{
		newButton.OnClick += NewButton_OnClick;
		resumeButton.OnClick += ResumeButton_OnClick;
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		newButton.OnClick -= NewButton_OnClick;
		resumeButton.OnClick -= NewButton_OnClick;
		base.OnDisable();
	}

	public override void Show(Action<GUIControl> onFinished, bool immediately, float delay, bool callInvokes)
	{
		Action<GUIControl> onFinished2 = onFinished;
		GUICharacter gUICharacter = base.ObjectsSpawner.GetGUICharacter(SingletonMonoBehaviour<TournamentManager>.Instance.PlayerCharacter);
		if (gUICharacter != null)
		{
			if (gUICharacter != null && !string.IsNullOrEmpty(gUICharacter.IconAvatarName))
			{
				avatarSprite.SetSprite(gUICharacter.SpriteCollectionsData[0], gUICharacter.IconAvatarName);
				characterNameLabel.text = base.ObjectsSpawner.GetCharacterLocalizedName(SingletonMonoBehaviour<TournamentManager>.Instance.PlayerCharacter);
				rankLabel.text = (SingletonMonoBehaviour<TournamentManager>.Instance.Enemies.Count - SingletonMonoBehaviour<TournamentManager>.Instance.CurrentTournament.currentEnemyNumber).ToString();
			}
		}
		else
		{
			onFinished2 = delegate(GUIControl control)
			{
				if (onFinished != null)
				{
					onFinished(control);
				}
				SingletonMonoBehaviour<GUIPopupManager>.Instance.HideActivePopup();
			};
		}
		base.Show(onFinished2, immediately, delay, callInvokes);
	}

	private void ResumeButton_OnClick()
	{
		ResumeType = TournamentResumeType.Resume;
		Hide();
	}

	private void NewButton_OnClick()
	{
		ResumeType = TournamentResumeType.New;
		Hide();
	}
}
