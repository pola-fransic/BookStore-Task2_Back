namespace BookStoreApp.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToCurrency(this decimal amount)
        {
            return $"{amount:0.00} EGP";
        }
    }
}