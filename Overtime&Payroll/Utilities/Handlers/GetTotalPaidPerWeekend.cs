namespace server.Utilities.Handlers
{
    public class GetTotalPaidPerWeekend
    {
        public static int PaidPerWeekend(int totalHour, double salaryPerHours)
        {
            try
            {
                int paid = 0;
                for (int i = 0; i < totalHour; i++)
                {
                    if (i <= 8)
                    {
                        paid += 2 * (int)salaryPerHours;
                    }
                    else if (i == 9)
                    {
                        paid += 3 * (int)salaryPerHours;
                    }
                    else
                    {
                        paid += 4 * (int)salaryPerHours;
                    }
                }
                return paid;

            }
            catch
            {
                return 0;
            }
        }

    }
}
