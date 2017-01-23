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

            var paramOut = new SqlParameter();
            paramOut.ParameterName = "@output";
            paramOut.SqlDbType = SqlDbType.Int;
            paramOut.Direction = ParameterDirection.Output;

            //var outParam = new SqlParameter("@ReturnCode", SqlDbType.Int);
            //outParam.Direction = ParameterDirection.Output;

            SqlParameter paramName = new SqlParameter("@name", authorName);
            SqlParameter paramSurname = new SqlParameter("@surname", authorSurname);
            var data = dbLibrary.Database.SqlQuery<object>("checkAuthor @name, @surname, @output OUTPUT", paramName, paramSurname, paramOut);
            authorId = (int)paramOut.Value;
            System.Diagnostics.Debug.WriteLine("Returned Author Id: " + authorId);
        }
    }
}