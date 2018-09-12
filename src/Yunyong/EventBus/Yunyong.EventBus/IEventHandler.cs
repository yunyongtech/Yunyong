using System.Threading.Tasks;
using Yunyong.EventBus.Events;

namespace Yunyong.EventBus
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent e);
    }
}