namespace VoNguyenHoangKim.Common.Validate
{
    public class IdGenerator
    {
        private static IdGenerator _instance;
        private static readonly object _lock = new object();

        public IdGenerator() { }

        public static IdGenerator Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new IdGenerator();
                }
            }
        }

        public string GenerateId(string prefix, string lastId)
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(lastId))
                {
                    return $"{prefix}00000001";
                }

                var number = int.Parse(lastId.Replace(prefix, "")) + 1;
                return $"{prefix}{number:D8}";
            }
        }
    }
}