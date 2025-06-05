using System.Diagnostics;
using Mit_McServices.CompanyInfo.Implementations;
using UnityEngine;

namespace Mit_McServices.CompanyInfo
{
	public class CompanyInfoManager : MonoBehaviour
	{
		private const bool LOGS_ARE_ENABLED = false;

		private CompanyInfoImplementation implementation;

		protected static CompanyInfoManager instance;

		public static CompanyInfoManager Instance
		{
			get
			{
				return instance;
			}
		}

		private void Awake()
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			Initialize();
		}

		protected void OnDestroy()
		{
			instance = null;
		}

		private void Initialize()
		{
			SetImplementation();
			implementation.Initialize();
		}

		private void SetImplementation()
		{
			implementation = base.gameObject.AddComponent<CApiCompanyInfoImplementation>();
		}

		public void DisplayCustomerSupportPage()
		{
			implementation.DisplayCustomerSupportPage();
		}

		public void DisplayMoreGamesPage()
		{
			implementation.DisplayMoreGamesPage();
		}

		public void DisplayCorporateInfo()
		{
			implementation.DisplayCorporateInfo();
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
