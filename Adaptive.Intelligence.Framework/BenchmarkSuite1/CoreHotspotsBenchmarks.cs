using Adaptive.Intelligence.Extensions;
using Adaptive.Intelligence.Utility;
using BenchmarkDotNet.Attributes;
using Microsoft.VSDiagnostics;

namespace Adaptive.Intelligence.Framework.Benchmarks;
[CPUUsageDiagnoser]
public class CoreHotspotsBenchmarks
{
    private byte[][] _byteSegments = [];
    private string[] _words = [];
    [GlobalSetup]
    public void Setup()
    {
        _byteSegments = new byte[128][];
        for (int i = 0; i < _byteSegments.Length; i++)
        {
            _byteSegments[i] = ByteArrayUtil.CreateRandomArray(256);
        }

        _words = ["companies", "classes", "routes", "toy", "boy", "property", "analysis", "types", "plates", "city"];
    }

    [Benchmark]
    public byte[] ConcatenateArrays_128x256() => ByteArrayUtil.ConcatenateArrays(_byteSegments);
    [Benchmark]
    public int Singularize_10Words()
    {
        int total = 0;
        for (int i = 0; i < _words.Length; i++)
        {
            total += _words[i].Singularize().Length;
        }

        return total;
    }

    [Benchmark]
    public int Pluralize_10Words()
    {
        int total = 0;
        for (int i = 0; i < _words.Length; i++)
        {
            total += _words[i].Pluralize().Length;
        }

        return total;
    }
}