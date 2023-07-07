namespace EStoreWeb.Routes
{
    public static class RoutesManager
    {
        private const string LocalHostDomain = "http://localhost:5063/api";
        public static string GetUrlPattern(EndPoint endpoint)
        {
            return LocalHostDomain + "/" + endpoint;
        }
        #region product
        public static string GetAllProduct = $"{GetUrlPattern(EndPoint.Product)}/getall";
        public static string AddProduct = $"{GetUrlPattern(EndPoint.Product)}/add";
        #endregion

        #region user
        public static string Login = $"{GetUrlPattern(EndPoint.User)}/login";
        public static string Register = $"{GetUrlPattern(EndPoint.User)}/register";
        public static string UserProfile = $"{GetUrlPattern(EndPoint.User)}/profile";
        public static string GetSaleReport = $"{GetUrlPattern(EndPoint.User)}/salereport";
        #endregion

        #region cart
        public static string AddToCart = $"{GetUrlPattern(EndPoint.Cart)}/add";
        public static string DeleteCart = $"{GetUrlPattern(EndPoint.Cart)}/delete";
        #endregion

        #region order
        public static string OrderHistory = $"{GetUrlPattern(EndPoint.Order)}/orderhistory";
        public static string AddOrder = $"{GetUrlPattern(EndPoint.Order)}/add";
        #endregion
    }
    public enum EndPoint
    {
        Product,
        Order,
        User,
        Category,
        Cart
    }
}
