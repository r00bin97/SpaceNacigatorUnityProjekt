﻿// Menu - Startet das Spiel

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameButton : MonoBehaviour {

	public void Click(){
		EventManager.StartGame();
	}
}
