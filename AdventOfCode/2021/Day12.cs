using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class GraphNode
    {
        public string Name { get; set; }
        public List<GraphNode> ConnectedNodes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public class Day12
    {
        Dictionary<string, GraphNode> graphNodes = new Dictionary<string, GraphNode>();
        GraphNode startNode;
        GraphNode endNode;

        GraphNode GetNode(string name)
        {
            if (!graphNodes.ContainsKey(name))
            {
                graphNodes[name] = new GraphNode { Name = name, ConnectedNodes = new List<GraphNode>() };
            }

            return graphNodes[name];
        }

        void ReadInput()
        {
            foreach (string connection in File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day12.txt"))
            {
                string[] nodes = connection.Split('-');

                GraphNode node1 = GetNode(nodes[0]);
                GraphNode node2 = GetNode(nodes[1]);

                node1.ConnectedNodes.Add(node2);
                node2.ConnectedNodes.Add(node1);
            }

            startNode = GetNode("start");
            endNode = GetNode("end");
        }

        List<string> paths = new List<string>();

        long GetPaths(GraphNode node, Dictionary<string, int> visitedNodes, string exceptionNode)
        {
            return GetPaths(node, visitedNodes, exceptionNode, "");
        }

        long GetPaths(GraphNode node, Dictionary<string, int> visitedNodes, string exceptionNode, string pathSoFar)
        {
            if (node == endNode)
            {
                paths.Add(pathSoFar);

                return 1;
            }

            if (!char.IsUpper(node.Name[0]))
            {
                if (visitedNodes.ContainsKey(node.Name))
                    visitedNodes[node.Name]++;
                else
                    visitedNodes[node.Name] = 1;
            }

            long numPaths = 0;

            foreach (GraphNode connectedNode in node.ConnectedNodes)
            {                
                if (!visitedNodes.ContainsKey(connectedNode.Name) || ((connectedNode.Name == exceptionNode) && (visitedNodes[connectedNode.Name] < 2)))
                {
                    numPaths += GetPaths(connectedNode, new Dictionary<string, int>(visitedNodes), exceptionNode, pathSoFar + "-" + connectedNode.Name);
                }
            }

            return numPaths;
        }

        public long Compute()
        {
            ReadInput();

            return GetPaths(GetNode("start"), new Dictionary<string, int>(), null);
        }

        public long Compute2()
        {
            ReadInput();

            List<string> exceptionNodes = new List<string>(from nodeName in graphNodes.Keys where !Char.IsUpper(nodeName[0]) select nodeName);
            exceptionNodes.Remove("start");
            exceptionNodes.Remove("end");

            foreach (string exceptionNode in exceptionNodes)
            {
                GetPaths(GetNode("start"), new Dictionary<string, int>(), exceptionNode);
            }

            paths = new List<string>(paths.Distinct());

            paths.Sort();

            return paths.Count;
        }
    }
}
