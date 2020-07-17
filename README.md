We've a slow code which is known to be very slow! We're going to benchmark it to find the fact!

```cs
public async Task<int> SlowCode()
{
    int finalResult = 0;

    for (int i = 0; i < 100; i++)
    {
        var result1 = (ResultClass)typeof(Tests).GetMethod("Sum1")/*Reflection*/.Invoke(this, new object[] { 1, 2 /*Boxing*/ })/*Dynamic Dispath*/;
        var result2 = await ((Task<ResultClass>)typeof(Tests).GetMethod("Sum1Async")/*Reflection*/.Invoke(this, new object[] { 1, 2 /*Boxing*/ }))/*Dynamic Dispath*/;
        // await without configure await false (I know console apps have no sync context!)
        finalResult += result1.Sum + result2.Sum;

        try
        {
            throw new DivideByZeroException(); // exception
        }
        catch { }
    }

    return finalResult;
}
```

[SlowCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L29-L45) SlowCode performs reflection and other bad codes 100 times. It also throws an exception 100 times! Every 100 iterations only took 551.600 us!

I'm going to compare its performance with [FastCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L51-L60) which uses struct instead of class, it uses direct method call instead of reflection and it uses ValueTask instead of Task.

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.6.20318.15
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT
  Job-JWLQUD : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT

RunStrategy=Throughput
```

|   Method |       Mean |      Error |     StdDev |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-----------:|-----------:|-----------:|--------:|------:|------:|----------:|
| SlowCode | 565.852 us | 11.1719 us | 12.8655 us | 13.6719 |     - |     - |   57675 B |
| FastCode |   1.447 us |  0.0101 us |  0.0095 us |       - |     - |     - |         - |

We also have fast api & slow api
In slow api, we throws ResourceNotFoundException which gets mapped to 404 status code later in a middleware.
There's also a fast api which simply returns Not found

Using https://github.com/rogerwelin/cassowary we achieved following results:

Fast: 10499.31 req/s
Slow: 10050.26 req/s

Note that in real world, api throws exceptions rarely, and api logic is more complicated than a simple return!
For example api needs authorization, authenticatin, database and cache access etc.
So, this load test is testing worse case!

Note that I've tested these on Windows. Results might be different on Linux!
