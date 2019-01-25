using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffects : MonoBehaviour {

	[SerializeField] private Camera camera;
	[SerializeField] private PostProcessingProfile ppProfile;
	void Start () {
		ppProfile.chromaticAberration.enabled = false;
	}

	public void decrementChromaticAberration()
	{
		//copy settings from profile into a temporary variable
		ChromaticAberrationModel.Settings chromaticSetting = ppProfile.chromaticAberration.settings;

		//change the intensity in the temporary settings variable
		chromaticSetting.intensity -= 0.05f;

		//set the settings in the actual profile to the temp settings with the changed value
		ppProfile.chromaticAberration.settings = chromaticSetting;

	}

	public void activateChromaticAberration()
	{
		setChromaticAberration(0.8f);
		ppProfile.chromaticAberration.enabled = true;
	}

	public void setChromaticAberration(float set) //just in case
	{
		//copy settings from profile into a temporary variable
		ChromaticAberrationModel.Settings chromaticSetting = ppProfile.chromaticAberration.settings;

		//change the intensity in the temporary settings variable
		chromaticSetting.intensity = set;

		//set the settings in the actual profile to the temp settings with the changed value
		ppProfile.chromaticAberration.settings = chromaticSetting;
	}

	public void deactivateChromaticAberrationWithFade()
	{
		StartCoroutine("coroutine");
	}

	public IEnumerator coroutine() //disables the chromatic aberration with a short fade
	{
		while (ppProfile.chromaticAberration.settings.intensity > 0)
		{
			decrementChromaticAberration();
			yield return null;
		}
		yield return new WaitForSeconds (8f);
	}
}
