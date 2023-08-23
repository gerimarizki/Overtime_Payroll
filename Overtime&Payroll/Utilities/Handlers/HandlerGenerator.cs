namespace server.Utilities.Handlers
{
    public class HandlerGenerator
    {
        public static string NIK(string? lastNIK = null)
        {
            if (lastNIK is null)
            {
                return "111111";
            }

            var generateNIK = Convert.ToInt32(lastNIK) + 1;
            return generateNIK.ToString();
        }

        public static string OvertimeId(string? OvertimeId = null)
        {
            if (OvertimeId is null)
            {
                return "11111111";
            }

            var generateNumber = Convert.ToInt32(OvertimeId) + 1;
            return generateNumber.ToString();
        }

        public static int RandomOTP()
        {
            Random random = new Random();
            HashSet<int> uniqueDigits = new HashSet<int>();
            while (uniqueDigits.Count < 6)
            {
                int digit = random.Next(0, 9);
                uniqueDigits.Add(digit);
            }

            int generatedOTP = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

            return generatedOTP;
        }
    }
}
