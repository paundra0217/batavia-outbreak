<p align="center">
  <img width="50%" alt="prostir" src="https://github.com/paundra0217/paundra0217/blob/main/images/Video%20tanpa%20judul%20(2).gif">
  </br>
</p>

## 🔴About
**Batavia Outbreak [WIP]** is a 3D top down shooter game which tells about a survivor who's exploring a place in which were swarmed by infectious creatures. Your task is to shoot down all zombies while trying to survive and explore places that were infected.

Indonesia during the final stages of World War II, in 1945, when Japanese forces are still occupying the region. The game takes place primarily around the city of Batavia (modern-day Jakarta), A secretive Japanese biological warfare unit, Unit 731, has been conducting experiments on bioweapons in the area. Disaster strikes when a military transport plane carrying a deadly bio-weapon crashes in the dense forests near Batavia. The bioweapon is a virulent pathogen designed to incapacitate enemy forces, but upon exposure to the local population, it has an unforeseen effect: it turns those infected into aggressive, mindless creatures.

## ⬇️Game Page
Itch.io : https://nezux.itch.io/731-batavia-outbreak

## 🕹️Game controls
| Key Binding       | Function          |
| ----------------- | ----------------- |
| W, A, S, D        | Player Movement   |
| Left Click        | Shoot             |
| Right Click       | Strike Alt (for melee)   |
| 1, 2, 3           | Switch Weapons   |
| Scroll Wheel      | Scroll thorugh equipped weapons |

## 💼My Responsilibites
As the sole programmer on this ongoing project, I am currently responsible for the entire development of the game's core systems. This includes scripting the game mechanics, user interfaces, and level design. I am actively working on game mechanics including the player's action, enemies and it's behaviour, weapon system, as well as the level and some UI elements, which will significantly enhance the gameplay experience. Through continuous testing and optimization, I strive to deliver a polished and engaging game.

## 📋 Project Info and Developers
This project is being made using Unity 2023.2.15f1

Developers:
- **Paundra Amirtha Tanto (Programming)**
- Ariq Bimo Nurputro (3D Art)
- Jonathan Oyong (3D Art)

##  📜Scripts
|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `InputHandler.cs` | Handles the input from the player and calls the functions respectively based on each input. |
| `EnemyManager.cs` | Manages and counts the spawned enemy existed in the game. |
| `WeaponHandle.cs` | Handles and Manages equipped weapons in the player. |
| `AudioController.cs` | Manages all kind of audios existed in the project, and handles play, pause, and stop function calls based on events happening in the game. |
| `etc`  | |

## 📂Files description
```
├── batavia-outbreak                    # In this Folder, containing all the Unity project files, to be opened by a Unity Editor
   ├── ...
   ├── Assets                           # In this Folder, it contains all of our code, assets, scenes, etc.
      ├── ...
      ├── Scenes                        # In this folder, there are scenes. You can open these scenes to play the game via Unity.
         ├── TestBench.unity            # As this game is in work in progress, currently, the only playable scene is this file, which is TestBench.unity
      ├── Scripts                       # In this Folder, there are scripts that manage various game mechanics and functionalities.
         ├── Entity                     # In this Folder, there are scripts that handles the behaviour of the entities of the game, including Player and Enemy.
         ├── Object                     # In this Folder, there are scripts that handles the behaviour of the objects, including weapons and projectiles.
         ├── System                     # In this folder, there are scripts that handles the system or backend of the game.
         ├── UI                         # In this folder, there are scripts that handles the UI of the game.
      ├── ....
   ├── ...
      
```
