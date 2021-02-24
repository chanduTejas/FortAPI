
namespace FortCode.Common
{
    public static class SqlQueries
    {
        public const string InsertUserQuery = @"INSERT INTO Fort_Users
                                                VALUES (
	                                                @username
	                                                ,@password
	                                                ,@email
	                                                )";

		public const string UserExistQuery = @"SELECT COUNT(1)
																FROM Fort_Users WITH (NOLOCK)
																WHERE Email = @email";

        public const string AuthenticateUserQuery = @"SELECT Idx as Id,UserName as Name, Email, Password
																FROM Fort_Users WITH (NOLOCK)
																WHERE Email = @email";

        public const string GetAllCountryByUserQuery = @"SELECT CountryID, Country  AS CountryName
														,City
													FROM Fort_Users_FavoriteCity WITH (NOLOCK)
													WHERE userId = @Id";

		public const string InsertCountryQuery = @"INSERT INTO Fort_Users_FavoriteCity
                                                VALUES (
	                                                @userId
	                                                ,@CountryName
	                                                ,@City
	                                                )";

		public const string DeleteCountry = @"DELETE
												FROM Fort_Users_FavoriteCity
												WHERE CountryID = @CountryID";

	}
}
