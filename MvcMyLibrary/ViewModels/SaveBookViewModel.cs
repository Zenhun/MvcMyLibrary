using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcMyLibrary.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MvcMyLibrary.ViewModels
{
    public class SaveBookViewModel
    {
        [Required]
        public CompleteBook Book { get; set; }
        IEnumerable<Genre> genreList;
        [Required]
        public IEnumerable<Genre> GenreList
        {
            get 
            {
                MyLibraryContext dbLibrary = new MyLibraryContext();

                genreList = (from g in dbLibrary.Genres
                             orderby g.GenreName
                             select g).ToList();


                return genreList; 
            }
            set { genreList = value; }
        }
    }
}