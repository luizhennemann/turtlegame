# Turtle Game

This document describes how to run the Turtle Game:

## Initialization

### **game-settings**

The settings are configured on the 'game-settings' file. There are some minimum requirements to follow in order to configure the game correctly:

* The Board, StartingPosition, ExitPosition should be informed. Mines are not mandatory, as you can set up an easy game without mines.
* The Board should have at least 2 rows and 2 columns.
* All the tiles (StartingPosition, ExitPosition and Mines) should be inside the Board.
* A direction should be informed on StartingPosition (North, South, East, West).
* ExitPosition should be on the edge of the Board.
* StartingPosition and ExitPosition should not be the on the same tile.
* Mine tiles should not be on the same tile as StartingPosition and ExitPosition.


Please see file example below:

```json
{
	"board": {
		"rows": 4,
		"cols": 5
	},
	"startingPosition": {
		"tile": "0,1",
		"dir": "North"
	},
	"exitPosition": {
		"tile": "4,2"
	},
	"mines": [
		"1,1",
		"3,1",
		"3,3"
	]
}
```

### **moves**

The moves file should contain the sequences of moves the turtle will execute. This file should contain at least one sequence, and all the sequences in the file should contain at least one move. The moves allowed are:

* `Turn` - Turns the turtle to the right
* `Move` - Moves the turtle one tile forward

Please see file example below. The moves should be between quotes and surrounded by brackets.

```json
[
  [ "Move", "Turn", "Move", "Move", "Move", "Move", "Turn", "Move", "Move" ],
  [ "Turn", "Move" ],
  [ "Turn", "Turn", "Move", "Turn", "Turn", "Turn", "Move" ],
  [ "Turn", "Turn", "Turn" ],
  [ "Move", "Move", "Move", "Move", "Move", "Move", "Move" ]
]
```

## Run

To run the game execute the comand below specifing the files to be read (game-settings and moves).

```json
TurtleGame.exe game-settings moves
```

The outcome should be displayed for each sequence executed. The four possible outcomes are:

* Mine hit
* Success
* Fell off the edge
* Still in danger