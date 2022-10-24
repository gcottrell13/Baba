## moving to a new map
- the conditions for triggering this are elsewhere

Given the current map, the new map id and the direction of motion:
. the new map display should already exist; if not, create it
. remove all old map displays that are not adjacent to the new map display
. offset the map container graphics coordinate
. offset the current map display graphics coordinate
. place the new map display graphics at (0, 0)
. animate the map container graphics coordinates to (0, 0)
. take note of which "x is you" objects in current map display are outside the current map display => ALL_YOU
. delete all "x is you" objects from current map display
. for each item in ALL_YOU, add them to the new map display only if they would fall inside it

## button event

every YOU item moves in the given direction (unless the button is SPACE, then they do not move)
every MOVE item moves in their facing direction

## movement algorithm

every item has a corresponding list of coordinates it should move to in this movement frame.
effects from items can append coords to this list.

first remove player controls.

for each item that wants to move, resolve movement by:
"check movement" followed by "resolve shift"
create animations to move each moving object along all the coordinates they have accumulated during the resolution process.

after the animations have completed:
if any YOU items are outside the current map display, trigger movement to a new map.

finally, reinstate player controls.

### check movement
check if it can move by using these rules:
. if there is a STOP or SHUT in the destination, do not move.
. if the destination is outside the current map, do not move (unless the object is YOU. These are allowed to be in an invalid position).
. if there is a PUSH in the destination, check if the PUSHable object can move using "check movement".
if the object's movement has not been blocked, then it may move to the destination. its coordinates immediately update, but the graphics do not yet animate.
(append the destination coordiates to the object's coord list)
any PULL objects in the opposite direction of the moving object's movement, adjacent to that moving object, move in the same direction as that moving object.

### resolve shift
for all objects that occupy the same space as a SHIFT object:
. use "check movement" in the direction of the SHIFT object's facing direction, and move it if it can move.

### resolve FEAR
