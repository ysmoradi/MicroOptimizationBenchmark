[SlowCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L28-L40) has reflection, uses Task instead of ValueTask. It also suffers from boxing and uses class intead of struct.

I'm going to compare its performance with [FastCode](https://github.com/ysmoradi/MicroOptimizationBenchmark/blob/master/MicroOptimizationBenchmark/Program.cs#L43-L55) which uses struct instead of class, it uses direct method call instead of reflection and it uses ValueTask instead of Task. It also uses struct instead of class.

SlowCode performs reflection and other bad codes 100 times, and each time it tooks 48.459 us.

``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.503 (1809/October2018Update/Redstone5)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT
  Job-AYLPYT : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT

InvocationCount=1000000  RunStrategy=Throughput  

```
|   Method |      Mean |     Error |    StdDev |
|--------- |----------:|----------:|----------:|
| SlowCode | 48.459 us | 0.0456 us | 0.0381 us |
| FastCode |  2.247 us | 0.0023 us | 0.0021 us |
