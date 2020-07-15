We've a slow code which is known to be very slow! We're going to benchmark it to find the fact!

```cs
public async Task<int> SlowCode()
{
     int finalResult = 0;
            
     for (int i = 0; i < 100; i++)
     {
          var result1 = (ResultClass)typeof(Tests).GetMethod("Sum1").Invoke(this, new object[] { 1, 2 });
          var result2 = await ((Task<ResultClass>)typeof(Tests).GetMethod("Sum1Async").Invoke(this, new object[] { 1, 2 }));
          finalResult += result1.Sum + result2.Sum;
     }
            
     return finalResult;
}
```

[SlowCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L28-L40) has reflection, uses Task instead of ValueTask. It also suffers from boxing and uses class intead of struct.

I'm going to compare its performance with [FastCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L43-L55) which uses struct instead of class, it uses direct method call instead of reflection and it uses ValueTask instead of Task.

SlowCode performs reflection and other bad codes 100 times. It also throws an exception 100 times! Every 100 iterations only took 551.600 us!

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.388 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.6.20318.15
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT
  Job-JWLQUD : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT

RunStrategy=Throughput
```

|   Method |       Mean |     Error |    StdDev |   Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-----------:|----------:|----------:|--------:|------:|------:|----------:|
| SlowCode | 551.600 us | 4.7328 us | 3.9521 us | 13.6719 |     - |     - |   57675 B |
| FastCode |   1.752 us | 0.0175 us | 0.0163 us |       - |     - |     - |         - |
