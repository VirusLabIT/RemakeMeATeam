using System;
using System.Collections.Generic;
using DelaunatorSharp;


public class Edge
{
    public int U { get; }
    public int V { get; }
    public double Weight { get; }

    public Edge(int u, int v, double weight)
    {
        U = u;
        V = v;
        Weight = weight;
    }
}

public class UnionFind
{
    private int[] parent;
    private int[] rank;

    public UnionFind(int size)
    {
        parent = new int[size];
        rank = new int[size];
        for (int i = 0; i < size; i++)
            parent[i] = i;
    }

    public int Find(int x)
    {
        if (parent[x] != x)
            parent[x] = Find(parent[x]);
        return parent[x];
    }

    public void Union(int x, int y)
    {
        int xRoot = Find(x);
        int yRoot = Find(y);
        if (xRoot == yRoot) return;

        if (rank[xRoot] < rank[yRoot])
            parent[xRoot] = yRoot;
        else
        {
            parent[yRoot] = xRoot;
            if (rank[xRoot] == rank[yRoot])
                rank[xRoot]++;
        }
    }
}

public class MSTBuilder
{
    public static List<Edge> BuildMST(IPoint[] points)
    {
        // Step 1: Generate Delaunay Triangulation
        Delaunator delaunator = new Delaunator(points);

        // Step 2: Extract unique edges from triangulation
        HashSet<Tuple<int, int>> edges = new HashSet<Tuple<int, int>>();
        for (int i = 0; i < delaunator.Triangles.Length; i += 3)
        {
            int i0 = delaunator.Triangles[i];
            int i1 = delaunator.Triangles[i + 1];
            int i2 = delaunator.Triangles[i + 2];
            AddEdge(edges, i0, i1);
            AddEdge(edges, i1, i2);
            AddEdge(edges, i2, i0);
        }

        // Step 3: Calculate weights for each edge
        List<Edge> weightedEdges = new List<Edge>();
        foreach (var edge in edges)
        {
            int u = edge.Item1;
            int v = edge.Item2;
            double dx = points[v].X - points[u].X;
            double dy = points[v].Y - points[u].Y;
            double weight = Math.Sqrt(dx * dx + dy * dy);
            weightedEdges.Add(new Edge(u, v, weight));
        }

        // Step 4: Apply Kruskal's Algorithm
        weightedEdges.Sort((a, b) => a.Weight.CompareTo(b.Weight));
        UnionFind uf = new UnionFind(points.Length);
        List<Edge> mst = new List<Edge>();

        foreach (Edge edge in weightedEdges)
        {
            if (uf.Find(edge.U) != uf.Find(edge.V))
            {
                mst.Add(edge);
                uf.Union(edge.U, edge.V);
                if (mst.Count == points.Length - 1)
                    break;
            }
        }

        return mst;
    }

    private static void AddEdge(HashSet<Tuple<int, int>> edges, int a, int b)
    {
        if (a > b)
            (a, b) = (b, a);
        edges.Add(Tuple.Create(a, b));
    }
}