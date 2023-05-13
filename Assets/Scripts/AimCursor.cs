using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class AimCursor : MonoBehaviour {

    public Transform cursorScreen;
    public Canvas inGameCanvas;

    [SerializeField] Transform Head;
    [SerializeField] Transform Hand;
    [SerializeField] float rotationSpeed;

    [SerializeField] float YAngle;
    [SerializeField] float bodyRotationAngle;

    [SerializeField] PlayerControls controls;


    [SerializeField] Vector3 PivotScreenPos;
    [SerializeField] Vector3 toCursor;


    [SerializeField] Vector3 toCursor2;
    public bool Gamepad;
    public Transform cursorCenter;
    public float aimRadius = 200f;

    private Vector3 mouseOld;


    private void Awake() {
        Cursor.visible = false;
        controls = new PlayerControls();
    }

    private void OnEnable() {
        controls.Player.Enable();
    }

    private void OnDisable() {
        controls.Player.Disable();
    }


    private void LateUpdate() {

        

        PivotScreenPos = Camera.main.WorldToScreenPoint(Hand.position);
   
        
        if (Input.mousePosition != mouseOld) { Gamepad = false; }

        if (controls.Player.Look.ReadValue<Vector2>() != Vector2.zero) {
            Gamepad = true;
        }

        //YAngle = cursorScreen.position.x < PivotScreenPos.x ? bodyRotationAngle : -bodyRotationAngle;
        //Head.rotation = Quaternion.Lerp(Head.rotation, Quaternion.Euler(0f, YAngle, 0f), Time.deltaTime * rotationSpeed);
        if (!Gamepad) {
            cursorScreen.position = Input.mousePosition;

            Vector3 PivotScreenPosXYZ = new Vector3(PivotScreenPos.x, PivotScreenPos.y, 0f);

            toCursor = cursorScreen.position - PivotScreenPosXYZ;
            

        
        } else {
            Vector2 gamepadAimInput = controls.Player.Look.ReadValue<Vector2>();
            gamepadAimInput = new Vector2(Mathf.Clamp(gamepadAimInput.x, -1, 1), Mathf.Clamp(gamepadAimInput.y, -1, 1));

            Vector2 PivotScreenPosXY = new Vector2(PivotScreenPos.x, PivotScreenPos.y);
            Vector3 PivotScreenPosXYZ = new Vector3(PivotScreenPos.x, PivotScreenPos.y, 0f);
            cursorScreen.position = PivotScreenPosXY + gamepadAimInput * aimRadius;

            toCursor = cursorScreen.position - PivotScreenPosXYZ;

        }

        Hand.rotation = Quaternion.LookRotation(toCursor2);

        toCursor2 = cursorScreen.position - PivotScreenPos;
        Head.rotation = Quaternion.Lerp(Head.rotation, Quaternion.LookRotation(toCursor2), Time.deltaTime * rotationSpeed);

        mouseOld = Input.mousePosition;
    }


}
