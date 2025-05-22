using System;

namespace AN_Task_SolidPrinciples.Interface_Segregation_Principle
{
    internal class ISP_BadExample
    {
        // This interface is too broad — it forces implementing classes to define all methods,
        // even if some aren't relevant.
        public interface IMultiFunctionDevice
        {
            void Print(string document);
            void Scan(string document);
            void Fax(string document);
        }

        // OldPrinter only prints but is forced to implement Scan and Fax methods which it doesn't support.
        public class SimplePrinter : IMultiFunctionDevice
        {
            public void Print(string document)
            {
                Console.WriteLine("Printing: " + document);
            }

            public void Scan(string document)
            {
                throw new NotImplementedException("Scan not supported.");
            }

            public void Fax(string document)
            {
                throw new NotImplementedException("Fax not supported.");
            }
        }
    }
}
