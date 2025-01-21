Jewel Matching Game 
======================

A small game based on Candy crush. swap jewels and use gadgets to eliminate all the jewels on the board 
currently only contains two levels which are solvable! 

Use
======================

All Unity files are available to those who would like to try and edit to add or adjust levels.
This project was created in Unity 2022.3.14f1 so using this or later versions is recommended 

Installation (unity project):

Extract the contents of this folder 
- Open the Unity Hub 
- Select Add > Add project from disk
- navigate to the unzipped folder 
- select the folder and click add in the bottom left
- the project can now be opened through the unity hub 

The builds file also contains a working exe that can be played without using unity

Rules 
======================

Matching Rules:
	- any jewels with 3 or more in a row or column can be eliminated
	- any jewels in a 2x2 square can be eliminated
    These rules can applied together so rows and columns eliminated at the same time 

Gadgets:
	- Bomb : when the jewel a bomb is attached to is destroyed, destroy all jewels around it in a 3x3 grid
	- Color Bomb : when the jewel a color bomb is attached to is destroyed, destroy all jewels of the same color on the board
	- Concretion : when the jewel a Concretion is attached to is destroyed, creates a blocker tile in the same spot as the jewel
	- Fragile : when a jewel a fragile is attached to falls, destroy the jewel