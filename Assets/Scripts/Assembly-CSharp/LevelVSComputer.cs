using System;
using UnityEngine;

public class LevelVSComputer : Level
{
	[SerializeField]
	private int playerHealthPercentForLoseInEasyMatches = 60;

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		base.CharacterSecond.IsAI = true;
	}

	public override void EnableDragging()
	{
		if (base.ActiveCharacter == base.CharacterFirst)
		{
			arrowDragger.IsDraggingEnabled = true;
		}
		else
		{
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, EnemyShooting, 1f);
		}
	}

	public void ResetShotSettings()
	{
		if (base.ActiveCharacter.IsAI)
		{
			base.ActiveCharacter.ResetHittingSettings();
		}
	}

	public void ShowRevivePopup()
	{
		PopupType typePopup = PopupType.Revieve;
		GUIPopup gUIPopup = base.CachedManagers.CachedGUIPopupManager.ShowPopup(typePopup);
		gUIPopup.OnPopupHide = (Action<GUIPopup>)Delegate.Combine(gUIPopup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
	}

	private void EnemyShooting()
	{
		bool isPlayerLoses = base.InactiveArcher.HealthPercents <= playerHealthPercentForLoseInEasyMatches;
		ShootingInfo shootingInfo = base.ActiveCharacter.GetShootingInfo(isPlayerLoses);
		base.ActiveCharacter.SetCurrentAngleAndVelocity(shootingInfo, 1f, true);
		base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, ReleaseWeapon, 1f);
	}

	private void Popup_OnPopupHide(GUIPopup popup)
	{
		popup.OnPopupHide = (Action<GUIPopup>)Delegate.Remove(popup.OnPopupHide, new Action<GUIPopup>(Popup_OnPopupHide));
		base.CachedManagers.CachedGameManager.IsGamePaused = false;
		if (base.CharacterFirst.Health <= 0f)
		{
			base.CharacterFirst.RestoreArcher();
		}
	}
}
