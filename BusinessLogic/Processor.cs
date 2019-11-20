using System;
using System.Reactive.Linq;

namespace BusinessLogic
{
    public class Processor
    {
        private readonly Ingress _ingress;

        public Processor(Ingress ingress)
        {
            _ingress = ingress;
        }

        public void Run()
        {
            _ingress.IngressMessages.Subscribe(Handle);
        }

        private void Handle(string message)
        {
            Console.WriteLine($"Processing: {message}");
        }
    }
}
