# Tower Defense Game - Enemy and Turret System

This project is part of a Tower Defense game built in Unity, focusing on the implementation of modular, component-based systems for the `Enemy` and `Turret` classes.

## Overview

The goal of this project is to create a maintainable and scalable Tower Defense game by breaking down the core functionalities of enemies and turrets into separate components. This approach allows for easier management, testing, and expansion of game features.

### Key Features

- **Component-Based Architecture**: Both `Enemy` and `Turret` classes are broken down into individual components that handle specific tasks such as health management, movement, targeting, and damage calculation.
- **Modular Design**: Each component is responsible for a single aspect of the game logic, making it easy to reuse, modify, or extend.
- **Event-Driven Communication**: Components communicate via events or interfaces, ensuring loose coupling and flexibility.

## Project Structure

### Components

- **Health Component**
  - Manages health, armor, and damage for enemies.
  - Handles health-related events like death or health changes.

- **Movement Component**
  - Controls the movement of enemies along a predefined path.
  - Handles speed adjustments and movement patterns.

- **Pathfinding Component**
  - Manages the path that the enemy follows through the level.

- **Death Component**
  - Handles logic for when an enemy dies, such as playing death animations or destroying the game object.

- **Visual Effects Component**
  - Manages visual effects like the wiggle animation and size adjustments for enemies.

- **Game Interaction Component**
  - Handles interactions with other game elements, such as dealing damage to the core.

### ScriptableObjects

- **EnemySO**
  - Stores enemy stats such as health, armor, speed, size multiplier, and damage to the core.
  
- **TurretSO**
  - Stores turret stats such as damage, range, splash damage properties, and upgrade costs.

### Main Classes

- **Enemy**
  - Abstract class representing an enemy in the game. Components manage its health, movement, and interactions.
  
- **Turret**
  - Abstract class representing a turret in the game. Components manage targeting, shooting, and rotation.

## Getting Started

### Prerequisites

- Unity 2021.3 or later
- Basic knowledge of C# and Unity's component-based architecture

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/tower-defense-game.git
    ```

2. Open the project in Unity.

3. Explore the `Assets/Scripts` directory to understand how components are organized and how they interact with the main classes.

### Usage

- **Turret Placement**: Place turrets on the grid to defend the core from incoming enemies.
- **Enemy Pathing**: Enemies follow a predefined path toward the core. The game will deduct core health if an enemy reaches the end of the path.

### Future Improvements

- Implement additional turret types with unique behaviors.
- Add more complex enemy movement patterns and special abilities.
- Introduce a wave system with increasing difficulty.

## Contributing

I am currently not looking for any contributions as of the moment. Feel free to explore though.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- Inspired by the mechanics of popular tower defense games like *Radiant Defense* by Hexage.
- Special thanks to the Unity community for helpful tutorials and code snippets.
