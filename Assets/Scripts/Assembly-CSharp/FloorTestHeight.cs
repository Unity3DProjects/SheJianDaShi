using UnityEngine;

public class FloorTestHeight : MonoBehaviour
{
	[SerializeField]
	private LineRenderer rend;

	private void Update()
	{
		rend.SetPosition(0, base.transform.position);
		rend.SetPosition(1, SingletonMonoBehaviour<LevelsManager>.Instance.CurrentLevel.Location.GetPositionOnFloor(base.transform.position.x));
	}
}
