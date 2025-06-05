using System.Collections.Generic;
using UnityEngine;

public class Masks : MonoBehaviour
{
	[Header("Body")]
	[SerializeField]
	private string nameSlotBody = "body";

	[SerializeField]
	private MaskBase prefabMaskBody;

	[SerializeField]
	private string[] excludesNamesForCutBody = new string[5] { "arm_l", "arm_r", "head", "thigh_l", "thigh_r" };

	[SerializeField]
	private string[] excludesNamesForHideBody = new string[5] { "arm_l", "arm_r", "head", "thigh_l", "thigh_r" };

	[Header("Head")]
	[SerializeField]
	private string nameSlotHead = "head";

	[SerializeField]
	private MaskBase prefabMaskHead;

	[SerializeField]
	private string[] excludesNamesForCutHead = new string[4] { "brow_l", "brow_r", "eyes", "mouth" };

	[SerializeField]
	private string[] excludesNamesForHideHead = new string[0];

	[Header("Arm left")]
	[SerializeField]
	private string nameSlotArmL = "arm_l";

	[SerializeField]
	private MaskBase prefabMaskArmL;

	[SerializeField]
	private string[] excludesNamesForCutArmL = new string[0];

	[SerializeField]
	private string[] excludesNamesForHideArmL = new string[0];

	[Header("Arm rigth")]
	[SerializeField]
	private string nameSlotArmR = "arm_r";

	[SerializeField]
	private MaskBase prefabMaskArmR;

	[SerializeField]
	private string[] excludesNamesForCutArmR = new string[0];

	[SerializeField]
	private string[] excludesNamesForHideArmR = new string[0];

	[Header("Thigh left")]
	[SerializeField]
	private string nameSlotThighL = "thigh_l";

	[SerializeField]
	private MaskBase prefabMaskThighL;

	[SerializeField]
	private string[] excludesNamesForCutThighL = new string[0];

	[SerializeField]
	private string[] excludesNamesForHideThighL = new string[0];

	[Header("Thigh rigth")]
	[SerializeField]
	private string nameSlotThighR = "thigh_r";

	[SerializeField]
	private MaskBase prefabMaskThighR;

	[SerializeField]
	private string[] excludesNamesForCutThighR = new string[0];

	[SerializeField]
	private string[] excludesNamesForHideThighR = new string[0];

	public MaskManager.MaskSettings GetSettingsBody()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotBody;
		result.prefabMask = prefabMaskBody;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutBody);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideBody);
		return result;
	}

	public MaskManager.MaskSettings GetSettingsHead()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotHead;
		result.prefabMask = prefabMaskHead;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutHead);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideHead);
		return result;
	}

	public MaskManager.MaskSettings GetSettingsArmLeft()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotArmL;
		result.prefabMask = prefabMaskArmL;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutArmL);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideArmL);
		return result;
	}

	public MaskManager.MaskSettings GetSettingsArmRigth()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotArmR;
		result.prefabMask = prefabMaskArmR;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutArmR);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideArmR);
		return result;
	}

	public MaskManager.MaskSettings GetSettingsThighLeft()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotThighL;
		result.prefabMask = prefabMaskThighL;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutThighL);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideThighL);
		return result;
	}

	public MaskManager.MaskSettings GetSettingsThighRigth()
	{
		MaskManager.MaskSettings result = default(MaskManager.MaskSettings);
		result.nameSlot = nameSlotThighR;
		result.prefabMask = prefabMaskThighR;
		result.excludesNamesForCut = new List<string>(excludesNamesForCutThighR);
		result.excludesNamesForHide = new List<string>(excludesNamesForHideThighR);
		return result;
	}
}
