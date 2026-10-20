# Homesick

A first-person 3D narrative game about packing up your childhood home. Pick up what your family left behind, relive the memory tied to each piece, and pack them away one by one until the house is empty.

Solo project by Daniela Banda-Hernandez · Unity 6 · C#

## Systems I built

All gameplay scripts are in [`Assets/Scripts`](Assets/Scripts).

| Script | What it does |
| --- | --- |
| `PlayerPickDrop.cs` | Picks up and drops objects with E using a sphere cast from the camera, so small objects don't need pixel-perfect aim. |
| `ObjectGrab.cs` | Makes an object holdable. While held, it's kinematic, attached to the camera's grab point, and ignores the player's collider, which stops it from jittering or shoving the player. Triggers the object's memory on first pickup. |
| `MemoryData.cs` | A ScriptableObject holding one memory: an opening line and the lines that follow. Adding a memory means making a new asset, with no new code. |
| `MemoryPopup.cs` | Plays a memory: opening line over the room, fade to black, lines one at a time (skippable), fade back. Pauses the game and locks camera input while it plays. |
| `PackingCrate.cs` | A trigger zone inside the crate that counts packed objects, updates the "Packed: x/5" counter, and shows the ending once everything is packed. |

## Third-party assets

Player controller: Unity Starter Assets (First Person). Room models and props are from Synty's POLYGON Starter pack and other Unity Asset Store packages. Those packages aren't included in this repository, so the project won't open as-is; the scripts above are the code I wrote.
