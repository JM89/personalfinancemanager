using AutoMapper;
using PFM.Services.Core.Automapper;
using System;


namespace PFM.UnitTests.Fixture
{
    public class AutoMapperFixture : IDisposable
    {
        public AutoMapperFixture()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ModelToEntityMapping>();
                cfg.AddProfile<EntityToModelMapping>();
                cfg.AddProfile<EntityToEntityMapping>();
                cfg.AddProfile<SearchParametersMapping>();
            });
        }

        public void Dispose()
        {
        }
    }
}
