This is a prototype of a Match 3 game.

* The initial play area should populate without any immediate matches.

* Players can drag an item to a new position that will result in a match of 3 or more items.
* Players should NOT be able to move items to positions with no matches.
* Players cannot attempt to move ANOTHER item while a move is still being processed.

* Items should fade after being matched, and removed once the animation is complete.
* When matched, an appropriate sound effect for the item should play.  Pins should make one sound, balls should make a different sound.  
* New items should drop only after old items are completely removed.
* New items should drop until they arrive at an empty cell, then stop.
* New matches should be detected after all dropping items arrive at destination.
* This process should repeat until no more matches are detected.

* When a player makes a move, the Player Move Num should increment by one.
* When a match is made, detail information for the match should appear in a list at the top of the screen.  Information should include the item type removed, the number removed, and whether or not it is a "bonus" match  (a match caused as a result of a drop.)

* Obstacles appear as sprites that look like brick walls.
* There should never be both an Obstacle and an Item in a cell.
* Obstacles should be removed if they are adjacent to an item that is part of a match.
* Obstacles can be configured to DROP if no item is underneath them.  In this build, GREY obstacles should drop and RED obstacles should not.

* Item "Blocks" should appear as semi-transparent boxes on top of Items.
* A Block should always occur WITH an item, never by itself.
* Blocks are removed when the item they obscure is part of a match.  
* When blocks are removed, ONLY the block should be removed and NOT the item.
* Items which are blocked should never drop.
* After removing the block, the item should function normally (i.e. match to remove, and drop if nothing beneath).

* The Pause/Play button should work as expected.
* Under the Pause/Play button, there are 3 FPS indicators.  These should always stay near the native frame rate for the device.  

* Tapping the Menu button on the top left should toggle the menu - it should "slide down" or "slide up" into place.
* "Show Cell IDs" should cause cell identifiers to appear - Play Area Cells, Drop Cells (stacked on top of columns), and Move cells (that animate items from origin to destination).
* "Move Speed" should adjust the rate at which items are moved to origin/destination.
* "Drop Speed" should adjust the rate at which items drop from above the column.
* "Remove Speed" should adjust the rate at which items fade out.
* "Num Blocks" should adjust the number of Blocks generated  (requires restart).
* "Num Obstacles" should adjust the number of Obstacles generated (requires restart)
* "Limit Swap Range" when toggled, player can only drag an item one square away from origin.  Otherwise, player can drag an item to any valid destination.
* "Num Item Types" allows a choice of 3-7 numbers of different item types (requires restart).
* "Play Area Size"  allows choice of a 6x6 or 9x9 play area (requires restart).
* The MIDDLE on the menu screen should restore all options to default.
* The RIGHT button on the menu screen should restart the game.
