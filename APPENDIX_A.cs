// By Alexandros Panagiotakopoulos 
// MTP 333
// Hellenic Meditteranean University
// MSc in Informatics Engineering
// Academic year 2024-2025
// For Computational Intelligence course [TP287]

using System;
using System.Collections.Generic;
using System.Linq;



// Main class
class Program
{
    static void Main(string[] args)
    {
        // Create all the directions (nodes) and turn them into a dictionary
        var inputs = new List<(string, Dictionary<string, double>)>
        {
            ("Fruit Seller's Home", new Dictionary<string, double> { { "OAMT Office", 207.97 }, { "Carpentry Shop", 348.69 } }),
            ("OAMT Office", new Dictionary<string, double> { { "Taxi Rank", 199.58 }, { "Cola Market", 1039.25 }, { "Fruit Seller's Home", 207.97 }, { "Carpentry Shop" , 344.88 } }),
            ("Carpentry Shop", new Dictionary<string, double> { { "OAMT Office", 344.88 }, { "Fruit Seller's Home", 348.69 }, { "NIB", 306.77 }, { "Post Office", 264.76 } }),
            ("Taxi Rank", new Dictionary<string, double> { { "OAMT Office", 199.58 }, { "Ayia Street", 323.94 }, { "Old Market", 264.88 } }),
            ("Cola Market", new Dictionary<string, double> { { "OAMT Office", 1039.25 }, { "New Market", 691.34 }, { "Ayia Street", 585.76 }, { "Abattoir", 395.15 } }),
            ("Ayia Street", new Dictionary<string, double> { { "Taxi Rank", 323.94 }, { "Old Market", 210.14 }, { "Cola Market", 585.76 }, { "Goil Filling Station", 194.57 }, { "Kotokoli line", 326.58 } }),
            ("NIB", new Dictionary<string, double> { { "Carpentry Shop", 306.77 }, { "Old Market", 148.02 }, { "GCB", 93.21 } }),
            ("GCB", new Dictionary<string, double> { { "NIB", 93.21 }, { "Post Office", 229.63 }, { "Straw market", 189.21} }),
            ("Straw market", new Dictionary<string, double> { { "Jubilee park", 190.15 }, { "Kotokoli line", 232.21 }, { "GCB", 189.21} }),
            ("Kotokoli line", new Dictionary<string, double> { { "Transport Station", 96.92 }, { "Straw market", 232.21 }, { "Ayia Street", 326.58}, {"Old Market",275.33 } }),
            ("Old Market", new Dictionary<string, double> { { "NIB", 148.02 }, { "Kotokoli line", 275.33 }, { "Ayia Street", 210.14}, {"Taxi Rank",264.88 } }),
            ("Jubilee park", new Dictionary<string, double> { { "Post Office", 428.28 }, { "New Market", 548.58 }, { "Straw market", 190.15 },{"Transport Station", 352.52 }  }),
            ("New Market", new Dictionary<string, double> { { "Jubilee park", 548.58 }, { "Transport Station", 310.10 }, { "Cola Market", 691.34}, {"Abattoir", 366.9 } }),
            ("Transport Station", new Dictionary<string, double> { { "Jubilee park", 352.52 }, { "Kotokoli line", 96.92 }, { "Goil Filling Station", 250.00},{ "New Market", 310.10} }),
            ("Goil Filling Station", new Dictionary<string, double> { { "Abattoir", 140.33 }, { "Transport Station", 250.00  }, { "Ayia Street", 194.57} }),
            ("Post Office", new Dictionary<string, double> { { "GCB", 229.63 }, { "Carpentry Shop", 264.76 }, { "Jubilee park", 428.28} }),
            ("Abattoir", new Dictionary<string, double> { { "Cola Market", 395.15 }, { "New Market", 366.90 }, { "Goil Filling Station", 140.33} }),
        };

        var destinations = inputs.Select(i => i.Item1).ToArray(); // Get all the destinations
        var distances = new Dictionary<(string, string), double>(); // Create a dictionary to store the distances between the destinations

        foreach (var input in inputs) // Fill the distances dictionary with the distances from the input
        {
            foreach (var path in input.Item2) // For each path from the input   
            {
                distances[(input.Item1, path.Key)] = path.Value; // Add the distance to the dictionary
                //Console.WriteLine($"Distance from {input.Item1} to {path.Key}: {path.Value}"); // Print the distance
            }
        }

        // Check for mismatches in the dictionary
        // If there are routes missing or missmatches in the distances, print a message
        // The program won't function if there are missmatches!
        foreach (var distance in distances)
        {
            var reverseKey = (distance.Key.Item2, distance.Key.Item1); // Get the reverse path
            if (distances.ContainsKey(reverseKey)) // If the reverse path exists in the dictionary
            {
                if (distances[reverseKey] != distance.Value) // If the reverse path has a different distance
                {
                    Console.WriteLine($"Mismatch found: Distance from {distance.Key.Item1} to {distance.Key.Item2} is {distance.Value}, but distance from {distance.Key.Item2} to {distance.Key.Item1} is {distances[reverseKey]}");
                } // Print a message (mismatch found)
            }
            else
            {
                Console.WriteLine($"Missing reverse path: Distance from {distance.Key.Item1} to {distance.Key.Item2} is {distance.Value}, but no path from {distance.Key.Item2} to {distance.Key.Item1}");
            } // Print a message (missing reverse path)
        }

        var bestRoute = FindBestRoute(destinations, distances); // Find the best route

        if (bestRoute.route.Count > 0) // If a valid route was found
        {
            Console.WriteLine($"Best Route: " + string.Join(" -> ", bestRoute.route)); // Print the best route
            Console.WriteLine($"Distance: " + bestRoute.distance); // Print the distance
        }
        else // If no valid route was found
        {
            Console.WriteLine("No valid route found."); // Print a message (no valid route was found)
        }

    }

