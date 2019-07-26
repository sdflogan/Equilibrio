using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

	public Text FpsLabel;

	FPSCounter FpsCounter;

	void Awake()
	{
		FpsCounter = GetComponent<FPSCounter>();	
	}

	void Update()
	{
		FpsLabel.text = Mathf.Clamp(FpsCounter.FPS, 0, 99).ToString();
	}
}
