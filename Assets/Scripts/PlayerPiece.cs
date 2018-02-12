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

    // Use this for initialization
    void Start() {
		
	}
	
	// Update is called once per frame
	void Update() {

	}

    public void inPlay() {
        //If this piece has been selected
        //Make it hover above the raycast
		if (this.hasBeenPlayed == false) {
			holdingPiece = true;
		}
    }


	void FixedUpdate() {
		if (GameLogic.GetComponent<GameLogic>().playerTurn == true) {
			if (holdingPiece == true) {
				Vector3 forwardDir = raycastHolder.transform.TransformDirection(Vector3.forward) * 100;
				Debug.DrawRay(raycastHolder.transform.position, forwardDir, Color.green);


				if (Physics.Raycast(raycastHolder.transform.position, (forwardDir), out hit)) {
					gravityAttractor.transform.position = new Vector3(hit.point.x, hit.point.y + hoverHeight, hit.point.z);

					pieceBeingHeld.GetComponent<Rigidbody> ().useGravity = false;
					pieceBeingHeld.GetComponent<BoxCollider> ().enabled = false;

					pieceBeingHeld.GetComponent<Rigidbody>().AddForce(gravityAttractor.transform.position - pieceBeingHeld.transform.position);

					if (hit.collider.gameObject.tag == "Grid Plate") {
						if (Input.GetMouseButtonDown(0)) {
							holdingPiece = false;
							hit.collider.gameObject.SetActive(false);
							pieceBeingHeld.GetComponent<PlayerPiece>().hasBeenPlayed = true;
							pieceBeingHeld.GetComponent<Rigidbody> ().useGravity = true;
							pieceBeingHeld.GetComponent<BoxCollider> ().enabled = true;
							GameLogic.GetComponent<GameLogic>().playerMove(hit.collider.gameObject);
						}
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
