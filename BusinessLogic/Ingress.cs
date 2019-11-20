using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace BusinessLogic
{
    public class Ingress
    {
        private readonly Subject<string> _sub;

        public IObservable<string> IngressMessages => _sub.AsObservable();

        public Ingress()
        {
            _sub = new Subject<string>();
        }

        public void OnMessage(string message)
        {
            _sub.OnNext(message);
        }
    }
}
