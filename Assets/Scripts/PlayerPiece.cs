using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPiece : MonoBehaviour {

	public GameObject GameLogic;
	public GameObject raycastHolder;
	public GameObject player;
	public GameObject gravityAttractor;

	public bool holdingPiece = false;
	public float hoverHeight = 0.3f;
	public bool hasBeenPlayed = false;

	RaycastHit hit;
	public float gravityFactor = 10.0f;

	private Vector3 forceDirection;
	private Rigidbody playerPieceRigidObject;
	private BoxCollider playerPieceBoxCollider;
	private GameLogic mainGameLogicController;
	private Vector3 gridPlatePosition;	
	private GameObject hitColliderGameObject;

	private const float playerPieceDropHeight = .1f;
	private const float playerPieceMoveSpeed = .5f;


    // Use this for initialization
    void Start() {
		playerPieceRigidObject = this.gameObject.GetComponent<Rigidbody> ();
		playerPieceBoxCollider = this.gameObject.GetComponent<BoxCollider> ();
		mainGameLogicController = GameLogic.GetComponent<GameLogic> ();
	}
	
	// Update is called once per frame
	void Update() {

	}

    public void inPlay() {
        //If this piece has been selected
        //Make it hover above the raycast
		if (hasBeenPlayed == false) {
			if (!mainGameLogicController.playerTurn) {
				return;
			}
			bool notifyResult = mainGameLogicController.setCurrentHeldPlayerPiece (this.gameObject);

			if (notifyResult) {
				holdingPiece = true;
			}
		}
    }

	void FixedUpdate() {
		if (holdingPiece == true) {
			Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
			Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);

			if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit)) {
				hitColliderGameObject = hit.collider.gameObject;
				gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

				playerPieceRigidObject.useGravity = false;
				playerPieceBoxCollider.enabled = false;

				playerPieceRigidObject.AddForce(gravityAttractor.transform.position - this.gameObject.transform.position);

				if (hitColliderGameObject.tag == "Grid Plate") {
					if (Input.GetMouseButtonDown(0)) {
						gridPlatePosition = hitColliderGameObject.transform.position;
						hitColliderGameObject.SetActive(false);
						playPiece ();
					}
				}
			}
		}
	}

	public void Move(Vector3 targetPlatePosition) {
		Vector3 transformedVector = new Vector3 (targetPlatePosition.x, targetPlatePosition.y + playerPieceDropHeight, targetPlatePosition.z);
		iTween.MoveTo (this.gameObject,
			iTween.Hash(
				"position", transformedVector,
				"speed", playerPieceMoveSpeed,
				"easetype", iTween.EaseType.easeInOutCubic,
				"onstart", "prepareMove",
				"oncomplete", "finishMove"
			)
		);
	}

	private void prepareMove() {
		playerPieceBoxCollider.enabled = true;
		playerPieceRigidObject.useGravity = false;	
		playerPieceRigidObject.isKinematic = true;
	}

	private void finishMove() {
		playerPieceRigidObject.isKinematic = false;
		playerPieceRigidObject.useGravity = true;	
		gridPlatePosition = Vector3.zero;		
		notifyGameLogicScriptOfNewPlacedPlayerPiece ();
	}

	public void playPiece() {
		//If the player has selected a grid area				
		holdingPiece = false;
		hasBeenPlayed = true;

        //Animate the piece into position 
		Move(gridPlatePosition);	
    }

	private void notifyGameLogicScriptOfNewPlacedPlayerPiece() {
		//Tell our GameLogic script to occupy the game board array at the right location with a player piece
		mainGameLogicController.playerMove(hitColliderGameObject);
		hitColliderGameObject = null;
	}
}
