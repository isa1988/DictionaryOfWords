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
    // Создавать целую сборку под единственный публичный метод, как по мне, избыточно.
    // Если двигаться таким путем, то солюшн может быстро обрости мелкими DLL-ками с очень ограниченным функционалом.
    // Это не так плохо - разделять код по ответственности, но баланс между читаемостью кода и разделением тоже нужно искать.
    public class AutofacContainer
    {
        public IContainer Build(IServiceCollection services)
        {
            Service.AssemblyRunner.Run();

            var builder = new ContainerBuilder();
            builder.Populate(services);
            var assemblies = AppDomain.CurrentDomain
                .GetAssemblies()
                .OrderByDescending(a => a.FullName)
                .ToArray();

            // builder и так референсный тип, так что ref не нужен.
            ServicesRegister(ref builder, assemblies);
            RepositoriesRegister(ref builder, assemblies);

            return builder.Build();
        }

        private void ServicesRegister(ref ContainerBuilder builder, Assembly[] assemblies)
        {
            var servicesAssembly = assemblies.FirstOrDefault(t => t.FullName.ToLower().Contains("dictionaryofwords.service"));

            // Из-за FirstOrDefault ты можешь вызвать NRE здесь.
            builder.RegisterAssemblyTypes(servicesAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces();
        }

        private void RepositoriesRegister(ref ContainerBuilder builder, Assembly[] assemblies)
        {
            builder.RegisterGeneric(typeof(RepositoryBase<>))
                .As(typeof(IRepositoryBase<>));

            var dataAssembly = assemblies.FirstOrDefault(t => t.FullName.ToLower().Contains("dictionaryofwords.dal"));

            // Из-за FirstOrDefault ты можешь вызвать NRE здесь.
            builder.RegisterAssemblyTypes(dataAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();

            builder.RegisterType(typeof(UnitOfWork))
                .As(typeof(IUnitOfWork));
        }
    }
}
