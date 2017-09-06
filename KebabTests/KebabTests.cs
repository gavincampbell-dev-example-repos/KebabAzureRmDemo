using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace KebabTests
{
    [TestClass]
    public class KebabTests
    {
       
        static private string functionUrl;
        static private Uri docDbUri;
        static private string docDbKey;

        [ClassInitialize]
        static public void ClassInitialize(TestContext testContext)
        {
            functionUrl = testContext.Properties["functionUrl"].ToString();
            docDbUri = new Uri( testContext.Properties["docDbUri"].ToString());
            docDbKey = testContext.Properties["docDbKey"].ToString();
            DeleteAllDocsFromCollection("kebabDb", "kebabPreferences").Wait();

        }
        [TestMethod]
        public void KebabPost_ValidInput_StoresValuesInDb()
        {
            string content = "{ 'name':'Barry' , 'favouriteKebab':'Kofte' }";

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(functionUrl,
                    new StringContent(content, Encoding.UTF8, "application/json")).Result;
               
            }

            using (var client = new DocumentClient(docDbUri, docDbKey))
            {
                IQueryable<KebabPrefs> kebabPrefsQuery = client.CreateDocumentQuery<KebabPrefs>(
                    UriFactory.CreateDocumentCollectionUri("kebabDb", "kebabPreferences"))
                    .Where(kp => kp.name == "Barry");

                Assert.AreEqual("Kofte", kebabPrefsQuery.ToList().First().favouriteKebab);


            }

            }
        private static async Task DeleteAllDocsFromCollection(string dbName, string collectionName)
        {
            using (var client = new DocumentClient(docDbUri, docDbKey))
            {
                var db = client.CreateDatabaseQuery().Where(d => d.Id == dbName).ToList().First();
                var dc = client.CreateDocumentCollectionQuery(db.CollectionsLink).Where(c => c.Id == collectionName).ToList().First();
                var docs = client.CreateDocumentQuery(dc.DocumentsLink);

                foreach (var d in docs)
                {
                    await client.DeleteDocumentAsync(d.SelfLink);
                }


            }
        }
    }


}
