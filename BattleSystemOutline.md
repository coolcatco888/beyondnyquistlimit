# Introduction #

This is the general plan of the battle system.  Feel free to add details that I may have missed.  This is subject to change and any ideas are open for discussion.


# Details #

There are a few main elements to the battle system which will be discussed in this section, the Battle Arena, Classes, Movement, Weapons and Attacks.


## Battle Arena Terrain ##
  * Flat - Only flat will be described here in detail
  * Tiered
  * Rolling


**Flat**

Terrains can be created using heightmaps however the flat parts will be where the battle will occur.  Invisible walls background obstacles will prevent the player from leaving the battle area.  Battle Area obstacles such as rocks, rivers will be impassable and will have an invisible wall surrounding them.  Projectiles may be able to pass through them.

http://jimwtf.googlecode.com/files/battlesystem.PNG

## Classes ##

Melee:
- Knight

Ranged:
- Mage
- Archer

Healer/Support:
- Cleric


## Movement, Weapons and Attacks ##

Movement will be free range within the battle arena.  Each player will have a lock on an enemy and thus any attack will be fired in the direction of the targeted enemy.

Attack effects will mostly be billboards.  Weapons may be mapped to primary and secondary attack buttons, this is under discussion.