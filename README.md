# TowerDefence
A software project for C# .NET programming class

Tower Defence game made in Unity3D. Uses standard principles of Tower Defence genre. You are under attack of enemies, which go in waves along a predefined path. You can buy Towers which will shoot the enemies and kill them. If an enemy reaches the end of the path, you lose one of your lives.

## Towers

Towers can be bought in the shop which is on the right of the window. Each Tower has a range (displayed when you are buying the Tower) and it can only shoot at enemies which are in range. 

There are different types of towers:

### Simple Tower
Basic type of Tower. Shoots at the first Tower in range. Has basic damage and fire rate.

### Multi Tower
This Tower shoots 8 projectiles to all directions whenever there is an enemy in range. The projectiles can hit other enemies in range as well, but dissappear when they fly out of Tower's range. 

### Sniper Tower
This Tower has really big range and damage. But it has low fire rate and shoots only once every few seconds.

### Slowing Tower
Projectiles from this Tower slow down the enemy which got hit. Other enemies behind it can't overtake the slowed enemy, so they can also get slowed. 

### Bomb Tower
Shoots exploding projectiles. The explosion also damages other nearby enemies.

## Upgrades
All Towers can be upgraded. Select the Tower by clicking on it and buttons with available upgrades will appear in the shop. Each type of Tower has it's own upgrades.

## Configuration
The difficulty of the game can be tweaked by modifying `TowerConfiguration.txt` and `WaveConfiguration.txt` in `StreamingAssets` folder. TowerConfiguration contains information about types of Towers, their upgrades and also default amount of Money and Lives. WavesConfiguration is for setting up the waves of enemies.

### Tower Configuration
All values must be separated by comma (`,`). If the first character of line is hash (`#`) with no whitespace in front of it, the line is ignored as comment.

First thing you can set up is the default amount of Money and Lives. This is done by using "d" as the first token on line and then values for Money and Lives in this order.
```
d,250,20
```

Then you can specify paremeters of the 5 types of Towers in game. The Towers are in order __Simple, Multi, Sniper, Slowing, Bomb__. *The types of Towers can't be modified via the configuration file. You can set them only in UnityEditor as properties of shop buttons (ShopItemTower).*

Each Tower is specified on one line with tokens in this order: `name, cost, damage, range, reload time`. Cost and Damage can be only integer values, range and reload time (in seconds) are floats.
```
Simple Tower,100,1,3,1.5
```
Each Tower can have upgrades, which you can configure right below the Tower information. Each type of upgrade must be on a separate line and you can have maximum of 3 upgrades for each Tower.

The line with upgrade configuration starts with token "u". The next token is the upgrade name. And then you specify at least one level of the upgrade in format written below. You can have as many levels of each upgrade as you want, just separate them with commas.

Each level must be in format `cost:type:value`. Value is the new value to which the parameter is set. Type can be:

* "d" for upgrading Damage of Tower
* "r" for upgrading Range of Tower
* "t" for upgrading Reload time of Tower
* "e" for upgrading Explosion size of Tower shooting Exploding projectile (only the 5th tower by default)
* "sa" for upgrading Slow Amount of Tower shooting Slowing projectile (only the 4th tower by default)
  * Slow Amount should be value between 0 and 1. The speed of enemy gets multiplied by this value.
* "sd" for upgrading Slow Duration (in seconds) of Tower shooting Exploding projectile (only the 4th tower by default)
```
u,Reload Time,50:t:1.3,75:t:1.1,100:t:0.8 
```
### Wave Configuration
All values must be separated by comma (`,`). If the first character of line is hash (`#`) with no whitespace in front of it, the line is ignored as comment.

Each wave starts with `wave` and then number of wave.
```wave 1```
Then you can specify enemies spawned in this wave. Each line represents a batch of enemies. All enemies in a batch must be same and you can create one or more of them. The line format is `timeDelay, timeBetween, enemyTypeIndex, enemyHealth, reward, speed, [count]`
* timeDelay is delay in seconds before the game starts spawning enemies of this batch
* timeBetween is delay between spawning enemies in the batch
* enemyTypeIndex is type of enemy, only valid is currently `0`
* enemyHealth is HP every enemy in batch has
* reward is amount of Money player gets for killing each enemy in batch
* speed is the speed of enemies in batch
* [count] is a optional parameter, it specifies number of enemies in this batch. If not present, only one enemy of this type gets spawned.

Here is an example of a wave. First it spawns 3 enemies with 5 HP, then it waits 5 seconds and spawns one enemy with 50 HP and then 3 enemies with 5 HP again, but slower and with more delay in between:
```
wave 1
1, 1, 0, 5, 10, 1, 3
5, 1, 0, 50, 10, 1
1, 2, 0, 5, 10, 0.5, 3
```
Last line of the file can contain keyword `repeat`. This makes the last wave repeating, so the number of waves is infinite. Wave will get modified before each spawn, so it can get stronger over time. To modify the wave, you must specify the repeat line in following format: `repeat, enemy HP mult, enemy HP add, enemy count mult, enemy count add`. The HP and counts of all batches of enemies in the wave will first get multiplied by the defined "mult" constant (float) and than the "add" constant gets added to the number. 

In this example, the first spawn of this wave will have 10 enemies each with 10 HP. The second time it gets spawned, it will have 15 enemies with 10*1.1 + 2 = 13 HP.
```
wave 1
0, 1, 0, 10, 50, 1, 10
repeat,1.1,2,1,5 
```