namespace AN_Task_SolidPrinciples.Open_closed_Principle
{
    internal class OCP_GoodPractice
    {
        public interface IMembership
        {
            double CalculateFee();
        }

        public class BasicMembership : IMembership
        {
            public double CalculateFee() => 50;
        }

        public class SilverMembership : IMembership
        {
            public double CalculateFee() => 100;
        }

        public class GoldMembership : IMembership
        {
            public double CalculateFee() => 150;
        }



        public class MembershipFeeCalculator
        {
            public double CalculateFee(IMembership membership)
            {
                return membership.CalculateFee();
            }
        }
    }
}
