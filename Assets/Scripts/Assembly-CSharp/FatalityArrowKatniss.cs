using UnityEngine;

public class FatalityArrowKatniss : FatalityBrick
{
	private void OnCollisionEnter(Collision collision)
	{
		base.transform.parent = collision.gameObject.transform;
		base.gameObject.GetComponent<Collider>().enabled = false;
		base.gameObject.GetComponent<Rigidbody>().isKinematic = true;
	}

	protected override void Start()
	{
		base.transform.rotation = Quaternion.Euler(base.transform.rotation.eulerAngles + Vector3.forward * 90f);
		if (brickTrailEffectPrefab != null && !string.IsNullOrEmpty(brickTrailEffectPrefab.assetGUID))
		{
			trail = (GameObject)brickTrailEffectPrefab.GetInstance();
			trail.transform.parent = base.transform;
			trail.transform.localPosition = Vector3.zero;
			trail.transform.localRotation = Quaternion.Euler(Vector3.zero);
		}
		AudioManager.PlayAudioWithKey("rocket_activation");
	}
}
