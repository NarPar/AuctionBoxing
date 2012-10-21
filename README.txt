Auction Boxing is currently a work in progress.
Current features:
-Brawl: players begin with a set of items and simply duke it out. Good for testing items!

To-Do:

	PRIORITY:
		- level engine is messed (platforms)
		- Hat collisions
		- hat rethrowing
		- cape


	Programming:
		-BitMasks for accurate pixel collision
		-Player Colors
		-projectile instances
		-Bowler Hat
		-jump punch
		-Victory taunts (Ministry of silly walks)
		-Kick (Ministry of silly walks)
		//-punch combo so can't dodge second punch? (Maybe)
		-Different levels
		-Auction
		-loadouts
	
	Art:
		-Bit mask assets for all state animations
		-Victory taunts (Ministry of silly walks)
		-Kick (Ministry of silly walks)
		--cane pull animation is them trying to claw back to their position (Being torn away off the stage)
bug:
-jump and punch not getting upper level set. (something to do with isFalling?)
-jump fall direction backwards
-hitting people when reloading (Not confirmed)

Items:
-daisy on suit that douses enemies in water, stunning them while they dry off
-Stop watch whip
-monacle laser (focussing lenseflare effect)
-boot charge (combo to run, button to tackle, button again to recover)

animation info:

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

frame times:

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
