# 🚗 Fastest Path Routing System

## 📌 Overview

This project implements a high-performance routing system that finds the **fastest path (least time)** between any two points on a map. Unlike standard algorithms that work from node to node, this system supports arbitrary **source and destination coordinates**, allowing users to **walk** to and from nearby intersections within a specified range.

---

## 📂 Project Structure


-ShortestPathFinder.MapRouting/
-│
-├── Engine/                      # Core graph logic
-│   ├── Graph.cs                 # Graph data structure (nodes + edges)
-│   ├── OptimalAlgorithm.cs     # Main algorithm for fastest path (Dijkstra or optimized)
-│   └── PathBuilder.cs          # Reconstructs the path and calculates total time/distance
-│
-├── Handler/                    # Logic for walking range filtering
-│   └── HandleWalkingDistance.cs# Finds nodes reachable by walking from source/destination
-│
-├── Models/                     # Basic data structures
-│   ├── Edge.cs                 # Represents a road with length and speed
-│   ├── Node.cs                 # Represents an intersection (ID + coordinates)
-│   └── Query.cs                # Represents a routing query (source, destination, max walk)
-│
-├── Utilities/                  # Helper and support functions
-│   ├── HelperFunctions.cs      # Geometry, distance, rounding helpers
-│   ├── InputReader.cs          # Parses map and queries files
-│   └── TimeHandler.cs          # Tracks execution time (with and without I/O)
-│
-├── TestCases/                  # Testing scenarios
-│  ├── Large Cases/            # For stress and performance tests
-│   ├── Medium Cases/           # Balanced test cases
-│   └── Sample Cases/           # Small/basic examples for debugging
-│
-├── myOutput/
-│   └── results.txt             # Output file for path results and timings
-│
-├── Program.cs                  # Entry point: load input, process queries, output results
-└── README.md                   # Project overview and documentation (not shown in VS)

---

## 📈 Features

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

## 🧾 Input Format

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

📤 Sample Output
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

