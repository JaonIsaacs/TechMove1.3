using System;
using TechMove1._3.Domain.Interfaces;

namespace TechMove1._3.Application.Observers
{
    public class ContractSubject
    {
        private List<IObserver> observers = new();

        public void Attach(IObserver obs)
        {
            observers.Add(obs);
        }

        public void Notify(string msg)
        {
            foreach (var o in observers)
                o.Update(msg);
        }
    }
}
