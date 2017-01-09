using HMapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestObjects;
using HMapper.Extensions;
using TestObjects.Business;
using DTO = TestObjects.DTO;

namespace HMapperConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            Init.InitHMapper();
            Init.InitAutomapper();
            Init.InitExpressMapper();

            var test=AutoMapper.Mapper.Map<PolymorphicSubClass, DTO.PolymorphicSubClass>(PolymorphicSubClass.Create(5));

            Run("Very simple class", VerySimpleClass.CreateMany(300000), x => new DTO.VerySimpleClass(x));
            Run("Simple class", SimpleClass.CreateMany(40000), x => new DTO.SimpleClass(x));
            Run("Simple set", SimpleSet.CreateMany(10000, 100), x => new DTO.SimpleSet(x));
            Run("Multiple set", MultipleSets.CreateMany(1000, 100), x => new DTO.MultipleSets(x));
            Run("Dictionary", DictionarySet.CreateMany(300, 100), x => new DTO.DictionarySet(x));

            Run("Simple generic class of int", SimpleGeneric<int>.CreateMany(Enumerable.Range(1, 300000).Select(i => i).ToArray()), x => new DTO.SimpleGeneric<int>(x));
            Run("Mapped object generic class", MappedObjectGeneric<VerySimpleClass>.CreateMany(VerySimpleClass.CreateMany(200000)), x => new DTO.MappedObjectGeneric<DTO.VerySimpleClass>(x));
            Run("Multiple generic class", MultipleGenerics<int, string>.CreateMany(Enumerable.Range(1, 200000).Select(i => Tuple.Create(i, Guid.NewGuid().ToString())).ToArray()), x => new DTO.MultipleGenerics<string, int>(x));
            Run("Polymorphic class", PolymorphicSubSubClass.CreateMany(300000), x => new DTO.PolymorphicSubClass(x));
            Run("Set of polymorphic class", SetOfPolymorphic.CreateMany(150000), x => new DTO.SetOfPolymorphic(x));
            Run("Generic of polymorphic class", MappedObjectGeneric<PolymorphicBaseClass>.CreateMany(PolymorphicBaseClass.CreateMany(200000)), x => new DTO.MappedObjectGeneric<DTO.PolymorphicBaseClass>(x));
            Run("Set of generic polymorphic classes", SetOfGenericPolymorph.CreateMany(50000), x => new DTO.SetOfGenericPolymorph(x));
        }

        static void Run<TSource, TTarget>(string description, TSource[] sources, Func<TSource, TTarget> targetFactory)
        {
            int nb = sources.Length;
            Stopwatch sw = new Stopwatch();

            // warmup and tests if supported
            bool hMapSupport = false;
            bool autoMapSupport = false;
            bool expressMapSupport = false;
            var manualMap = targetFactory(sources[0]);

            try
            {
                var hMapperMap = Mapper.Map<TSource, TTarget>(sources[0]);
                if (manualMap.Equals(hMapperMap))
                    hMapSupport = true;
            }
            catch { }

            try
            {
                var autoMapperMap = AutoMapper.Mapper.Map<TSource, TTarget>(sources[0]);
                if (manualMap.Equals(autoMapperMap))
                    autoMapSupport = true;
            }
            catch { }

            try
            {
                var expressMapperMap = ExpressMapper.Mapper.Map<TSource, TTarget>(sources[0]);
                if (manualMap.Equals(expressMapperMap))
                    expressMapSupport = true;
            }
            catch { }

            long durationManual = -1;
            long durationAutoMapper = -1;
            long durationHMapper = -1;
            long durationExpressMapper = -1;

            // Manual mapping
            sw.Start();
            for (int i = 0; i < nb; i++)
                targetFactory(sources[i]);
            sw.Stop();
            durationManual = sw.ElapsedMilliseconds;

            // Automapper
            if (autoMapSupport)
            {
                sw.Restart();
                for (int i = 0; i < nb; i++)
                    AutoMapper.Mapper.Map<TSource, TTarget>(sources[i]);
                sw.Stop();
                durationAutoMapper = sw.ElapsedMilliseconds;
            }

            // HMapper
            if (hMapSupport)
            {
                sw.Restart();
                for (int i = 0; i < nb; i++)
                    Mapper.Map<TSource, TTarget>(sources[i]);
                sw.Stop();
                durationHMapper = sw.ElapsedMilliseconds;
            }

            //Express Mapper
            if (expressMapSupport)
            {
                sw.Restart();
                for (int i = 0; i < nb; i++)
                    ExpressMapper.Mapper.Map<TSource, TTarget>(sources[i]);
                sw.Stop();
                durationExpressMapper = sw.ElapsedMilliseconds;
            }

            Console.WriteLine($"{description}:");
            Console.WriteLine($"Manual : {durationManual}");
            Console.WriteLine($"H-mapper : {durationHMapper}");
            Console.WriteLine($"Automapper : {durationAutoMapper}");
            Console.WriteLine($"Expressmapper : {durationExpressMapper}");
            Console.WriteLine("");



            /* Previous result
             * 
             * 
             * 
Very simple class:
Manual : 1494
H-mapper : 1504
Automapper : 19675
Expressmapper : 10467

Simple class:
Manual : 3779
H-mapper : 4659
Automapper : 6096
Expressmapper : 7574

Simple set:
Manual : 3195
H-mapper : 251
Automapper : 799
Expressmapper : 594

Multiple set:
Manual : 9816
H-mapper : 9639
Automapper : 7247
Expressmapper : 12604

Dictionary:
Manual : 3444
H-mapper : 3010
Automapper : 2339
Expressmapper : -1

Simple generic class of int:
Manual : 2256
H-mapper : 2284
Automapper : 19165
Expressmapper : 12126

Mapped object generic class:
Manual : 2728
H-mapper : 2444
Automapper : 13975
Expressmapper : 9230

Multiple generic class:
Manual : 2108
H-mapper : 2135
Automapper : 12756
Expressmapper : 9295

Polymorphic class:
Manual : 1915
H-mapper : 1675
Automapper : 17223
Expressmapper : 10863

Set of polymorphic class:
Manual : 4972
H-mapper : 2528
Automapper : 36134
Expressmapper : -1

Generic of polymorphic class:
Manual : 2826
H-mapper : 3602
Automapper : 24029
Expressmapper : 9247

Set of generic polymorphic classes:
Manual : 2634
H-mapper : 2456
Automapper : 13836
Expressmapper : -1


    */
        }
    }
}
