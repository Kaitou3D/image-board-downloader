using ImageBoardProcessor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ImageBoardProcessor.Serializers
{
    public static class QuerySerilizer
    {

         

        public static void SaveQuery(Query query)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Query));
            TextWriter writer = new StreamWriter($@"{query.downloadDirectory}\{query.searchName}.xml");
            serializer.Serialize(writer, query);

        }

        public static Query LoadQuery(string file)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Query));
            Query loadedQuery;
            FileStream fs = new FileStream(file, FileMode.Open);

            loadedQuery = (Query)serializer.Deserialize(fs);

            return loadedQuery ;
        }
    }
}
