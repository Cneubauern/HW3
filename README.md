# HW3

#Homework assignment description
This homework assignment consists in merging and extending the following programs:

• TSP Simulated Annealing:
http://www.codeproject.com/Articles/26758/Simulated-Annealing-Solving-the-Travelling-Salesma
• GA in C#:
http://www.codeproject.com/KB/recipes/btl_ga.aspx

You need to solve the TSP problem for the dataset assigned to your team.

You and your team have to report your results and discussions in this wiki for the following tasks:

#1.(10pts) Implement a program that:

reads your data set
displays the cities in an euclidean plane (in 2D)
displays a random route
displays any route specified by the user and computes the total distance for the specified route.
#2.(10pts) Simulated Annealing for TSP (SA-TSP)
Compare results using different parameter values for initial temperature and temperature decrease rate. Find and report the optimal parameter values, as well as, the optimal path and the optimal total distance cost.
 
#3.(10pts) Greedy Strategy for TSP (GS-TSP)
Add to the project SA-TSP your own implementation of a Greedy Strategy (GS) to find an optimal solution for the TSP (example to choose always the shortest distance). Report the optimal path and the optimal total distance cost.

#4.(50pts) Genetic Algorithm for TSP (GA-TSP)
Implement a GA for solving the TSP. 
Implement one of the following especialiazed crossover operators: PMX, CX, OX1, OX2, POS, ER, VR, AP, MPX. In other words the crossover operators found in Larranaga et al.'s paper Genetic algorithms for the Travellin Salesman Problem: A Review of Representations and Operators, sections 4.3.1 to 4.3.11.
Compare your results for different mutation and crossover probabilities, as well as, different percentages of elitism.
Find and report the best parameter configurations, as well as, the optimal path and the optimal total distance cost.
 
#5.(20pts) Compare the best configurations found for the three methods for TSP, report your results and discussion: SA-TSP vs. GS-TSP vs. GA-TSP
