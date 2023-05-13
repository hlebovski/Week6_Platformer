using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTo : MonoBehaviour {

    public Transform AttachmentPoint;

	private void Update() {
		
		transform.position = AttachmentPoint.position;
		transform.rotation = AttachmentPoint.rotation;

	}

}
