Auction Boxing is a work in progress.

Current Features:

  Brawl: players begin with a set of items and duke it out. Good for testing items!


To-Do:

  PRIORITY:
    	- proper gui drawing (the "Round start in x" isn't in the center of the camera)
	- State deconstructers
    	- end-of-match stats
	- extend cane pull range
	- 

  PROGRAMMING:
    	- victory taunts (Ministry of Silly Walks)
    	- pause menu
    	- different levels
    	- auction
    	- loadouts
  
  ART:
    - victory taunts (Ministry of Silly Walks)
    - cane pull animation is them trying to claw back to their position (being torn away off the stage)
    
  BUGFIXES:

    	- People with zero health and not dying: -rounds after first, health subtracting?
    	- people flying really high upwards (getting hit while in the air?, no max upwards velocity?)
    	- Camera is slightly dissorienting when jumping (fast vertical changing?), maybe make camera changes gradual?
	- hitting people when reloading (not confirmed)

  ITEMS:
    - daisy on suit that douses enemies in water, stunning them while they dry off
    - stopwatch whip
    - monocle laser (focussing lenseflare effect)
    - boot charge (combo to run, button to tackle, button again to recover)


///////////////////////////////////////////////////////////////////////////////////////////////////////


Animation Info:

  Widths:
  
    idle = 15
    wWalk = 12;
    wRun = 27;
    wJump = 20;
    wLand = 18;
    wPunch = 37;
    wPunchHit = 34;
    wDodge = 17;
    wBlock = 15;
    wDown = 43;
    wDuck = 16;

    wCaneBonk = 54;
    wCaneHit = 19;
    wCanePull = 76;
    wCaneBalance = 28;

    wRevolverShoot = 56;
    wRevolverHit = 15;
    wRevolverReload = 56;

    wBowlerThrow = 34;
    wBowlerCatch = 37;
    wBowlerReThrow = 34;

  Frame Times:

    0.1f,
    0.1f,
    0.05f,
    0.1f,
    0.1f,
    0.09f,
    0.1f;
    0.05f;
    0.1f;
    0.08f;
    0.08f;

    // item frame times
    0.1f;
    0.1f;
    0.1f;
    0.1f;

    0.1f;
    0.05f;
    0.1f;

    0.1f;
    0.1f;
    0.1f;
