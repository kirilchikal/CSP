# Constraint Satisfaction Problem
Implementing CSP with Forward Checking constraint propagation on the example of solving Binary and Futoshiki puzzle

## Features
- Problem is defined by set of **[Variables](https://github.com/kirilchikal/CSP/blob/master/CSP/Variable.cs)** with their possible values 
- Each **Variable** contains their own number and can be set to a value of *int* type
- **[Model](https://github.com/kirilchikal/CSP/blob/master/CSP/Model.cs)** class is a problem representation (like Binary puzzle, Futoshiki etc.) that is defined as a set of Variables, **[Domain](https://github.com/kirilchikal/CSP/blob/master/CSP/Domain.cs)** and **[Constraints](https://github.com/kirilchikal/CSP/blob/master/CSP/Constraint.cs)** based on the solving problem
- There is the implementation of both search algoritms: ***[Backtracking](https://github.com/kirilchikal/CSP/blob/382167e751a15a6668c1b20632f347e1c5e98af9/CSP/CSP.cs#L115)*** and ***[Forward chechikg](https://github.com/kirilchikal/CSP/blob/382167e751a15a6668c1b20632f347e1c5e98af9/CSP/CSP.cs#L46)***. [See below]() the comparison of using this methods

## Problems
Below there are two problems for an example of how to use CSP algorytm
1. ### [Binary puzzle](https://github.com/kirilchikal/CSP/blob/master/CSP/BinaryPuzzle.cs)
  The objective is to fill the grid with 1s and 0s, where there is an equal number of 1s and 0s in each row and column and no more than two of either number adjacent to each other. Additionally, there can be no identical rows or columns.\
[![image](https://user-images.githubusercontent.com/48454522/176374119-e5f6b8aa-deff-4582-9476-d575a8339780.png)
](https://en.wikipedia.org/wiki/Takuzu#Solving_methods)\
2. ### [Futoshiki puzzle](https://github.com/kirilchikal/CSP/blob/master/CSP/FutoshikiPuzzle.cs) 
  The puzzle is played on a square grid. The objective is to place the numbers such that each row and column contains only one of each digit. Some digits may be given at the start. Inequality constraints are initially specified between some of the squares, such that one must be higher or lower than its neighbor. These constraints must be honored in order to complete the puzzle.
  
## Chosen heuristics
1. Variable-Selection Heuristics
  The CSP was implemented with the [MRV](https://github.com/kirilchikal/CSP/blob/382167e751a15a6668c1b20632f347e1c5e98af9/CSP/CSP.cs#L159) (Minimum remaining values) heuristic, that means choosing the variable with the fewest “legal” remaining values in its domain. Selecting this value helps detect inconsistencies earlier and, consequently, reduce the number of searches.
3. Value-Selection Heuristic 
  After selecting the variable, the order in which the values should be assigned to it must be selected. And to this problem has been implemented a heuristic, which is order values based on how many times each value is shared in a constraint. It means that the least "limiting" value will be selected. Btw after studying the impact of this heuristic was found that using Value-Selection Heuristic is senseles because eventually each value will be selected anyway

## Forward cheking with constraint propagation is better than Backtrack algorithm
Forward checking detects the inconsistency earlier than simple backtracking and thus it allows branches of the search tree that will lead to failure to be pruned earlier than with simple backtracking. This reduces the search tree and (hopefully) the overall amount of work done. Below you can see two graphs that compare both algorithms.
This first graph shows dependence of the used algorithms on the time of finding the first / all solutions (on the example of Binary puzzle). The second graph represents the visited states dependence./
<img src="https://user-images.githubusercontent.com/48454522/176381673-624f9777-82d2-49ce-88aa-4a50cbcc6303.png" width="50">
<img src="https://user-images.githubusercontent.com/48454522/176382594-6a7d53bb-9ac3-48fc-950f-eeaccd666ad1.png" width="50">

