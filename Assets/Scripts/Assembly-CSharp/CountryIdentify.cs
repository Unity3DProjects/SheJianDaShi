using System;
using System.Collections;
using UnityEngine;

public class CountryIdentify : MonoBehaviour
{
	public const string SERVER_URL = "https://api.playgendary.com";

	public const string API_URI_GET_USER_COUNTRY = "/v1/info/checkcountry";

	public static IEnumerator CheckCountry(Action<bool> callback)
	{
		// WWW info = new WWW("https://api.playgendary.com/v1/info/checkcountry");
		yield return null;
		// bool result = true;
		// if (!bool.TryParse(info.text, out result))
		// {
		// 	result = true;
		// }
		if (callback != null)
		{
			callback(true);
		}
	}
}
