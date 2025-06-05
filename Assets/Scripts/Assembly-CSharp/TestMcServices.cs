using Mit_McServices.CompanyInfo;
using UnityEngine;

public class TestMcServices : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 200f, 50f), "CompanyInfo"))
		{
			CompanyInfoManager.Instance.DisplayCorporateInfo();
		}
		if (GUI.Button(new Rect(300f, 10f, 200f, 50f), "MoreGames"))
		{
			CompanyInfoManager.Instance.DisplayMoreGamesPage();
		}
		if (GUI.Button(new Rect(10f, 100f, 200f, 50f), "Newsfeed"))
		{
		}
		if (GUI.Button(new Rect(10f, 200f, 200f, 50f), "RateIt"))
		{
			GameRate.Instance.ShowPopupOnConditions();
		}
	}
}
