using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderShowValue : MonoBehaviour {

	public TextMeshProUGUI SliderValue;

	public void SetValueText(float value) {
		SliderValue.text = value.ToString("0.0");
	}

}
