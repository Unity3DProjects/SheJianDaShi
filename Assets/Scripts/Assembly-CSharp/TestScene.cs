using UnityEngine;

public class TestScene : MonoBehaviour
{
	[SerializeField]
	private Character[] chrs;

	private void Start()
	{
		Character[] array = chrs;
		foreach (Character character in array)
		{
			foreach (MaskBase currentMask in character.masksManager.CurrentMasks)
			{
				Vector3 vector = character.BodyTransform.position + (Vector3)(Random.insideUnitCircle * 10f);
				character.masksManager.AddCutDecal(currentMask, vector, Random.insideUnitCircle);
			}
		}
	}
}
