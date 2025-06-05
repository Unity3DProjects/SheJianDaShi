using System;
using UnityEngine;

public class LevelFatalities : Level
{
	private Vector3 characterSecondStartPosition;

	public override void Init(ObjectsSpawner objectsSpawner, CameraController cameraController)
	{
		base.Init(objectsSpawner, cameraController);
		base.CharacterSecond.IsAI = true;
		characterSecondStartPosition = characterSecond.transform.position;
		if (characterSecond.Health > 2f)
		{
			CollisionInfo collisionInfo = new CollisionInfo();
			collisionInfo.damage = characterSecond.Health + 2f;
			collisionInfo.isDontApplyRagdoll = true;
			characterSecond.ApplyDamage(collisionInfo);
			characterSecond.RestoreArcher();
			characterSecond.SetAnimation(0, "finish_him", true);
		}
		activeCharacter = characterSecond;
	}

	public override void EnableDragging()
	{
		if (base.ActiveCharacter == base.CharacterFirst)
		{
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, Shoot, 1f);
		}
		else
		{
			base.CachedManagers.CachedScheduler.CallMethodWithDelay(this, Shoot, 1f);
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

	public override void SwitchActiveArcher()
	{
		int numberPlayer = 1;
		if (activeCharacter == characterFirst)
		{
			if (characterSecond != null)
			{
				activeCharacter = characterSecond;
				activeCharacter.SetCollidersEnabled(false);
				characterFirst.SetCollidersEnabled(true);
				numberPlayer = 2;
			}
		}
		else
		{
			activeCharacter = characterFirst;
			activeCharacter.SetCollidersEnabled(false);
			characterSecond.SetCollidersEnabled(true);
		}
		characterSecond.transform.position = characterSecondStartPosition;
		PlayerChanged(numberPlayer);
	}

	protected override void EndLevel(Character loseCharacter)
	{
		SingletonMonoBehaviour<GameManager>.Instance.StartTutorialMatch();
	}

	private void Shoot()
	{
		ShootingInfo info = default(ShootingInfo);
		if (base.CharacterSecond.transform.position.y < base.CharacterFirst.transform.position.y)
		{
			info.angle = 0f - Vector2.Angle(Vector2.right, base.CharacterSecond.transform.position - base.CharacterFirst.transform.position);
		}
		else
		{
			info.angle = Vector2.Angle(Vector2.right, base.CharacterSecond.transform.position - base.CharacterFirst.transform.position);
		}
		info.velocity = 2500f;
		base.ActiveCharacter.SetCurrentAngleAndVelocity(info, 1f, true);
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
