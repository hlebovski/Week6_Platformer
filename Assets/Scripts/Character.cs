using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Character : MonoBehaviour {

	[SerializeField] CharacterPart Top;
	[SerializeField] CharacterPart Bottom;

	private bool dragging;


	void Update()
    {
		if (Input.GetMouseButton(0)) {

			if (dragging) {
				float degree = Input.GetAxis("Mouse X");
				transform.Rotate(0f, -degree * 1000f * Time.deltaTime, 0f);
			} else {

				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				if (Physics.Raycast(ray, out RaycastHit hit)) {

					if (hit.collider.GetComponent<Character>() is Character character) {

						if (!dragging) dragging = true;
					}
				}
			}

		} else dragging = false;

	}

	public void SetName(string name) {
		//NameText.text = name;
	}


	public void ResetValues() {
		Top.ResetSize();
		Bottom.ResetSize();
		transform.rotation = Quaternion.identity;
		SetName("");
	}

}
