using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
    internal class Day08 : DayBase
    {
        List<Node> _nodeList = new List<Node>();
        List<string> _directions = new();
        public Day08() : base(8)
        {
            // Additional constructor logic for the derived class, if needed
        }
        public void Initialize()
        {
            foreach (string text in _textInput)
            {
                string[] parts = text.Split('=');
                if (parts.Length == 2)
                {
                    string name = parts[0].Trim();
                    string[] nodeParts = parts[1].Trim().Trim('(', ')').Split(',');

                    if (nodeParts.Length == 2)
                    {
                        Node node = new Node
                        {
                            Name = name,
                            NodeNameLeft = nodeParts[0].Trim(),
                            NodeNameRight = nodeParts[1].Trim()
                        };

                        _nodeList.Add(node);
                    }
                }
                else
                {
                    // Convert the string to a list of individual characters
                    List<string> resultList = text.ToCharArray()
                                                        .Select(c => c.ToString())
                                                        .ToList();

                    _directions.AddRange(resultList);
                }
            }
        }
        public override int SolveProblem_1()
        {
            int result = 0;

            Initialize();

            var currentNode = _nodeList.FirstOrDefault(node => node.Name.Equals("AAA"));
            var nextNodeName = string.Empty;
            var ct = 0;
            while (currentNode is not null && !currentNode.Name.Equals("ZZZ"))
            {
                for (int i = 0; i < _directions.Count(); i++)
                {
                    if (_directions[i].Equals("R"))
                    {
                        nextNodeName = currentNode.NodeNameRight;
                    }
                    else
                    {
                        nextNodeName = currentNode.NodeNameLeft;
                    }
                    ct++;
                    currentNode = _nodeList.FirstOrDefault(node => node.Name.Equals(nextNodeName));
                    Console.WriteLine($"Node: {currentNode.Name}, Iteration: {i}");
                }
            }

            result = ct;
            return result;
        }

        public override int SolveProblem_2()
        {
            int result = 0;
            Initialize();
            //var currentNode = _nodeList.FirstOrDefault(node => node.Name.EndsWith("A"));
            List<Node> currentNodes = _nodeList.Where(node => node.Name.EndsWith("A")).ToList();
            List<string> nextNodeNames = new();
            var ct = 0;
            var exit = false;
            while (!exit)
            {
                for (int i = 0; i < _directions.Count(); i++)
                {
                    if (_directions[i].Equals("R"))
                    {
                        nextNodeNames = currentNodes.Select(node => node.NodeNameRight).ToList();
                    }
                    else if(_directions[i].Equals("L"))
                    {
                        nextNodeNames = currentNodes.Select(node => node.NodeNameLeft).ToList();
                    }
                    else
                    {
                        continue;
                    }

                    ct++;
                    currentNodes = _nodeList.Where(node => nextNodeNames.Contains(node.Name)).ToList();

                    var nodesWithZ = currentNodes.Where(node => node.Name.EndsWith("Z")).ToList();
                    foreach (var item in nodesWithZ)
                    {
                        item.Steps = ct;
                    }

                    int countNodesWithStepGreaterThanZero = _nodeList.Count(node => node.Steps > 0);

                    if (countNodesWithStepGreaterThanZero == currentNodes.Count)
                    {
                        exit = true;
                    }

                    if (nextNodeNames.Count(node => node.EndsWith("Z")) > 0)
                    {
                        Console.WriteLine($"Node Name With Z: {nextNodeNames}");
                    }

                    Console.WriteLine($"Node Count: {currentNodes.Count}, Iteration: {i}");
                }
            }
            return ct;
        }
    }

    internal class Node
    {
        public string Name { get; set; }
        public string NodeNameLeft { get; set; }
        public string NodeNameRight { get; set; }
        public int Steps { get; set; } = -1;
    }
}
