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
	private bool isPlayerTurn = false;
	private Rigidbody playerPieceRigidObject;
	private BoxCollider playerPieceBoxCollider;
	private GameLogic mainGameLogicController;

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
			holdingPiece = true;
			mainGameLogicController.setCurrentHeldPlayerPiece (this.gameObject);
		}
    }


	void FixedUpdate() {
		if (holdingPiece == true) {
			Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
			Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);


			if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit)) {
				gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

				playerPieceRigidObject.useGravity = false;
				playerPieceBoxCollider.enabled = false;

				playerPieceRigidObject.AddForce(gravityAttractor.transform.position - this.gameObject.transform.position);

				if (hit.collider.gameObject.tag == "Grid Plate") {
					if (Input.GetMouseButtonDown(0)) {
						holdingPiece = false;
						hit.collider.gameObject.SetActive(false);
						hasBeenPlayed = true;
						playerPieceRigidObject.useGravity = true;
						playerPieceBoxCollider.enabled = true;
						mainGameLogicController.playerMove(hit.collider.gameObject);
					}
				}
			}
		}
	}

    public void playPiece() {
        //If the player has selected a grid area
        //Animate the piece into position
        hasBeenPlayed = true;

        //Tell our GameLogic script to occupy the game board array at the right location with a player piece
    }
}
