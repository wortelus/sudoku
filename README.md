# sudoku
Sudoku Experimental Project for Personal Use (formerly a high school homework)
## Description
A C# .NET Framework CLI application for generating and solving sudoku boards.
## Use of this software
If the program won't have any specified command line arguments, it will generate the most basic board:



The program will try to read a file from path in the first argument..
Example:
sudoku.exe C:\Users\wortelus\Desktop\input.txt

input.txt preview
```
xx9x3x7x1
6x3x285xx
x5xxxxxxx
5xxx9xx76
xxx784xxx
73xx6xxx8
xxxxxxx9x
xx684x2x3
2x7x5x1xx
```

Output:
```
 4   2   9     6   3   5     7   8   1

 6   7   3     1   2   8     5   4   9

 8   5   1     4   7   9     6   3   2

 5   1   8     3   9   2     4   7   6

 9   6   2     7   8   4     3   1   5

 7   3   4     5   6   1     9   2   8

 3   4   5     2   1   6     8   9   7

 1   9   6     8   4   7     2   5   3

 2   8   7     9   5   3     1   6   4
For Cycles: 5213        Recursive Calls: 678
Backtrack count: 597
```

## Future
As this was only my high school homework, I plan to add truly random generation of boards from scratch and add more features available via command line arguments

## License
This program is licensed under **MIT License**, by wortelus
More information in the LICENSE file
