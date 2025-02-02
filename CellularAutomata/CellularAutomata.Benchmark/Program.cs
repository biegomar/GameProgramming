// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using CellularAutomata.Benchmark;

var summary = BenchmarkRunner.Run<AutomataBenchmark>();
Console.ReadKey();