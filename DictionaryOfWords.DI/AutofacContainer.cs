using Autofac;
using Autofac.Extensions.DependencyInjection;
using DictionaryOfWords.Core.Repositories;
using DictionaryOfWords.DAL.Repositories;
using DictionaryOfWords.DAL.Unit;
using DictionaryOfWords.DAL.Unit.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace DictionaryOfWords.DI
{
    public class AutofacContainer
    {
        public IContainer Build(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .OrderByDescending(a => a.FullName)
                .ToArray();

            ServicesRegister(ref builder, assemblies);
            RepositoriesRegister(ref builder, assemblies);

            return builder.Build();
        }

        private void ServicesRegister(ref ContainerBuilder builder, Assembly[] assemblies)
        {
            var servicesAssembly = assemblies.FirstOrDefault(t => t.FullName.ToLower().Contains("dictionaryofwords.service"));
            builder.RegisterAssemblyTypes(servicesAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }

        private void RepositoriesRegister(ref ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterGeneric(typeof(RepositoryBase<>))
                .As(typeof(IRepository<>));

            var dataAssembly = assemblies.FirstOrDefault(t => t.FullName.ToLower().Contains("dictionaryofwords.dal"));
            builder.RegisterAssemblyTypes(dataAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(UnitOfWork))
                .As(typeof(IUnitOfWork));
        }
    }
}
