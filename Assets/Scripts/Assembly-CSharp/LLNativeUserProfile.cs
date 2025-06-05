public struct LLNativeUserProfile
{
	private const string LOG_FORMAT_STRING = "[LLNativeUserProfile] id = {0}, name = {1}, displayName = {2}, title = {3}, iconImageUrl = {4}, hiResImageUrl = {5}";

	public string id;

	public string name;

	public string displayName;

	public string title;

	public string iconImageUri;

	public string hiResImageUri;

	public override string ToString()
	{
		return string.Format("[LLNativeUserProfile] id = {0}, name = {1}, displayName = {2}, title = {3}, iconImageUrl = {4}, hiResImageUrl = {5}", id, name, displayName, title, iconImageUri, hiResImageUri);
	}
}
