# sudoku
Sudoku is a CLI program for generating and solving sudoku boards (formerly a high school homework)
## Description
A C# .NET Framework CLI application for generating and solving sudoku boards. The generation and solving steps are done recursively.
## Use of this software
If the program won't have any specified command line arguments, it will generate a random board.
- -f [path]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;to specify path to an unsolved board file, where each row will be on different line and unsolved values will contain non-numeric character
- -o [path]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;to specify output file of a solved/generated board
- -d [number]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;specify difficulty for generating boards (bigger numbers, more empty spaces = harder)
- -h &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;help

Example:
sudoku.exe C:\Users\wortelus\Desktop\input.txt -f input.txt

input.txt preview
```
8xxxxxxxx
xx36xxxxx
x7xx9x2xx
x5xxx7xxx
xxxx457xx
xxx1xxx3x
xx1xxxx68
xx85xxx1x
x9xxxx4xx
```

Output:
```

   8   1   2     7   5   3     6   4   9

   9   4   3     6   8   2     1   7   5

   6   7   5     4   9   1     2   8   3

   1   5   4     2   3   7     8   9   6

   3   6   9     8   4   5     7   2   1

   2   8   7     1   6   9     5   3   4

   5   2   1     9   7   4     3   6   8

   4   3   8     5   2   6     9   1   7

   7   9   6     3   1   8     4   5   2

For Loop Iteration Count: 396220
Total Recursive Calls: 49558
Backtrack Count: 49498
```

## License
This program is licensed under **MIT License**, by wortelus  
More information in the LICENSE file
