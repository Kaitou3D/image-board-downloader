using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageBoardProcessor.Models;

namespace ImageBoardProcessor.Interfaces
{
    public interface IImgBoard
    {
        //QueryModel query{get; set;}
        Task BuildQuery();
        Task<List<E621Model>> ExecuteSearch<T>(T tags) where T : IList<String>;
    }
}
