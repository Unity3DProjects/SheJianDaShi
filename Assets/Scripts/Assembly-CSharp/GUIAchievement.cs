using UnityEngine;

public class GUIAchievement : MonoBehaviour
{
	private const byte DISABLE_R = 90;

	private const byte DISABLE_G = 0;

	private const byte DISABLE_B = 100;

	private const byte DISABLE_A = byte.MaxValue;

	private const float DISABLE_GRAYSCALE = 1f;

	private const string DISABLE_SHADER = "tk2d/BlendVertexColorGrayscaleColorize";

	private const string DISABLE_GRAYSCALE_PROPERTY = "_GrayscaleFactor";

	[SerializeField]
	private tk2dSprite iconSprite;

	[SerializeField]
	private tk2dSprite lockSprite;

	[SerializeField]
	private tk2dTextMesh countReached;

	[SerializeField]
	private tk2dTextMesh nameAchievement;

	[SerializeField]
	private tk2dTextMesh descriptionAchievemen;

	[SerializeField]
	private GUILayouter internallayouter;

	private MicroachievementType typeAchievement;

	private int reachedLevelAchievemnt;

	private MicroachievementInfo cachedInfo;

	private bool isInit;

	public bool IsReached
	{
		get
		{
			bool flag = false;
			if (isInit)
			{
				flag = cachedInfo.ReachedCount > 0;
				if (flag && !cachedInfo.isRepeating)
				{
					flag = reachedLevelAchievemnt <= cachedInfo.ReachedValue;
				}
			}
			return flag;
		}
	}

	public void Init(MicroachievementType type, int level, GUILayoutCell parentCell)
	{
		if (isInit)
		{
			return;
		}
		typeAchievement = type;
		reachedLevelAchievemnt = level;
		parentCell.LayoutHandlerObjects.Add(internallayouter.gameObject);
		Managers instance = SingletonMonoBehaviour<Managers>.Instance;
		cachedInfo = instance.CachedMicroachievementsManager.InfoByType(typeAchievement);
		nameAchievement.text = instance.CachedLocalisationManager.GetTextByKey(cachedInfo.localisationName);
		descriptionAchievemen.text = instance.CachedLocalisationManager.GetTextByKey(cachedInfo.localisationDescription);
		int num = 0;
		for (int i = 0; i < cachedInfo.targetValues.Length; i++)
		{
			if (reachedLevelAchievemnt >= cachedInfo.targetValues[i])
			{
				num = i;
			}
		}
		if (cachedInfo.targetValues.Length > 0)
		{
			string newValue = cachedInfo.targetValues[num].ToString();
			descriptionAchievemen.text = descriptionAchievemen.text.Replace("%", newValue);
			nameAchievement.text = nameAchievement.text + " " + ToRoman(num + 1);
		}
		iconSprite.SetSprite(cachedInfo.iconSpriteName[num]);
		iconSprite.CachedRenderer.material = new Material(iconSprite.CachedRenderer.material);
		iconSprite.CachedRenderer.material.shader = Shader.Find("tk2d/BlendVertexColorGrayscaleColorize");
		isInit = true;
		Check();
	}

	public void Check()
	{
		if (isInit)
		{
			if (IsReached)
			{
				lockSprite.gameObject.SetActive(false);
				countReached.gameObject.SetActive(true);
				countReached.text = cachedInfo.ReachedCount.ToString();
				iconSprite.color = Color.black;
				iconSprite.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0f);
			}
			else
			{
				lockSprite.gameObject.SetActive(true);
				countReached.gameObject.SetActive(false);
				iconSprite.color = new Color32(90, 0, 100, byte.MaxValue);
				iconSprite.CachedRenderer.material.SetFloat("_GrayscaleFactor", 0.7f);
			}
		}
	}

	public static string ToRoman(int number)
	{
		if (number >= 1000)
		{
			return "M" + ToRoman(number - 1000);
		}
		if (number >= 900)
		{
			return "CM" + ToRoman(number - 900);
		}
		if (number >= 500)
		{
			return "D" + ToRoman(number - 500);
		}
		if (number >= 400)
		{
			return "CD" + ToRoman(number - 400);
		}
		if (number >= 100)
		{
			return "C" + ToRoman(number - 100);
		}
		if (number >= 90)
		{
			return "XC" + ToRoman(number - 90);
		}
		if (number >= 50)
		{
			return "L" + ToRoman(number - 50);
		}
		if (number >= 40)
		{
			return "XL" + ToRoman(number - 40);
		}
		if (number >= 10)
		{
			return "X" + ToRoman(number - 10);
		}
		if (number >= 9)
		{
			return "IX" + ToRoman(number - 9);
		}
		if (number >= 5)
		{
			return "V" + ToRoman(number - 5);
		}
		if (number >= 4)
		{
			return "IV" + ToRoman(number - 4);
		}
		if (number >= 1)
		{
			return "I" + ToRoman(number - 1);
		}
		return string.Empty;
	}
}
