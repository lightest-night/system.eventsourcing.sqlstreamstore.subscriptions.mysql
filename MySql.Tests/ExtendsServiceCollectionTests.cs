using System;
using System.Reflection;
using LightestNight.System.Data.MySql;
using LightestNight.System.EventSourcing.Observers;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql.Tests
{
    public class ExtendsServiceCollectionTests
    {
        private readonly IServiceCollection _services;

        public ExtendsServiceCollectionTests()
        {
            _services = new ServiceCollection();    
        }
        
        [Fact]
        public void Should_Add_Test_Observers_Through_Assembly()
        {
            // Act
            _services.AddEventStoreSubscriptions(() => new MySqlOptions(), null, null, Assembly.GetExecutingAssembly());
            
            // Assert
            var result = _services.BuildServiceProvider().GetService<IEventObserver>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestObserver>();
        }

        [Fact]
        public void Should_Add_Test_Observers_Through_Assembly_Using_TypeFactory()
        {
            // Arrange
            var factoryUsed = false;
            
            // Act
            _services.AddEventStoreSubscriptions(() => new MySqlOptions(), (serviceProvider, observerType) =>
            {
                factoryUsed = true;
                return Activator.CreateInstance(observerType) as IEventObserver ??
                       throw new InvalidOperationException();
            }, null, Assembly.GetExecutingAssembly());
            
            // Assert
            var result = _services.BuildServiceProvider().GetService<IEventObserver>();
            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestObserver>();
            factoryUsed.ShouldBeTrue();
        }

        [Fact]
        public void Should_Add_Test_Observers_By_Instance()
        {
            // Arrange
            var testObserver = new TestObserver();
            
            // Act
            _services.AddEventStoreSubscriptions(() => new MySqlOptions(), null, testObserver);
            
            // Assert
            _services.BuildServiceProvider().GetService<IEventObserver>().ShouldBe(testObserver);
        }
    }
}