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

SlowCode performs reflection and other bad codes 100 times, and every 100 iterations has a (44.546 us) mean value!

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100-preview.6.20318.15
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT
  Job-MOOBGY : .NET Core 5.0.0 (CoreCLR 5.0.20.30506, CoreFX 5.0.20.30506), X64 RyuJIT

InvocationCount=1000000  RunStrategy=Throughput

```
|   Method |      Mean |     Error |    StdDev |
|--------- |----------:|----------:|----------:|
| SlowCode | 44.546 us | 0.2011 us | 0.1783 us |
| FastCode |  1.718 us | 0.0131 us | 0.0122 us |
