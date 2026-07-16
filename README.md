# Cats vs. Dogs: Cyber-Pet Tower Defense 🐱🤖🐶

![Made with Unity](https://img.shields.io/badge/Made%20with-Unity%202022.3%20LTS-black.svg?logo=unity)
![Language](https://img.shields.io/badge/Language-C%23-239120.svg?logo=csharp)
![Architecture](https://img.shields.io/badge/Architecture-MVC-blue.svg)

A 2D tower defense game built with the Unity engine, featuring a cyber-pet theme: robotic **cat towers** defend your base (the "main server") against incoming waves of robotic **dog invaders**. Place towers on designated tiles, manage your economy, and survive every wave — if your base health hits zero, the system gets hacked and it's game over.

---

## 📖 Table of Contents

- [Gameplay Overview](#-gameplay-overview)
- [Towers (Defenders)](#-towers-defenders)
- [Enemies (Invaders)](#-enemies-invaders)
- [Combat Mechanics](#-combat-mechanics)
- [Architecture](#-architecture)
- [Project Structure](#-project-structure)
- [Simulation Logging](#-simulation-logging)
- [Getting Started](#-getting-started)
- [Tech Stack](#-tech-stack)

---

## 🎮 Gameplay Overview

- **Defend the base.** Enemies spawn at a start point and follow a fixed waypoint path toward your base. Every enemy that reaches the base deals damage to it. The game is lost when base health reaches **0** (`SYSTEM HACKED!`), and won when every wave has been spawned and all enemies on the field are destroyed (`SYSTEM PROTECTED!`).
- **Manage your economy.** You start with **200 gold** and **100 base health**. Building towers costs gold; every enemy you destroy pays a bounty. Spend wisely — cheap towers early or expensive specialists later.
- **Build on tiles.** Towers can only be placed on dedicated build tiles. Select a tower from the UI, then click a highlighted tile to construct it. Occupied tiles cannot be built on twice.
- **Survive the waves.** Enemies arrive in configurable waves composed of enemy groups (type × count × spawn interval). There is a **10-second grace period** before the first wave and a **5-second break** between waves.

### Game Flow

```
Main Menu ──▶ Game Scene
                 │
                 ▼
     10s preparation countdown
                 │
                 ▼
   ┌── Wave spawns enemy groups ──┐
   │   Towers auto-target & fire  │
   │   Kills award gold           │
   │   Leaks damage the base      │
   └──────── next wave ◀──────────┘
                 │
                 ▼
   Base HP ≤ 0 ────────▶ 💀 DEFEAT
   All waves cleared ──▶ 🏆 VICTORY
```

---

## 🐱 Towers (Defenders)

Three cat towers, each with a distinct tactical role:

| Tower | Role | Damage | Range | Fire Rate | Cost | Special |
|---|---|---:|---:|---:|---:|---|
| **Sniper-Cat v1** | Single-target DPS | 20 | 4.0 | 1.0 | 50 💰 | Deals **50% reduced damage** to armored targets. Fires a laser beam with sound & visual effects. |
| **Bazooka-Cat Heavy** | Area damage (AoE) | 20 | 3.0 | 3.0 | 75 💰 | Splash damage in a **2-unit radius** around the target. **Cannot target or hit flying units.** |
| **Hacker-Cat WiFi** | Support / Debuffer | 15 | 3.5 | 2.0 | 70 💰 | Applies a **50% slow for 3 seconds** on hit (slowed enemies flash cyan). |

---

## 🐶 Enemies (Invaders)

Three dog robots that march along the waypoint path toward your base:

| Enemy | Type | Health | Armor | Speed | Bounty | Base Damage |
|---|---|---:|---:|---:|---:|---:|
| **Robo-Pug v1** | Standard ground unit | 50 | 0 | 5.0 | 10 💰 | 5 |
| **Drone-Chihuahua Air** | Flying unit | 50 | 0 | 7.5 | 15 💰 | 5 |
| **Mecha-Bulldog MK2** | Armored tank | 75 | 100 | 2.5 | 20 💰 | 10 |

- **Drone-Chihuahua** is 50% faster than the standard unit and is **immune to Bazooka-Cat** entirely (both targeting and splash).
- **Mecha-Bulldog** has 50% more health, moves at half speed, and its **100 armor halves all incoming damage** via the armor formula — Sniper-Cat is doubly penalized against it.

---

## ⚔️ Combat Mechanics

### Armor & Damage Formula

All damage is resolved through a shared armor-mitigation formula (`MathHelper.cs`):

```
Net Damage = Raw Damage × (1 − Armor / (Armor + 100))
```

Example: Mecha-Bulldog's 100 armor reduces incoming damage by 50%. Sniper-Cat additionally halves its own raw damage against any armored target, so a Sniper-Cat shot against a Bulldog lands for only **20 × 0.5 × 0.5 = 5 damage**.

### Targeting AI

Towers do **not** simply shoot the nearest enemy. Target priority within range is:

1. **The enemy furthest along the path** (highest waypoint index — i.e., the greatest threat to the base).
2. On a tie, **the enemy closest to the tower** wins.

Bazooka-Cat runs the same logic but skips flying units.

### Slow Debuff

Hacker-Cat's hit reduces the target's speed by 50% for 3 seconds. The debuff does not stack — an already-slowed enemy cannot be slowed again until the effect expires. The enemy's sprite turns **cyan** while slowed.

---

## 🏗️ Architecture

The codebase follows the **Model-View-Controller (MVC)** pattern to keep data, game logic, and presentation cleanly separated:

```
Assets/Scripts/
├── Controllers/          # Game flow, input, and orchestration
│   ├── GameManager.cs    #   Singleton: economy, health, waves, win/lose, logging
│   ├── BuildManager.cs   #   Singleton: currently selected tower prefab
│   ├── TowerTile.cs      #   Buildable tile: hover highlight, click-to-build
│   ├── MainMenuManager.cs#   Scene loading & quit
│   └── MathHelper.cs     #   Static damage/armor math
├── Models/               # Entity data & behavior (inheritance hierarchies)
│   ├── Tower.cs          #   Abstract base: targeting, fire loop, range gizmo
│   │   ├── SniperCat.cs
│   │   ├── BazookaCat.cs
│   │   └── HackerCat.cs
│   └── Enemy.cs          #   Abstract base: waypoint movement, slow system
│       ├── RoboPug.cs
│       ├── DroneChihuahua.cs
│       └── MechaBulldog.cs
└── Views/                # UI & visual feedback only
    ├── CurrencyView.cs   #   Gold/health HUD text & health slider
    └── HealthBarView.cs  #   World-space enemy health bars + slow tint
```

Key OOP features demonstrated:

- **Abstraction & inheritance** — `Tower` and `Enemy` are abstract base classes; every concrete unit overrides behavior (`AtesEt`/Fire, `HasarAl`/TakeDamage, `Ol`/Die, `UsseSaldir`/AttackBase).
- **Polymorphism** — `BazookaCat` overrides the base targeting method to exclude flying units; each enemy defines its own death/attack behavior.
- **Encapsulation** — stats are serialized private/protected fields exposed through read-only properties (`Cost`, `Armor`, `WaypointIndex`, …).
- **Singletons** — `GameManager` and `BuildManager` provide global access points for game state and build selection.
- **Coroutines** — wave spawning, spawn intervals, and timed debuff expiry.

### Wave System

Waves are fully data-driven and configured in the Unity Inspector — no code changes needed to design new levels:

```
Wave (name)
└── Enemy Groups []
    ├── enemy prefab      # which enemy type
    ├── count             # how many
    └── spawn interval    # seconds between spawns
```

---

## 📁 Project Structure

```
TowerDefenseGame/
├── Assets/
│   ├── Prefabs/          # Tower, enemy & laser effect prefabs
│   ├── Scenes/
│   │   ├── MainMenu.unity        # Entry scene (Play / Quit)
│   │   ├── SampleScene.unity     # Main game level
│   │   └── Test_Logic_Scene.unity# Logic testing sandbox
│   ├── Scripts/          # C# source (MVC — see Architecture)
│   ├── Sprites/          # 2D art assets
│   └── TextMesh Pro/     # UI text rendering
├── ProjectSettings/      # Unity project configuration
└── README.md
```

---

## 📝 Simulation Logging

Every significant game event is appended to a timestamped log file, **`savunma_gunlugu.txt`** ("defense journal"), written to Unity's `Application.persistentDataPath`:

```
=== SIMULATION LOG ===
[14:32:01] Simulation started. Starting Health: 100, Gold: 200
[14:32:15] Tower 'Sniper-Cat v1' built at (2.0, 1.0, 0.0). Remaining gold: 150.
[14:32:20] Wave 1 started. (Robo-Pug v1: 3, Drone-Chihuahua Air: 1)
[14:32:24] Tower 'Sniper-Cat v1' -> 'Robo-Pug v1' fired. Net Damage: 20
[14:32:31] Base took damage! (-5). Remaining Health: 95
...
```

Logged events include: purchases and spending, wave start/end with enemy composition, every shot with its computed net damage, slow debuff applications, base damage, and the final win/lose result. The log file path is printed to the Unity Console on startup.

---

## 🚀 Getting Started

### Requirements

- **Unity 2022.3.62f3 (LTS)** — or any compatible 2022.3 LTS release
- No third-party packages or external dependencies required

### Run the Game

1. Clone the repository:
   ```bash
   git clone https://github.com/onurakbas/TowerDefenseGame.git
   ```
2. Open the project folder in **Unity Hub** (it will resolve built-in packages automatically).
3. Open `Assets/Scenes/MainMenu.unity` and press **Play**.

### How to Play

1. Click a tower button in the HUD to select a tower type.
2. Click a highlighted build tile (glows green on hover) to place it — gold is deducted automatically.
3. Towers acquire targets and fire on their own; prioritize choke points near the path.
4. Use **Hacker-Cat** slows against fast drones and remember that **Bazooka-Cat can't touch flying units** — keep a Sniper-Cat around for air defense.
5. Survive all waves to win. Use the **Play Again** / **Main Menu** buttons on the game-over panel.

---

## 🛠️ Tech Stack

- **Engine:** Unity 2022.3 LTS (2D)
- **Language:** C# — plain MonoBehaviour scripting, no third-party libraries
- **Built-in packages:** Unity 2D suite (Tilemap, SpriteShape, Pixel Perfect, 2D Animation), TextMeshPro
- **Rendering:** Built-in render pipeline, 2D sprites with `Physics2D` for AoE overlap checks

---

*Note: identifiers and in-game text are partially in Turkish, as the project was developed against a Turkish-language design specification.*
