using System.Runtime.InteropServices;
using UnityEngine;

namespace Mit_McServices.CompanyInfo.Implementations
{
	public class CApiCompanyInfoImplementation : MonoBehaviour, CompanyInfoImplementation
	{
		[DllImport("mcprime")]
		public static extern void mc_connection_load_helper();

		[DllImport("mcservices")]
		public static extern void mc_displayCustomerSupportPage();

		[DllImport("mcservices")]
		public static extern void mc_displayMoreGamesPage();

		[DllImport("mcservices")]
		public static extern void mc_displayCorporateInfo();

		public void Initialize()
		{
			mc_connection_load_helper();
		}

		public void DisplayCustomerSupportPage()
		{
			mc_displayCustomerSupportPage();
		}

		public void DisplayMoreGamesPage()
		{
			mc_displayMoreGamesPage();
		}

		public void DisplayCorporateInfo()
		{
			mc_displayCorporateInfo();
		}
	}
}
