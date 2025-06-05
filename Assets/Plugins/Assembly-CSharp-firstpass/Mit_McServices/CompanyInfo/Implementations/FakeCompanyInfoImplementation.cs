using System.Diagnostics;
using UnityEngine;

namespace Mit_McServices.CompanyInfo.Implementations
{
	public class FakeCompanyInfoImplementation : MonoBehaviour, CompanyInfoImplementation
	{
		protected const bool LOGS_ARE_ENABLED = false;

		public void Initialize()
		{
		}

		public void DisplayCustomerSupportPage()
		{
		}

		public void DisplayMoreGamesPage()
		{
		}

		public void DisplayCorporateInfo()
		{
		}

		[Conditional("UNITY_EDITOR")]
		[Conditional("MC_DEBUG")]
		private static void Log(string logStr)
		{
		}
	}
}
