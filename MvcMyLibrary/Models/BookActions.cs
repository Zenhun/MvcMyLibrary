using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcMyLibrary.Models
{
    public class BookActions
    {
        public static void BookSave(string title, string authorName, string authorSurname, int genreId, string imageUrl)
        {
            if (imageUrl == "")
                imageUrl = "noimage.jpg";

            int authorId;

            MyLibraryContext dbLibrary = new MyLibraryContext();

            var returnCode = new SqlParameter();
            returnCode.ParameterName = "@return_value";
            returnCode.SqlDbType = SqlDbType.Int;
            returnCode.Direction = ParameterDirection.Output;

            //var outParam = new SqlParameter("@ReturnCode", SqlDbType.Int);
            //outParam.Direction = ParameterDirection.Output;

            SqlParameter paramName = new SqlParameter("@name", authorName);
            SqlParameter paramSurname = new SqlParameter("@surname", authorSurname);
            var data = dbLibrary.Database.SqlQuery<object>("exec @return_value = checkAuthor @name, @surname", returnCode, paramName, paramSurname);
            authorId = (int)returnCode.Value;
            System.Diagnostics.Debug.WriteLine("Returned Author Id: " + authorId);
        }
    }
}