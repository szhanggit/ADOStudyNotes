namespace Service.Utility
{
    public interface IClientHelperService
    {
        int GetSkipNum(int? PageNumber, int? RowCount);
    }
    public class ClientHelperService : IClientHelperService
    {
        public ClientHelperService()
        {

        }

        public int GetSkipNum(int? PageNumber, int? RowCount)
        {
            int skip = 0;
            skip = ((PageNumber ?? 0) - 1) * (RowCount ?? 0);
            return skip;
        }
    }
}
