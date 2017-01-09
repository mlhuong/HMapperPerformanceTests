using HMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business = TestObjects.Business;
using DTO = TestObjects.DTO;

namespace HMapperConsoleTests
{
    public class Init
    {

        public static void InitHMapper()
        {
            MapConfig.Initialize(initializer =>
            {
                initializer.Map<Business.VerySimpleClass, DTO.VerySimpleClass>();
                initializer.Map<Business.SimpleClass, DTO.SimpleClass>()
                    .WithMember(x => x.Key, api => api.LinkTo(x => x.Id))
                    .WithMember(x => x.Date_Plus_2, api => api.LinkTo(x => x.Date.AddDays(2)))
                    .WithMember(nameof(DTO.SimpleClass.StringToBeIgnored), api => api.Ignore())
                    .WithMember(x => x.VerySimpleClass2String, api => api.LinkTo(x => x.VerySimpleClass2.MyString));
                initializer.Map<Business.SimpleClass, DTO.SimpleClass2>()
                    .WithMember(x => x.Key, api => api.LinkTo(nameof(Business.SimpleClass.Id)));
                initializer.Map<Business.SimpleSet, DTO.SimpleSet>();
                initializer.Map<Business.MultipleSets, DTO.MultipleSets>();
                initializer.Map<Business.DictionarySet, DTO.DictionarySet>();
                initializer.Map<Business.SimpleGeneric<TGen1>, DTO.SimpleGeneric<TGen1>>();
                initializer.Map<Business.MappedObjectGeneric<TGen1>, DTO.MappedObjectGeneric<TGen1>>();
                initializer.Map<Business.MultipleGenerics<TGen2, TGen1>, DTO.MultipleGenerics<TGen1, TGen2>>();
                initializer.Map<Business.PolymorphicSubClass, DTO.PolymorphicSubClass>();
                initializer.Map<Business.PolymorphicBaseClass, DTO.PolymorphicBaseClass>();
                initializer.Map<Business.SetOfPolymorphic, DTO.SetOfPolymorphic>();
                initializer.Map<Business.SetOfGenericPolymorph, DTO.SetOfGenericPolymorph>();
            });


        }
        public static void InitAutomapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Business.VerySimpleClass, DTO.VerySimpleClass>();
                cfg.CreateMap<Business.SimpleClass, DTO.SimpleClass>()
                    .ForMember(x => x.Key, api => api.MapFrom(x => x.Id))
                    .ForMember(x => x.Date_Plus_2, api => api.MapFrom(x => x.Date.AddDays(2)))
                    .ForMember(x => x.StringToBeIgnored, api => api.Ignore());
                cfg.CreateMap<Business.SimpleClass, DTO.SimpleClass2>()
                    .ForMember(x => x.Key, api => api.MapFrom(x => x.Id));
                cfg.CreateMap<Business.SimpleSet, DTO.SimpleSet>();
                cfg.CreateMap<Business.MultipleSets, DTO.MultipleSets>();
                cfg.CreateMap<Business.DictionarySet, DTO.DictionarySet>();
                cfg.CreateMap(typeof(Business.SimpleGeneric<>), typeof(DTO.SimpleGeneric<>));
                cfg.CreateMap(typeof(Business.MappedObjectGeneric<>), typeof(DTO.MappedObjectGeneric<>));
                cfg.CreateMap(typeof(Business.MultipleGenerics<,>), typeof(DTO.MultipleGenerics<,>));
                cfg.CreateMap<Business.PolymorphicBaseClass, DTO.PolymorphicBaseClass>();
                cfg.CreateMap<Business.PolymorphicSubClass, DTO.PolymorphicSubClass>()
                    .IncludeBase<Business.PolymorphicBaseClass, DTO.PolymorphicBaseClass>();
                cfg.CreateMap<Business.SetOfPolymorphic, DTO.SetOfPolymorphic>();
                cfg.CreateMap<Business.SetOfGenericPolymorph, DTO.SetOfGenericPolymorph>();
            });
        }

        public static void InitExpressMapper()
        {
            ExpressMapper.Mapper.Register<Business.VerySimpleClass, DTO.VerySimpleClass>();
            ExpressMapper.Mapper.Register<Business.SimpleClass, DTO.SimpleClass>()
                    .Member(x => x.Key, x => x.Id)
                    .Member(x => x.Date_Plus_2, x => x.Date.AddDays(2))
                    .Ignore(x => x.StringToBeIgnored);
            ExpressMapper.Mapper.Register<Business.SimpleClass, DTO.SimpleClass2>();
            ExpressMapper.Mapper.Register<Business.SimpleSet, DTO.SimpleSet>();
            ExpressMapper.Mapper.Register<Business.MultipleSets, DTO.MultipleSets>();
            ExpressMapper.Mapper.Register<Business.PolymorphicBaseClass, DTO.PolymorphicBaseClass>();
            ExpressMapper.Mapper.Register<Business.PolymorphicSubClass, DTO.PolymorphicSubClass>();
            ExpressMapper.Mapper.Register<Business.SetOfPolymorphic, DTO.SetOfPolymorphic>();
            ExpressMapper.Mapper.Register<Business.SetOfGenericPolymorph, DTO.SetOfGenericPolymorph>();
        }
    }
}
