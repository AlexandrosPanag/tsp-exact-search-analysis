# A Comparative Algorithmic Analysis of Exact Search Methods for Urban Route Optimization: Re-evaluating the 17-City Traveling Salesman Problem

Source code (C# and Ruby) and dataset for the comparative algorithmic analysis of the 17-city Bolgatanga TSP. This repository mathematically verifies the optimal Hamiltonian cycle of 4497.55 m, serving as a formal rebuttal to prior published literature.

# Exact Search Methods for Urban Route Optimization (17-City TSP)

![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.20798088.svg)
[WEBSITE](https://zenodo.org/records/20798088)


This repository contains the source code, datasets, and experimental findings for the paper: **"A Comparative Algorithmic Analysis of Exact Search Methods for Urban Route Optimization: Re-evaluating the 17-City Traveling Salesman Problem."**

## Overview
This project provides a rigorous programmatic validation of a specific 17-city Traveling Salesman Problem (TSP) instance based in Bolgatanga, Ghana. It utilizes two distinct exact search algorithms to definitively solve the instance and establish the true mathematical optimum.

* **C# Implementation:** Depth-First Search (DFS) with branch-and-bound pruning mechanics.
* **Ruby Implementation:** Systematic Breadth-First Search (BFS) using level-by-level queue exploration.

By executing a dual-language validation, this project eliminates procedural biases and guarantees the structural integrity of the final route computation.

## Key Findings & Rebuttal
A primary objective of this repository is to serve as a computational rebuttal to previous literature regarding this specific routing instance (Najat & Boah, 2021).

| Study / Method | Reported Distance | Status |
| :--- | :--- | :--- |
| Prior Literature (B&B) | 5080.74 m | Suboptimal |
| **Current Study (DFS & BFS)** | **4497.55 m** | **Optimal (Verified)** |

Both the C# and Ruby implementations completely evaluate the problem space and consistently identify an optimal route of **4497.55 m**, effectively reducing the previously published distance by 583.19 m (approx. 11.5%).

## Repository Structure
* `/csharp-dfs/` - Contains the C# source code for the Depth-First Search implementation.
* `/ruby-bfs/` - Contains the Ruby source code for the Breadth-First Search implementation.
* `/dataset/` - Includes the corrected distance matrix used for the computations.

## How to Run

### C# (DFS)
Ensure you have the .NET SDK installed. Navigate to the C# directory and execute:
```bash
dotnet run
