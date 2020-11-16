using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Attributes;
using Modulos.Benchmarks.Common;
using Xunit;
using Xunit.Abstractions;

namespace Modulos.Benchmarks
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class check_structure_comparison_default_or_with_custom_comparer
    {
        private readonly ITestOutputHelper _console;
 
        public check_structure_comparison_default_or_with_custom_comparer(ITestOutputHelper console)
        {
            _console = console;
        }

        [Fact(Skip = "Only manually.")]
        public void Execute()
        {
            BenchmarkUtils.RunBenchmarkAndLogIntoXUnitConsole<Benchmark>(_console);
        }

        [MemoryDiagnoser]
        public class Benchmark
        {
           
            [GlobalSetup]
            public void Setup()
            {
               
            }



            [Benchmark]
            public bool Compare1()
            {
                var s1 = new Struct1(Guid.NewGuid());
                var s2 = new Struct1(Guid.NewGuid());

                return Equals(s1, s2);
            }
            [Benchmark]
            public bool Compare1_prim()
            {
                var s1 = new Struct1(Guid.NewGuid());
                var s2 = new Struct1(Guid.NewGuid());

                return s1.Equals(s2);
            }

            [Benchmark]
            public bool Compare2()
            {
                var s1 = new Struct2(Guid.NewGuid());
                var s2 = new Struct2(Guid.NewGuid());

                return Equals(s1, s2);
            }

            [Benchmark]
            public bool Compare2_prim()
            {
                var s1 = new Struct2(Guid.NewGuid());
                var s2 = new Struct2(Guid.NewGuid());

                return s1.Equals(s2);
            }

            [Benchmark]
            public bool Compare4()
            {
                var s1 = new Struct2(Guid.NewGuid());
                var s2 = new Struct2(Guid.NewGuid());

                return s1 == s2;
            
            }

            [Benchmark]
            public bool Compare5()
            {
                var s1 = new Class1(Guid.NewGuid());
                var s2 = new Class1(Guid.NewGuid());

                return s1 == s2;
            }

            [Benchmark]
            public bool Compare5_prim()
            {
                var s1 = new Class1(Guid.NewGuid());
                var s2 = new Class1(Guid.NewGuid());

                return Equals(s1, s2);
            }

            [Benchmark]
            public bool Compare5_v3()
            {
                var s1 = new Class1(Guid.NewGuid());
                var s2 = new Class1(Guid.NewGuid());

                return s1.Equals(s2);
            }

            internal struct Struct1
            {
                public Guid Id { get; private set; }

                public Struct1(Guid id)
                {
                    Id = id;
                }
            }


            internal sealed class Class1 : IEquatable<Class1>
            {
                public Guid Id { get; private set; }

                public Class1(Guid id)
                {
                    Id = id;
                }

                public bool Equals(Class1 other)
                {
                    if (ReferenceEquals(null, other)) return false;
                    if (ReferenceEquals(this, other)) return true;
                    return Id.Equals(other.Id);
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    return obj is Class1 other && Equals(other);
                }

                public override int GetHashCode()
                {
                    // ReSharper disable once NonReadonlyMemberInGetHashCode
                    return Id.GetHashCode();
                }

                public static bool operator ==(Class1 left, Class1 right)
                {
                    return Equals(left, right);
                }

                public static bool operator !=(Class1 left, Class1 right)
                {
                    return !Equals(left, right);
                }
            }

            internal struct Struct2 : IEquatable<Struct2>
            {
                public Guid Id { get; private set; }

                public Struct2(Guid id)
                {
                    Id = id;
                }

                public bool Equals(Struct2 other)
                {
                    return Id.Equals(other.Id);
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    return obj is Struct2 other && Equals(other);
                }

                public override int GetHashCode()
                {
                    // ReSharper disable once NonReadonlyMemberInGetHashCode
                    return Id.GetHashCode();
                }

                public static bool operator ==(Struct2 left, Struct2 right)
                {
                    return left.Id == right.Id;
                }

                public static bool operator !=(Struct2 left, Struct2 right)
                {
                    return left.Id != right.Id;
  
                }
            }
        }
    }
}