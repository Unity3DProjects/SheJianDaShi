using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class SettingsSetter : MonoBehaviour
{
	private const string UNLOCKED_ACTIVATIONS_KEY = "unlocked_activations";

	private const string IS_VS_COMP_TUTORIAL_FINISHED = "is_vs_comp_tutorial_finished";

	public const string MUSIC_CATEGORY = "Music";

	private const string MUSIC_KEY = "Music";

	private const string SOUNDS_KEY = "Sounds";

	private void Awake()
	{
		CustomPlayerPrefs.DeleteAll();
		List<TutorialType> list = new List<TutorialType>();
		list.Add(TutorialType.Aiming);
		list.Add(TutorialType.AlienProbe);
		list.Add(TutorialType.Boomerang);
		list.Add(TutorialType.Card);
		list.Add(TutorialType.Cat);
		list.Add(TutorialType.Cleaver);
		list.Add(TutorialType.Disk);
		list.Add(TutorialType.Dog);
		list.Add(TutorialType.FireShuriken);
		list.Add(TutorialType.FireworkArrow);
		list.Add(TutorialType.FrozenSpear);
		list.Add(TutorialType.Gamepad);
		list.Add(TutorialType.Ice);
		list.Add(TutorialType.KatnissArrow);
		list.Add(TutorialType.Kunai);
		list.Add(TutorialType.MagicArrow);
		list.Add(TutorialType.Mjolnir);
		list.Add(TutorialType.PickAxe);
		list.Add(TutorialType.Propeller);
		list.Add(TutorialType.Rocket);
		list.Add(TutorialType.Shooting);
		list.Add(TutorialType.Shovel);
		list.Add(TutorialType.Skull);
		list.Add(TutorialType.Aiming);
		list.Add(TutorialType.Shotgun);
		list.Add(TutorialType.Spinner);
		list.Add(TutorialType.Broomstick);
		list.Add(TutorialType.Turkey);
		CustomPlayerPrefs.SetString("unlocked_activations", Json.Serialize(list));
		CustomPlayerPrefs.SetBool("is_vs_comp_tutorial_finished", true);
		CustomPlayerPrefs.SetInt("Music", 0);
		CustomPlayerPrefs.SetInt("Sounds", 0);
	}

	private void Start()
	{
		SingletonMonoBehaviour<TutorialManager>.Instance.IsVSCompTutorialInProgress = false;
	}
}
