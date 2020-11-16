using System;
using System.Linq;
using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Running;
using Xunit.Abstractions;

namespace Modulos.Benchmarks.Common
{
    public static class BenchmarkUtils
    {
        public static void RunBenchmarkAndLogIntoXUnitConsole<TBenchmark>(ITestOutputHelper console) where TBenchmark : class 
        { 
            var logger = new XUnitLoggerForBenchmarkNet();

            //var config = new ManualConfig
            //{
            //    UnionRule = ConfigUnionRule.AlwaysUseGlobal
            //};
            //config.Add(MemoryDiagnoser.Default);
            //config.Add(logger);
            //config.Add(DefaultConfig.Instance.GetColumnProviders().ToArray());
            //config.Add(DefaultConfig.Instance.GetExporters().ToArray());
            //config.Add(DefaultConfig.Instance.GetDiagnosers().ToArray());
            //config.Add(DefaultConfig.Instance.GetAnalysers().ToArray());
            //config.Add(DefaultConfig.Instance.GetJobs().ToArray());
            //config.Add(DefaultConfig.Instance.GetValidators().ToArray());
            //// Overriding the default

            var summary = BenchmarkRunner.Run<TBenchmark>();
            
            MarkdownExporter.Console.ExportToLog(summary, logger);

          
            //ConclusionHelper.Print(logger, summary.Config.GetCompositeAnalyser().Analyse(summary).ToList());
            ConclusionHelper.Print(logger, summary.BenchmarksCases[0].Config.GetCompositeAnalyser().Analyse(summary).ToList());
            foreach (var line in logger.ToString().Split(Environment.NewLine))
            {
                console.WriteLine(line);
            }

            
        }
    }

    //private class Config : ManualConfig
    //{
    //    public Config()
    //    {
    //        Add(
    //            new Job("MySuperJob", RunMode.Dry, EnvMode.RyuJitX64)
    //            {
    //                Env = { Runtime = Runtime.Core },
    //                Run = { LaunchCount = 5, IterationTime = TimeInterval.Millisecond * 200 },
    //                Accuracy = { MaxStdErrRelative = 0.01 }
    //            });

    //        // The same, using the .With() factory methods:
    //        Add(
    //            Job.Dry
    //                .With(Platform.X64)
    //                .With(Jit.RyuJit)
    //                .With(Runtime.Core)
    //                .WithLaunchCount(5)
    //                .WithIterationTime(TimeInterval.Millisecond * 200)
    //                .WithMaxStdErrRelative(0.01)
    //                .WithId("MySuperJob"));
    //    }
    //}
}