using BenchmarkDotNet.Attributes;
using StringInterpolation;
using System.Buffers;

namespace StringInterpolationBenchmark;

[MemoryDiagnoser]
public class JoinBenchmark
{
    private static IEnumerable<int> Fibonacci()
    {
        int current = 1, next = 1;

        while (true)
        {
            yield return current;
            next = current + (current = next);
        }
    }

    [Params(10, 100, 1000/*, 10_000*/)]
    public int N { get; set; }

    // baseline
    [Benchmark]
    public string StringJoin() => $"new[] {{ {string.Join(", ", Fibonacci().Take(N))} }}";

    [Benchmark]
    public string StringJoinWithBuffer()
    {
        var buffer = ArrayPool<char>.Shared.Rent(10 * N);
        var s = string.Create(null, buffer,
            $"new[] {{ {string.Join(", ", Fibonacci().Take(N))} }}");
        ArrayPool<char>.Shared.Return(buffer);
        return s;
    }

    // less allocated but much slower
    [Benchmark]
    public string FormatJoin() => $"new[] {{ {Format.Join(", ", Fibonacci().Take(N))} }}";

    // least allocated and faster
    [Benchmark]
    public string FormatJoinWithBuffer()
    {
        var buffer = ArrayPool<char>.Shared.Rent(10 * N);
        var s = string.Create(null, buffer,
            $"new[] {{ {Format.Join(", ", Fibonacci().Take(N))} }}");
        ArrayPool<char>.Shared.Return(buffer);
        return s;
    }
}
