using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headControl : MonoBehaviour {
    public GameObject GameLogic;
    public GameObject Player;

	private GameLogic mainGameLogicController;
	private GameObject currentPlayerHeldPiece;
    private float speed = 5.0f;
	// Use this for initialization
	void Start () {
		mainGameLogicController = GameLogic.GetComponent<GameLogic> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void setCurrentPlayerHeldPiece(GameObject playerPiece) {
		this.currentPlayerHeldPiece = playerPiece;
	}

	void FixedUpdate() {
		if (currentPlayerHeldPiece) {
			Vector3 dir = currentPlayerHeldPiece.transform.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(-dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
        } else {
            Vector3 dir = Player.transform.position - transform.position;
            dir.y = 0; // keep the direction strictly horizontal
            Quaternion rot = Quaternion.LookRotation(-dir);
            // slerp to the desired rotation over time
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
        }
	}
}
