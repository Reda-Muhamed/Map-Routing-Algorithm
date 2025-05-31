# 🚗 Fastest Path Routing System

## Overview

This project implements a high-performance routing system that finds the **fastest path (least time)** between any two points on a map. Unlike standard algorithms that work from node to node, this system supports arbitrary **source and destination coordinates**, allowing users to **walk** to and from nearby intersections within a specified range.

---

## Table of Content
- [Project Structure](#project-structure)
- [Features](#features)
- [Input Format](#input-format)
- [Sample Output](#sample-output)
- [Contributors](#contributors)

---

## Project Structure

-ShortestPathFinder.MapRouting/
│
├── Engine/                     
│   ├── Graph.cs                 
│   ├── OptimalAlgorithm.cs     
│   └── PathBuilder.cs          
│
├── Handler/                    
│   └── HandleWalkingDistance.cs
│
├── Models/                     
│   ├── Edge.cs                 
│   ├── Node.cs                
│   └── Query.cs               
│
├── Utilities/                 
│   ├── HelperFunctions.cs      
│   ├── InputReader.cs          
│   └── TimeHandler.cs          
│
├── TestCases/                 
│  ├── Large Cases/           
│   ├── Medium Cases/           
│   └── Sample Cases/           
├── myOutput/
│   └── results.txt             
│
├── Program.cs                  
└── README.md                  

---

## Features

- Source and destination points can be **any coordinates**, not just graph nodes.
- Walking allowed within a **radius R** (in meters) from source/destination to nearest intersection.
- Road network modeled with **nodes and edges**, each edge has a **length** and **speed**.
- Output includes:
  - Path nodes from source to destination.
  - Total travel time (minutes).
  - Distance walked and distance driven.
  - Execution time (logic only and with I/O).
- Optimized for **large graphs**:
  - Up to **200,000 nodes**
  - Up to **250,000 edges**
  - Up to **1,000 queries**

---

## Input Format

### `map.txt`

<number of intersections> <node_id> <x> <y> ... <number of roads> <from_node> <to_node> <length_km> <speed_kmph> ... ```

###queries.txt
<number of queries>
<source_x> <source_y> <destination_x> <destination_y> <max_walking_distance_in_meters>
...
💡 Algorithms Used
Dijkstra’s algorithm (optimized with a priority queue) for shortest-time path.

Euclidean distance calculation for identifying reachable nodes within walking distance.

Performance monitoring using C#’s Stopwatch for execution time.

---

## Sample Output
pgsql
Copy
Edit
The path nodes: 0, 3, 4, 5, 2
Shortest time = 4.63 mins
Path length = 1.72 km
Walking distance = 0.28 km
Vehicle distance = 1.44 km
Execution time (no I/O) = 1 ms
Execution time (with I/O) = 5 ms
🛠 Technologies
Language: C# (.NET)

Design: Object-Oriented Programming (OOP)

Data Structures: Graphs, Priority Queue, Geometry

---

## Contributors
|                   Name                    |    Github Link    |
| :---------------------------------------: | :--------: |
|      Reda Mohamed Reda Mohamed    | https://github.com/Reda-Muhamed |
| Tasneem Mohamed Ahmed Mohamed | https://github.com/Tasneem357Mohamed |
|      Bsmala Tarek Kamal Khalil Elbagoury     | https://github.com/Bsmalatarek |
|          Yara Ahmed Abdelrahman       |  |
|          Yassmina Mohamed Saleh     | |
