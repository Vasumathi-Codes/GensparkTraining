using System;


// An object of superclass should be able to replace with the object of its subclass,
// without affecting the program

//A(Parent) => B(Child)
// A a = new A();
// A a = new B() should work fine

//Because B inherits from A, it should follow all rules and contracts set by A.
//It should do everything A promises to do.
//It shouldn't throw errors or change behavior in ways that surprise code expecting A.

namespace AN_Task_SolidPrinciples.Liskov_substitution_Principle
{
    internal class LSP_BadExample
    {
        public class Bird
        {
            public virtual void Fly()
            {
                Console.WriteLine("Flying...");
            }
        }

        public class Sparrow : Bird
        {
            public override void Fly()
            {
                Console.WriteLine("Sparrow flying...");
            }
        }

        public class Penguin : Bird
        {
            public override void Fly()
            {
                // This breaks LSP because Penguin can't fly, but Fly() must be supported
                throw new NotSupportedException("Penguins can't fly!");
            }
        }

        public static void Test()
        {
            Bird sparrow = new Sparrow();
            sparrow.Fly();  // Works fine

            Bird penguin = new Penguin();
            penguin.Fly();  // Throws exception - breaks LSP!
        }
    }
}
