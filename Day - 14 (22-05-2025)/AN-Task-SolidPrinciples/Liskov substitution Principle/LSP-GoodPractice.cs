using System;

namespace AN_Task_SolidPrinciples.Liskov_substitution_Principle
{
    internal class LSP_GoodPractice
    {
        // Base bird class for all birds
        public abstract class Bird
        {
            public abstract void Eat();
        }

        // Interface for flying behavior
        public interface IFlyable
        {
            void Fly();
        }

        public class Sparrow : Bird, IFlyable
        {
            public override void Eat()
            {
                Console.WriteLine("Sparrow is eating.");
            }

            public void Fly()
            {
                Console.WriteLine("Sparrow flying...");
            }
        }

        public class Penguin : Bird
        {
            public override void Eat()
            {
                Console.WriteLine("Penguin is eating.");
            }

            // No Fly method here because penguins can't fly
        }

        public static void Test()
        {
            Bird sparrow = new Sparrow();
            sparrow.Eat();
            ((IFlyable)sparrow).Fly();

            Bird penguin = new Penguin();
            penguin.Eat();
            // No Fly method called on penguin — safe and LSP compliant
        }
    }
}
