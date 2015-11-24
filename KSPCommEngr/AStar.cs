#region license

/* The MIT License (MIT)

 * Copyright (c) 2015 Skoth

 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KSPCommEngr
{
    // Credit: Adapted from Sebastian Lague's "A* Pathfinding Tutorial" 
    // YouTube Video series: https://www.youtube.com/watch?v=-L-WgKMFuhE&list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW.
    // A* Search Grid Static Class Implementation
    // Algorithm begins by checking individual nodes (N) surrounding the starting 
    // node (S) and assigning values to each one. 'G' represents the distance from 
    // N to S, while 'H' represents a heuristic cost that is the distance from the
    // end node (E) to N.
    public static class AStar
    {
        public static float F
        {
            get
            {
                return G + H;
            }
        }
        public static float G;
        public static float H;
    }

    // Considerations: Will not work as a traditional A* search algorithm, as 
    // the requirements of this project dictate orthogonal graphing, which is 
    // not the intent of the A* Search. In other words, adapting it for
    // orthogonality means intentionally "unoptimizing" the function.

    /*
    Sebastian Lague's Pseudocode:
    OPEN // The set of nodes to be evaluated
    CLOSED // The set of nodes already evaluated

    loop
        current = node in OPEN with the lowest f_cost
        remove current from OPEN
        add current to CLOSED

        if current is the target node // Path has been found
            return
        
        foreach neighbour of the current node
            if neighbour is not traversable or neighbour is in CLOSED
                skip to the next neighbour

            if new path to neighbour is shorter OR neighbour is not in OPEN
                set f_cost of neighbour
                set parent of neighbour to current
                if neighbour is not in OPEN
                    add neighbour to OPEN

    */

    /*
    Wikipedia Article's Pseudocode:

    function A*(start,goal) {
        ClosedSet := {}    	  // The set of nodes already evaluated.
        OpenSet := {start}    // The set of tentative nodes to be evaluated, initially containing the start node
        Came_From := the empty map    // The map of navigated nodes.
 
        g_score := map with default value of Infinity
        g_score[start] := 0    // Cost from start along best known path.
        // Estimated total cost from start to goal through y.
        f_score := map with default value of Infinity
        f_score[start] := g_score[start] + heuristic_cost_estimate(start, goal)
     
        while OpenSet is not empty
            current := the node in OpenSet having the lowest f_score[] value
            if current = goal
                return reconstruct_path(Came_From, goal)
         
            OpenSet.Remove(current)
            ClosedSet.Add(current)
            for each neighbor of current
                if neighbor in ClosedSet	
                    continue		// Ignore the neighbor which is already evaluated.
                tentative_g_score := g_score[current] + dist_between(current,neighbor) // length of this path.
                if neighbor not in OpenSet	// Discover a new node
                    OpenSet.Add(neighbor)
                else if tentative_g_score >= g_score[neighbor] 
                    continue		// This is not a better path.

                // This path is the best until now. Record it!
                Came_From[neighbor] := current
                g_score[neighbor] := tentative_g_score
                f_score[neighbor] := g_score[neighbor] + heuristic_cost_estimate(neighbor, goal)

        return failure
    }
    function reconstruct_path(Came_From,current) {
        total_path := [current]
        while current in Came_From.Keys:
            current := Came_From[current]
            total_path.append(current)
        return total_path
    }

    Remark: the above pseudocode assumes that the heuristic function is monotonic (or consistent, see below), 
    which is a frequent case in many practical problems, such as the Shortest Distance Path in road networks. 
    However, if the assumption is not true, nodes in the closed set may be rediscovered and their cost 
    improved. In other words, the closed set can be omitted (yielding a tree search algorithm) if a solution 
    is guaranteed to exist, or if the algorithm is adapted so that new nodes are added to the open set only 
    if they have a lower f value than at any previous iteration. 
    */
}
