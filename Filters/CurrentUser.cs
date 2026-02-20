namespace Quote_Tracker.Filters
{
    public static class CurrentUser
    {
        public static int? GetUserId(HttpContext httpContext)
        {
            if (httpContext.Items["UserId"] is int id)
                return id;
            return httpContext.Session.GetInt32(RequireAuthAttribute.SessionUserIdKey);
        }
    }
}
