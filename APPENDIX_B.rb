# By Alexandros Panagiotakopoulos 
# MTP 333
# Hellenic Meditteranean University
# MSc in Informatics Engineering
# Academic year 2024-2025
# For Computational Intelligence course [TP287]

require 'set'
require 'thread'

# Define the destinations and distances using a hash table
destinations = {
  "Fruit Seller's Home" => { "OAMT Office" => 207.97, "Carpentry Shop" => 348.69 },
  "OAMT Office" => { "Taxi Rank" => 199.58, "Cola Market" => 1039.25, "Fruit Seller's Home" => 207.97, "Carpentry Shop" => 344.88 },
  "Carpentry Shop" => { "OAMT Office" => 344.88, "Fruit Seller's Home" => 348.69, "NIB" => 306.77, "Post Office" => 264.76 },
  "Taxi Rank" => { "OAMT Office" => 199.58, "Ayia Street" => 323.94, "Old Market" => 264.88 },
  "Cola Market" => { "OAMT Office" => 1039.25, "New Market" => 691.34, "Ayia Street" => 585.76, "Abattoir" => 395.15 },
  "Ayia Street" => { "Taxi Rank" => 323.94, "Old Market" => 210.14, "Cola Market" => 585.76, "Goil Filling Station" => 194.57, "Kotokoli line" => 326.58 },
  "NIB" => { "Carpentry Shop" => 306.77, "Old Market" => 148.02, "GCB" => 93.21 },
  "GCB" => { "NIB" => 93.21, "Post Office" => 229.63, "Straw market" => 189.21 },
  "Straw market" => { "Jubilee park" => 190.15, "Kotokoli line" => 232.21, "GCB" => 189.21 },
  "Kotokoli line" => { "Transport Station" => 96.92, "Straw market" => 232.21, "Ayia Street" => 326.58, "Old Market" => 275.33 },
  "Old Market" => { "NIB" => 148.02, "Kotokoli line" => 275.33, "Ayia Street" => 210.14, "Taxi Rank" => 264.88 },
  "Jubilee park" => { "Post Office" => 428.28, "New Market" => 548.58, "Straw market" => 190.15, "Transport Station" => 352.52 },
  "New Market" => { "Jubilee park" => 548.58, "Transport Station" => 310.10, "Cola Market" => 691.34, "Abattoir" => 366.9 },
  "Transport Station" => { "Jubilee park" => 352.52, "Kotokoli line" => 96.92, "Goil Filling Station" => 250.00, "New Market" => 310.10 },
  "Goil Filling Station" => { "Abattoir" => 140.33, "Transport Station" => 250.00, "Ayia Street" => 194.57 },
  "Post Office" => { "GCB" => 229.63, "Carpentry Shop" => 264.76, "Jubilee park" => 428.28 },
  "Abattoir" => { "Cola Market" => 395.15, "New Market" => 366.90, "Goil Filling Station" => 140.33 }
}

# Function to perform BFS and find the shortest path
def bfs(start, destinations) # Breadth First Search
  queue = Queue.new # FIFO
  queue << [start, [start], 0] # [current_node, path, current_distance]
  shortest_path = [] # Initialize the shortest path
  shortest_distance = Float::INFINITY # Initialize the shortest distance INFINITY (largest possible number)

  until queue.empty? # Until the queue is empty
    current_node, path, current_distance = queue.pop # Get the current node, path and current distance

    if path.size == destinations.size # If the path size is equal to the number of destinations
      if destinations[current_node][start] # If the current node has a path back to the start
        total_distance = current_distance + destinations[current_node][start] # Calculate the total distance
        if total_distance < shortest_distance # If the total distance is less than the shortest distance
          shortest_distance = total_distance # Update the shortest distance
          shortest_path = path + [start] # Update the shortest path
        end
      end
      next
    end

    destinations[current_node].each do |neighbor, distance| # For each neighbor of the current node
      next if path.include?(neighbor) # Skip if the neighbor is already in the path
      queue << [neighbor, path + [neighbor], current_distance + distance] # Add the neighbor to the queue
    end
  end

  { path: shortest_path, distance: shortest_distance } # Return the shortest path and distance
end

# Function to find the shortest path using BFS
def find_shortest_path(destinations) # Find the shortest path using BFS
  start = destinations.keys.first # Get the first destination as the start
  bfs(start, destinations) # Perform BFS and return the result
end

# Function to check for mismatches between destinations
def check_mismatches(destinations) # Check for mismatches between destinations
  mismatches = [] # Initialize the mismatches array

  destinations.each do |location, neighbors| # For each location and its neighbors
    neighbors.each do |neighbor, distance| # For each neighbor and distance
      unless destinations.key?(neighbor) # If the neighbor is not a valid location
        mismatches << "Location '#{location}' has a missing neighbor '#{neighbor}'"
        next
      end

      reverse_distance = destinations[neighbor][location]
      if reverse_distance.nil?
        mismatches << "Missing reverse path: from '#{neighbor}' to '#{location}'"
      elsif reverse_distance != distance
        mismatches << "Asymmetric mismatch: '#{location}'->'#{neighbor}' is #{distance}, but '#{neighbor}'->'#{location}' is #{reverse_distance}"
      end
    end
  end

  if mismatches.empty? # If no mismatches were found
    puts "No mismatches found." # Print a message
  else
    puts "Mismatches found:" # Print a message
    mismatches.uniq.each { |m| puts m } # Print unique mismatches
  end
end

# Check for mismatches in the destinations
check_mismatches(destinations) # Check for mismatches

# Find and print the shortest path
result = find_shortest_path(destinations) # Find the shortest path
puts "Shortest path: #{result[:path].join(' -> ')}" # Print the shortest path
puts "Total distance: #{result[:distance]}" # Print the total distance