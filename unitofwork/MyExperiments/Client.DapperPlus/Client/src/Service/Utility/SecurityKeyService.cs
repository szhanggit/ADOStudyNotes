namespace Service.Utility
{
    public class SecurityKeyService
    {
        private static char[] charMaps = new char[34] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static Dictionary<char, char> confusionDictionary = new Dictionary<char, char> { { '0', '0' }, { '1', '1' }, { '2', '2' }, { '3', '3' }, { '4', '4' }, { '5', '5' }, { '6', '6' }, { '7', '7' }, { '8', '8' }, { '9', '9' }, { 'A', '0' }
        ,{ 'B', '5' },{ 'C', '4' },{ 'D', '4' },{ 'E', '1' },{ 'F', '9' },{ 'G', '4' },{ 'H', '6' },{ 'I', '5' },{ 'J', '9' },{ 'K', '4' },{ 'L', '8' },{ 'M', '2' },{ 'N', '8' },{ 'O', '7' },{ 'P', '7' },{ 'Q', '3' },{ 'R', '9' },{ 'S', '3' }
        ,{ 'T', '3' },{ 'U', '7' },{ 'V', '7' },{ 'W', '7' },{ 'X', '6' },{ 'Y', '3' },{ 'Z', '3' }};
        private static Random random = new Random();

        /// <summary>
        /// GenerateSecurityKey
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateSecurityKey(int length)
        {
            string SecurityKey = string.Empty;

            int charPosition = 16;
            if (length > 0)
            {
                while (length-- > 0)
                {
                    SecurityKey += charMaps[random.Next(0, charPosition)];
                }
            }

            var confusedVoucherNumber = string.Empty;

            SecurityKey.ToCharArray().ToList().ForEach(c => confusedVoucherNumber += confusionDictionary[c]);

            SecurityKey += DammAlgorithm.CalculateCheckSum(confusedVoucherNumber);

            return SecurityKey;
        }
    }
}
