namespace Service.Utility
{
    public delegate int CalculateCheckSumDel(string number);
    public delegate int CalculateCheckSumWithIntDel(int number, CalculateCheckSumDel calculateCheckSum);
    public delegate int CalculateCheckSumWithLongDel(long number, CalculateCheckSumDel calculateCheckSum);
    public interface IDammAlgorithm
    {
        int CalculateCheckSum(string number);
        int CalculateCheckSum(int number, CalculateCheckSumDel calculateCheckSum);
        int CalculateCheckSum(long number, CalculateCheckSumDel calculateCheckSum);
        string GenerateCheckSum(string number, CalculateCheckSumDel calculateCheckSum);
        int GenerateCheckSum(int number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithIntDel calculateCheckSumWithInt);
        long GenerateCheckSum(long number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithLongDel calculateCheckSumWithLong);
        bool Validate(string number, CalculateCheckSumDel calculateCheckSum);
        bool Validate(int number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithIntDel calculateCheckSumWithInt);
        bool Validate(long number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithLongDel calculateCheckSumWithLong);
    }
    public class DammAlgorithm : IDammAlgorithm
    {
        /// <summary>
        /// The quasigroup table from http://en.wikipedia.org/wiki/Damm_algorithm
        /// </summary>

        static int[,] matrix = new int[,]
        {
            {0, 3, 1, 7, 5, 9, 8, 6, 4, 2},
            {7, 0, 9, 2, 1, 5, 4, 8, 6, 3},
            {4, 2, 0, 6, 8, 7, 1, 3, 5, 9},
            {1, 7, 5, 0, 9, 8, 3, 4, 2, 6},
            {6, 1, 2, 3, 0, 4, 5, 9, 7, 8},
            {3, 6, 7, 4, 2, 0, 9, 5, 8, 1},
            {5, 8, 6, 9, 7, 2, 0, 1, 3, 4},
            {8, 9, 4, 5, 3, 6, 2, 0, 1, 7},
            {9, 4, 3, 8, 6, 1, 7, 2, 0, 5},
            {2, 5, 8, 1, 4, 3, 6, 7, 9, 0}
        };

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public int CalculateCheckSum(string number)
        {
            var numbers = (from n in number select int.Parse(n.ToString()));
            int interim = 0;
            var en = numbers.GetEnumerator();
            while (en.MoveNext())
            {
                interim = matrix[interim, en.Current];
            }
            return interim;
        }

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public int CalculateCheckSum(int number, CalculateCheckSumDel calculateCheckSum)
        {
            return calculateCheckSum(number.ToString());
        }

        /// <summary>
        /// Calculate the checksum digit from provided number
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>Damm checksum</returns>
        public int CalculateCheckSum(long number, CalculateCheckSumDel calculateCheckSum)
        {
            return calculateCheckSum(number.ToString());
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public string GenerateCheckSum(string number, CalculateCheckSumDel calculateCheckSum)
        {
            var checkSumNumber = calculateCheckSum(number);
            return number + checkSumNumber.ToString();
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public int GenerateCheckSum(int number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithIntDel calculateCheckSumWithInt)
        {
            var checkSumNumber = calculateCheckSumWithInt(number, calculateCheckSum);
            return (number * 10) + checkSumNumber;
        }

        /// <summary>
        /// Calculate the checksum digit from provided number and return the full number with the checksum
        /// </summary>
        /// <param name="number">the number</param>
        /// <returns>full number with the Damm checksum</returns>
        public long GenerateCheckSum(long number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithLongDel calculateCheckSumWithLong)
        {
            var checkSumNumber = calculateCheckSumWithLong(number, calculateCheckSum);
            return (number * 10) + checkSumNumber;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public bool Validate(string number, CalculateCheckSumDel calculateCheckSum)
        {
            return calculateCheckSum(number) == 0;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public bool Validate(int number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithIntDel calculateCheckSumWithInt)
        {
            return calculateCheckSumWithInt(number, calculateCheckSum) == 0;
        }

        /// <summary>
        /// validates the number using the last digit as the Damm checksum
        /// </summary>
        /// <param name="number">the number to check</param>
        /// <returns>True if valid; otherwise false</returns>
        public bool Validate(long number, CalculateCheckSumDel calculateCheckSum, CalculateCheckSumWithLongDel calculateCheckSumWithLong)
        {
            return calculateCheckSumWithLong(number, calculateCheckSum) == 0;
        }
    }
}
