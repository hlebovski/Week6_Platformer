using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour {

	
	public float groundDistance = 0.6f;
	public float wallDistance = 0.6f;
    public float ceilingDistance = 0.6f;

    CapsuleCollider touchingCol;

	//RaycastHit[] groundHits = new RaycastHit[4];
	//RaycastHit[] wallHits = new RaycastHit[4];




	[SerializeField]
	private bool _isGrounded = true;

	public bool IsGrounded { get { 
			return _isGrounded;
		} private set {
			_isGrounded = value;
		}
	}

    [SerializeField]
    private bool _isOnCeiling = true;

    public bool IsOnCeiling {
        get {
            return _isOnCeiling;
        }
        private set {
            _isOnCeiling = value;
        }
    }

    [SerializeField]
	private bool _isOnWall = true;
	private Vector3 wallCheckDirection => touchingCol.transform.localScale.x > 0 ? Vector3.right : -Vector3.right;

	public bool IsOnWall {
		get {
			return _isOnWall;
		}
		private set {
			_isOnWall = value;
		}
	}




	private void Awake() {
		touchingCol = GetComponentInChildren<CapsuleCollider>();
	}

	private void FixedUpdate() {

        Vector3 TopPoint = transform.position + Vector3.up * touchingCol.height * touchingCol.transform.localScale.y - Vector3.up * touchingCol.radius; 
        Vector3 BottomPoint = transform.position + Vector3.up * touchingCol.radius;

        Ray rayDownRight = new Ray(BottomPoint + Vector3.right / 4, Vector3.down);
        Ray rayDownLeft = new Ray(BottomPoint - Vector3.right / 4, Vector3.down);

        IsGrounded = Physics.Raycast(rayDownRight, groundDistance) || Physics.Raycast(rayDownLeft, groundDistance);
		Debug.DrawRay(BottomPoint + Vector3.right / 4, Vector3.down * groundDistance , Color.red);

        Ray rayUpRight = new Ray(TopPoint + Vector3.right / 4, Vector3.up);
        Ray rayUpLeft = new Ray(TopPoint - Vector3.right / 4, Vector3.up);

        IsOnCeiling = Physics.Raycast(rayUpRight, ceilingDistance) || Physics.Raycast(rayUpLeft, ceilingDistance);
        Debug.DrawRay(TopPoint + Vector3.right / 4, Vector3.up * ceilingDistance, Color.yellow);

        Ray rayTop = new Ray(TopPoint + Vector3.up / 4, wallCheckDirection);
        Ray rayBottom = new Ray(BottomPoint - Vector3.up * 0, wallCheckDirection);

        IsOnWall = Physics.Raycast(rayTop, wallDistance) || Physics.Raycast(rayBottom, wallDistance);
        Debug.DrawRay(TopPoint + Vector3.up / 4, wallCheckDirection * wallDistance, Color.cyan);
        Debug.DrawRay(BottomPoint - Vector3.up * 0, wallCheckDirection * wallDistance, Color.cyan);


        //Vector3 sphereOffset = Vector3.up * (touchingCol.height - 2 * touchingCol.radius) / 2;
        //Debug.DrawRay(transform.position + touchingCol.center, -transform.up * groundDistance, Color.yellow);

        //IsGrounded = Physics.CapsuleCast(
        //	transform.position + touchingCol.center + sphereOffset,
        //	transform.position + touchingCol.center - sphereOffset,
        //	touchingCol.radius,
        //	-transform.up,
        //	out RaycastHit groundHits,
        //	groundDistance);

    }
}
