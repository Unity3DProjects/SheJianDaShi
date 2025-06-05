namespace Mit_McServices.Rate.Settings
{
	public struct RateTexts
	{
		private string title;

		private string message;

		private string laterButton;

		private string okButton;

		public string Title
		{
			get
			{
				return title;
			}
		}

		public string Message
		{
			get
			{
				return message;
			}
		}

		public string LaterButton
		{
			get
			{
				return laterButton;
			}
		}

		public string OkButton
		{
			get
			{
				return okButton;
			}
		}

		public RateTexts(string title, string message, string laterButton, string okButton)
		{
			this.title = title;
			this.message = message;
			this.laterButton = laterButton;
			this.okButton = okButton;
		}
	}
}
