using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ShortestPathFinder.MapRouting.Models;

namespace ShortestPathFinder.MapRouting
{
    public static class MapVisualizer
    {
        public static int VIRTUAL_SOURCE_NODE_ID = -2;
        public static int VIRTUAL_DESTINATION_NODE_ID = -3;

        public static void VisualizeMap(
            List<Node> nodes,
            List<Edge> edges,
            List<int> paths,
            (double SourceX, double SourceY, double DestX, double DestY) queryPoint,
            string outputFile)
        {

            if (nodes == null || edges == null || (double.IsNaN(queryPoint.SourceX) && double.IsNaN(queryPoint.DestX)))
            {
                return;
            }

            int width = 800;
            int height = 800;
            int margin = 50;

            try
            {
                using (Bitmap bm = new Bitmap(width, height))
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.Clear(Color.White);

                    double minX = double.MaxValue, maxX = double.MinValue;
                    double minY = double.MaxValue, maxY = double.MinValue;
                    int vN = 0;
                    foreach (var n in nodes)
                    {
                        if (n == null || double.IsNaN(n.XPoint) || double.IsNaN(n.YPoint) ||
                            double.IsInfinity(n.XPoint) || double.IsInfinity(n.YPoint))
                            continue;
                        minX = Math.Min(minX, n.XPoint);
                        maxX = Math.Max(maxX, n.XPoint);
                        minY = Math.Min(minY, n.YPoint);
                        maxY = Math.Max(maxY, n.YPoint);
                        vN++;
                    }
                    minX = Math.Min(minX, queryPoint.SourceX);
                    maxX = Math.Max(maxX, queryPoint.SourceX);
                    minY = Math.Min(minY, queryPoint.SourceY);
                    maxY = Math.Max(maxY, queryPoint.SourceY);
                    minX = Math.Min(minX, queryPoint.DestX);
                    maxX = Math.Max(maxX, queryPoint.DestX);
                    minY = Math.Min(minY, queryPoint.DestY);
                    maxY = Math.Max(maxY, queryPoint.DestY);

                    if (vN == 0 && queryPoint.SourceX == queryPoint.DestX && queryPoint.SourceY == queryPoint.DestY)
                    {
                        return;
                    }

                    const double targR = 10000.0;
                    double scaleX = (maxX - minX) != 0 ? targR / (maxX - minX) : 1.0;
                    double scaleY = (maxY - minY) != 0 ? targR / (maxY - minY) : 1.0;
                    double scale = Math.Min(scaleX, scaleY);

                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if (nodes[i] != null)
                        {
                            nodes[i].XPoint = (nodes[i].XPoint - minX) * scale;
                            nodes[i].YPoint = (nodes[i].YPoint - minY) * scale;
                        }
                    }
                    var scaledQueryPoint = (
                        SourceX: (queryPoint.SourceX - minX) * scale,
                        SourceY: (queryPoint.SourceY - minY) * scale,
                        DestX: (queryPoint.DestX - minX) * scale,
                        DestY: (queryPoint.DestY - minY) * scale
                    );

                    minX = 0;
                    maxX = (maxX - minX) * scale;
                    minY = 0;
                    maxY = (maxY - minY) * scale;

                    if (Math.Abs(maxX - minX) < 1e-10)
                    {
                        minX = 0;
                        maxX = targR;
                    }
                    if (Math.Abs(maxY - minY) < 1e-10)
                    {
                        minY = 0;
                        maxY = targR;
                    }

                    scaleX = (width - 2 * margin) / (maxX - minX);
                    scaleY = (height - 2 * margin) / (maxY - minY);

                    PointF ToCanvas(double x, double y)
                    {
                        if (double.IsNaN(x) || double.IsNaN(y) || double.IsInfinity(x) || double.IsInfinity(y))
                            return new PointF(margin, margin);
                        float canvasX = (float)(margin + (x - minX) * scaleX);
                        float canvasY = (float)(height - margin - (y - minY) * scaleY);
                        canvasX = Math.Max(margin, Math.Min(canvasX, width - margin));
                        canvasY = Math.Max(margin, Math.Min(canvasY, height - margin));
                        return new PointF(canvasX, canvasY);
                    }

                    g.DrawLine(Pens.Black, margin, height - margin, width - margin, height - margin);
                    g.DrawLine(Pens.Black, margin, margin, margin, height - margin);

                    using (Font axisFont = new Font("Arial", 10))
                    {
                        for (int i = 0; i <= 10000; i += 2000)
                        {
                            float xPos = (float)(margin + (i * (width - 2 * margin) / 10000.0));
                            float yPos = (float)(height - margin - (i * (height - 2 * margin) / 10000.0));
                            g.DrawString(i.ToString(), axisFont, Brushes.Black, xPos - 10, height - margin + 10);
                            if (i > 0)
                                g.DrawString(i.ToString(), axisFont, Brushes.Black, margin - 30, yPos - 5);
                        }
                    }

                    PointF sourcePoint = ToCanvas(scaledQueryPoint.SourceX, scaledQueryPoint.SourceY);
                    PointF destPoint = ToCanvas(scaledQueryPoint.DestX, scaledQueryPoint.DestY);

                    foreach (var edge in edges)
                    {
                        if (edge == null || edge.IsWalking) continue;
                        var fromNode = nodes.Find(n => n != null && n.Id == edge.From);
                        var toNode = nodes.Find(n => n != null && n.Id == edge.To);
                        if (fromNode != null && toNode != null)
                        {
                            PointF p1 = ToCanvas(fromNode.XPoint, fromNode.YPoint);
                            PointF p2 = ToCanvas(toNode.XPoint, toNode.YPoint);
                            g.DrawLine(Pens.Black, p1, p2);
                        }
                    }

                    foreach (var node in nodes)
                    {
                        if (node == null || node.Id < 0) continue;
                        PointF p = ToCanvas(node.XPoint, node.YPoint);
                        if (node.Id == VIRTUAL_SOURCE_NODE_ID || node.Id == VIRTUAL_DESTINATION_NODE_ID) continue;
                        g.DrawEllipse(Pens.Black, p.X - 2, p.Y - 2, 4, 4);
                        g.DrawString(node.Id.ToString(), new Font("Arial", 2), Brushes.Black, p.X - 3, p.Y - 3);
                    }

                    if (paths != null && paths.Count > 0)
                    {
                        using (Pen redPen = new Pen(Color.Red, 12))
                        {
                            for (int j = 0; j < paths.Count - 1; j++)
                            {
                                var fromNode = nodes.Find(n => n != null && n.Id == paths[j]);
                                var toNode = nodes.Find(n => n != null && n.Id == paths[j + 1]);
                                if (fromNode != null && toNode != null)
                                {
                                    PointF p1 = ToCanvas(fromNode.XPoint, fromNode.YPoint);
                                    PointF p2 = ToCanvas(toNode.XPoint, toNode.YPoint);
                                    g.DrawLine(redPen, p1, p2);
                                }
                            }
                        }
                    }

                    if (!double.IsNaN(sourcePoint.X) && !double.IsNaN(destPoint.X))
                    {
                        g.FillEllipse(Brushes.Green, sourcePoint.X - 5, sourcePoint.Y - 5, 20, 20);
                        g.FillEllipse(Brushes.Blue, destPoint.X - 5, destPoint.Y - 5, 20, 20);
                    }

                    string caption = $"Figure 1: Map Routing Visualization for Query {queryPoint.SourceX},{queryPoint.SourceY} to {queryPoint.DestX},{queryPoint.DestY}";
                    g.DrawString(caption, new Font("Arial", 12), Brushes.Black, new PointF(margin, height - 30));

                    try
                    {
                        bm.Save(outputFile, ImageFormat.Png);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to save {outputFile}: {ex.Message}\nStack Trace: {ex.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during image creation or drawing in {outputFile}: {ex.Message}\nStack Trace: {ex.StackTrace}");
            }
        }
    }
}
