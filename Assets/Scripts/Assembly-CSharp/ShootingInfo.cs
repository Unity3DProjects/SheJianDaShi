public struct ShootingInfo
{
	public float angle;

	public float velocity;

	public ShootType type;

	public bool autoActivation;

	public ShootingInfo(float angle, float velocity, ShootType type = ShootType.None, bool autoActivation = false)
	{
		this.angle = angle;
		this.velocity = velocity;
		this.type = type;
		this.autoActivation = autoActivation;
	}
}
