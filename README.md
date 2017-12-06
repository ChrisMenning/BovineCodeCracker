# BovineCodeCracker
A Mastermind / Bulls and Cows game for 1 or 2 players, with an AI opponent
A Mastermind / Bulls and Cows game for 1 or 2 players, with many custom features. Written in C# with .NET.

Custom code lengths of between 3-10 characters.
Custom code depths (how many unique symbols) of between 3-10 characters.
Attempts Allowed 8-18.
Single Player, Player Vs. Computer, or Player Vs. Player
Game Styles

"Classic" - Code Length: 4, Code Depth: 6, Attempts Allowed: 8.
"Deluxe" - Code Length: 5, Code Depth: 10, Attempts Allowed: 12.
"Supreme" - Code Length: 6, Code Depth: 10, Attempts Allowed: 14.
"Glacial" - Code Length: 10, Code Depth: 10, Attempts Allowed: 16.
"Custom" - User-selected parameters.
About BessieBot

The computer opponent solves the codes through a process of elimination. The symbols and their order are randomly selected on the 
first turn. On the 2nd turn, a list of all permutations of code possibilities is created. From there, the list of possibilities is 
filtered a few different ways, including analysis of all previous turns, those turns that contained bulls, and comparison of each of 
the bull-possessing turns in order to identify which characters in which spaces are responsible for the bulls. It is also able to deduce 
from tcomparisons of cow-possessing turns where the bulls definitely are not.
