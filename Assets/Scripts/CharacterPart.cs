using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPart : MonoBehaviour {

	public Slider SizeSlider;
	private float _defaultValue;

	private void Start() {
		_defaultValue = transform.localScale.y;
		SizeSlider.value = _defaultValue;
	}

	//public float GetSize() {
	//	return transform.localScale.magnitude;
	//}

	public void SetSize(float value) {
		transform.localScale = Vector3.one * value;
	}

	public void ResetSize() {
		SizeSlider.value = _defaultValue;
	}

}