    static (List<string> route, double distance) FindBestRoute(string[] destinations, Dictionary<(string, string), double> distances) // Find the best route
    {
        var startNode = "Fruit Seller's Home"; // Set the start node (Fruit Seller's Home)
        var bestRoute = new List<string>(); // Create a list to store the best route
        double bestDistance = double.MaxValue; // Set the best distance to the maximum possible value

        void DFS(string currentNode, List<string> currentRoute, double currentDistance, HashSet<string> visited) // Depth-first search algorithm
        {
            if (currentDistance >= bestDistance) return; // Branch and Bound: prune paths that can't improve the best distance

            if (visited.Count == destinations.Length) // If all the destinations have been visited
            {
                if (distances.ContainsKey((currentNode, startNode))) // If there is a path from the current node to the start node
                {
                    currentDistance += distances[(currentNode, startNode)]; // Add the distance to the start node
                    currentRoute.Add(startNode); // Add the start node to the route

                    if (currentDistance < bestDistance) //  If the current distance is less than the best distance
                    {
                        bestDistance = currentDistance; // Set the best distance to the current distance
                        bestRoute = new List<string>(currentRoute); // Set the best route to the current route
                    }

                    currentRoute.RemoveAt(currentRoute.Count - 1); // Remove the start node from the route  
                }
                return;
            }

            foreach (var neighbor in distances.Keys.Where(k => k.Item1 == currentNode && !visited.Contains(k.Item2))) // For each neighbor of the current node
            {
                visited.Add(neighbor.Item2); // Add the neighbor to the visited set
                currentRoute.Add(neighbor.Item2); // Add the neighbor to the current route
                DFS(neighbor.Item2, currentRoute, currentDistance + distances[neighbor], visited); // Recursively call the DFS function
                visited.Remove(neighbor.Item2); // Remove the neighbor from the visited set
                currentRoute.RemoveAt(currentRoute.Count - 1); //  Remove the neighbor from the current route
            }
        }

        DFS(startNode, new List<string> { startNode }, 0, new HashSet<string> { startNode }); // Call the DFS function

        return (bestRoute, bestDistance); // Return the best route and the best distance
    }
}
