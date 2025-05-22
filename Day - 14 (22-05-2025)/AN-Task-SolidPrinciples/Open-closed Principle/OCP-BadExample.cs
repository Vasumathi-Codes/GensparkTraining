namespace AN_Task_SolidPrinciples.Open_closed_Principle
{
    internal class OCP_BadExample
    {
        public class MembershipFeeCalculator
        {
            public double CalculateFee(string membershipType)
            {
                if (membershipType == "Basic")
                {
                    return 50;
                }
                else if (membershipType == "Silver")
                {
                    return 100;
                }
                else if (membershipType == "Gold")
                {
                    return 150;
                } 
                else 
                {
                    return 0;
                }
            }
        }
    }
}
