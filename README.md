# Sudoku Solver Using Ant Colony
Implemntation of Ant Colony to solve Sudoku boards.

## Introduction
Sudoku is a popular logic-based number-placement puzzle game. The objective of the game is to fill a 9x9 grid with digits so that each column, each row, and each of the nine 3x3 sub-grids that compose the grid contains all of the digits from 1 to 9. The game can be played in various levels of difficulty, from easy to expert. It's a great way to improve problem-solving skills and keep the brain active.

## Description
This Project implements Ant Colony algorithm in order to solve sudoku board.

## Features
* Solving 1x1, 4x4, 9x9, 16x16, 25x25 boards.
* Special Ants Colony implementation in C#.

## Ant Colony
Ant Colony optimization algorithm (ACO) is a probabilistic technique for solving computational problems which can be reduced to finding good paths through graphs. 
In this project I have implemented the Ant Colony algorithm in order to solve sudoku board.

## Constraint Propagation
Constraint propgation is human techniques which implemented as algorithm in order to remove possibilities and get more clues from the board.
Examples for constraints you can find [here](https://www.sudokuwiki.org/)

## Usage
In order to use the project, open the sln file and run the project.
```
Press 1 to take the input from the console
Press 2 to take the input from the file
Press 3 to exit the program
1
enter the string to console
0000000000000000
the initial board is:

+---+---+---+---+
| 0 | 0 | 0 | 0 |
+---+---+---+---+
| 0 | 0 | 0 | 0 |
+---+---+---+---+
| 0 | 0 | 0 | 0 |
+---+---+---+---+
| 0 | 0 | 0 | 0 |
+---+---+---+---+
```
## Refrence
[Click here](https://www.researchgate.net/publication/335954009_Solving_Sudoku_With_Ant_Colony_Optimization) to see the paper which explains the Ant Colony algorithm which I used in this Sudoku solving implementation.