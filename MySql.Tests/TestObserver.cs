using System;
using System.Threading;
using System.Threading.Tasks;
using LightestNight.System.EventSourcing.Events;
using LightestNight.System.EventSourcing.Observers;

namespace LightestNight.System.EventSourcing.SqlStreamStore.Subscriptions.MySql.Tests
{
    public class TestObserver : IEventObserver
    {
        public Task InitialiseObserver(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task EventReceived(EventSourceEvent evt, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}