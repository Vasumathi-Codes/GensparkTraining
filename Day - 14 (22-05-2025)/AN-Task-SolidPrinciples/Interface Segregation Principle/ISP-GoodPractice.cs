using System;

namespace AN_Task_SolidPrinciples.Interface_Segregation_Principle
{
    internal class ISP_GoodPractice
    {
        // Segregated interfaces: smaller, focused interfaces.
        public interface IPrinter
        {
            void Print(string document);
        }

        public interface IScanner
        {
            void Scan(string document);
        }

        public interface IFax
        {
            void Fax(string document);
        }

        // OldPrinter only implements printing, no need to worry about Scan or Fax.
        public class OldPrinter : IPrinter
        {
            public void Print(string document)
            {
                Console.WriteLine("Printing: " + document);
            }
        }

        // MultiFunctionPrinter supports printing, scanning and faxing.
        public class MultiFunctionPrinter : IPrinter, IScanner, IFax
        {
            public void Print(string document)
            {
                Console.WriteLine("Printing: " + document);
            }

            public void Scan(string document)
            {
                Console.WriteLine("Scanning: " + document);
            }

            public void Fax(string document)
            {
                Console.WriteLine("Faxing: " + document);
            }
        }
    }
}
