using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour {

	[SerializeField] float walkSpeed = 6f;
	[SerializeField] float runSpeed = 10f;
	[SerializeField] float fallingWalkSpeed = 6f;
	[SerializeField] float jumpHeight = 3f;
	[SerializeField] float gravityMultiplier = 1.5f;

    Vector2 moveInput;
    float jumpInput;
    Vector2 lookInput;
    float gravity;

    [SerializeField] Vector2 velocity;
    [SerializeField] Vector2 acceleration;

    [SerializeField]
    public bool JumpPressed = false;
    public bool FirePressed = false;

    CapsuleCollider touchingCol;
    AimCursor head;

    Rigidbody rbPlayer;
    TouchingDirections touchingDirections;



    // ---------- <Properties ---------- //

    public float CurrentMoveSpeed {
		get {
			if (IsMoving && !touchingDirections.IsOnWall) {
				if (touchingDirections.IsGrounded) {
					if (IsRunning) return runSpeed; 
					else return walkSpeed; 
				} else if (rbPlayer.velocity.y > 0) {
                    //Air moves
                    if (IsRunning) return runSpeed;
                    else return walkSpeed;
                } else {
                    //Falling
                    return fallingWalkSpeed;
                }
			} else { 
				//Idle speed is 0
				return 0f; 
			}
		}
	}

    [SerializeField]
	private bool _isMoving = false;

	public bool IsMoving { 
		get { 
			return _isMoving;
		} 
		private set {
			_isMoving = value;
		} 
	}

	[SerializeField]
	private bool _isRunning = false;

	private bool IsRunning {
		get {
			return _isRunning;
		}
		set { 
			_isRunning = value;
		}
	}

    [SerializeField]
    private bool isJumping = false;
    private bool IsJump = false;

    [SerializeField]
    private bool _isCrouching = false;

    private bool IsCrouching {
        get {
            return _isCrouching;
        }
        set {
            _isCrouching = value;
        }
    }

    [SerializeField]
	private bool _isFacingRight = true;

    public bool IsFacingRight { get {
			return _isFacingRight;
		} private set { 
			if(_isFacingRight != value) {
                touchingCol.transform.localScale = new Vector3(touchingCol.transform.localScale.x * -1, touchingCol.transform.localScale.y, touchingCol.transform.localScale.z);
			}
			_isFacingRight = value;
		} 
	}

    

    // ---------- Properties> ---------- //




    private void Awake() {
		rbPlayer = GetComponent<Rigidbody>();
		touchingDirections = GetComponent<TouchingDirections>();
        touchingCol = GetComponentInChildren<CapsuleCollider>();
        head = GetComponentInChildren<AimCursor>();
    }



	private void FixedUpdate() {

        // Setting velocity to accumulate to reach wanted velocity and applying it to RB:
        Vector2 wantedVelocity = moveInput * CurrentMoveSpeed;
        velocity = Vector2.MoveTowards(velocity, wantedVelocity, acceleration.x * Time.deltaTime);

        rbPlayer.velocity = new Vector3(velocity.x, rbPlayer.velocity.y, rbPlayer.velocity.z);


        // Setting modified gravity and applying it to RB:
        gravity = Physics.gravity.y * gravityMultiplier;

		//Little jump related test for shortening the height
        if (isJumping && rbPlayer.velocity.y > 5 && jumpInput < 0.5f) { gravity *= 9;}

        rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, rbPlayer.velocity.y + gravity * Time.deltaTime, rbPlayer.velocity.z);

        

        // Handling jump and applying it to RB:
        if (jumpInput > 0.5f) {
            if (touchingDirections.IsGrounded) {

                rbPlayer.velocity = new Vector3(rbPlayer.velocity.x, Mathf.Sqrt(jumpHeight * -3f * gravity), rbPlayer.velocity.z);
                isJumping = true;
            }
        } else {
            isJumping = false;
        }

        // Handling crouch
        if (IsCrouching) {
            touchingCol.transform.localScale = new Vector3(touchingCol.transform.localScale.x, 0.5f, touchingCol.transform.localScale.z);
            head.transform.position = Vector3.Lerp(head.transform.position, transform.position + Vector3.up * 0.5f, Time.deltaTime * 10); 
        } else {
            if (!touchingDirections.IsOnCeiling) {
                
                head.transform.position = Vector3.Lerp(head.transform.position, transform.position + Vector3.up * 1.5f, Time.deltaTime * 10);
                touchingCol.transform.localScale = new Vector3(touchingCol.transform.localScale.x, 0.9f, touchingCol.transform.localScale.z);

            }
        }

    }






    // ---------- <Utils> ---------- //

    private void SetFacingDirection(Vector2 moveInput) {
        if (moveInput.x > 0 && !IsFacingRight) {
            IsFacingRight = true;
        } else if (moveInput.x < 0 && IsFacingRight) {
            IsFacingRight = false;
        }
    }

    public void QuitGame() {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }





    // ---------- <Callbacks ---------- //


    public void OnMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

		SetFacingDirection(moveInput);
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookInput = context.ReadValue<Vector2>();

    }

    public void OnRun(InputAction.CallbackContext context) {
		if (context.started) {
			IsRunning = true;
		} else if (context.canceled) {
			IsRunning = false;
		}
	}

    public void OnCrouch(InputAction.CallbackContext context) {
        if (context.started) {
            IsCrouching = true;
        } else if (context.canceled) {
            IsCrouching = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        // TODO Check if is on the ground
        jumpInput = context.ReadValue<float>();
		if (jumpInput > 0.5 && touchingDirections.IsGrounded) {
            IsJump = true;
		} else { IsJump = false; }
    }


	public void OnFire(InputAction.CallbackContext context) {
        if (context.ReadValue<float>() > 0.5f) {
			FirePressed = true;
        } else {
            FirePressed = false;
        }
	}

    // ---------- Callbacks> ---------- //

}
