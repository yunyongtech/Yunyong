namespace Yunyong.Core
{
    public static class PagingSetting
    {
        private static int _defaultPageSize = 10;

        public static int DefaultPageSize
        {
            get => _defaultPageSize;
            set
            {
                if (value <= 0)
                {
                    value = 10;
                }
                _defaultPageSize = value;
            }
        }
    }
}