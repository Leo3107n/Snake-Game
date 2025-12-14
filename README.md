Snake Game - Code Explanation and Guide
Game States:
                         The game uses an enum GameState to manage different phases: Start, Running, Paused, and GameOver. This makes it easy to control what the game should do at any given time, such as waiting for the player to start, pausing gameplay, or showing the game over message.
Snake Movement:
                         The snake’s direction is tracked using the Direction enum with values Up, Down, Left, and Right. The snake moves continuously in the current direction, and the player can change it using the arrow keys. The ProcessCmdKey method ensures the snake cannot reverse directly into itself.
User Interface:
                        The game uses a WinForms panel (gamePanel) to draw the snake and food. Buttons allow the player to start or restart the game, pause/resume, and change the snake’s color. Labels display the score, high score, level, and status messages. This provides a simple but effective interface for interacting with the game.
Starting and Restarting:
                      The StartGame method initializes the snake with three segments, resets the score and level, and spawns food. It also sets the game state to Running and starts the timer, which drives the game loop.
Pausing and Resuming:
                      The TogglePause method allows the player to pause the game either by pressing the Pause button or the Space key. When paused, the timer stops, freezing the snake’s movement. Resuming restarts the timer and continues gameplay.
Game Loop:
                      The UpdateGame method is called on every timer tick. It moves the snake forward, checks for collisions, and handles food consumption. If the snake eats food, the score increases and the snake grows longer. If it collides with itself or the wall, the game ends.
Collision Detection:
                     Collision detection ensures the snake cannot move outside the grid or into itself. If such a collision occurs, the GameOver method is triggered, stopping the timer and updating the high score if necessary.
Food Spawning:
                     The SpawnFood method places food randomly on the grid, ensuring it does not overlap with the snake’s body. Food is drawn as a red circle, and eating it increases the score by ten points.
Level and Speed:
                     The UpdateLevelAndSpeed method increases the level every fifty points. With each level, the snake’s speed increases by reducing the timer interval, making the game progressively harder. The speed will never go below the defined minimum.
Drawing the Game:
                     The DrawGame method handles rendering. The snake is drawn as green rectangles (or the chosen color), and food is drawn as a red circle. The graphics are refreshed each time the panel is invalidated, ensuring smooth animation.
Snake Color Customization:
                    The ChangeSnakeColor method opens a color picker dialog, allowing the player to choose a new snake color. Once selected, the snake is redrawn with the chosen color.

How to Use This Code

1.	Open in Visual Studio: Create a new Windows Forms App project and replace the default Form1.cs with this code.
2.	Build and Run: Compile the project and run the application. A window will open with the game panel and controls.
3.	Play the Game: 
•	Click Start / Restart to begin.
•	Use the arrow keys to control the snake.
•	Press Space or click Pause to pause/resume.
•	Eat food to increase your score and level.
•	Avoid hitting walls or yourself to keep playing.
•	Change the snake’s color anytime with the Change Color button.
4.	Game Over: When the snake collides, the game ends and your score is displayed. You can restart by pressing the Start button again.
