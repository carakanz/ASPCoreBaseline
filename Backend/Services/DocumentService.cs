using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class DocumentService
    {
        private readonly IGridFSBucket _documents;

        public DocumentService(Models.IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.MongoDBConnection);
            var database = client.GetDatabase(settings.MongoDBName);

            _documents = new GridFSBucket(database);
        }

        public async Task<GridFSDownloadStream<ObjectId>> GetAsync(string id) =>
            await _documents.OpenDownloadStreamAsync(new ObjectId(id));

        public async Task<string> CreateAsync(Stream source, string name)
        {
            var id = await _documents.UploadFromStreamAsync(name, source);
            return id.ToString();
        }

        public async void UpdateAsync(string id, Stream source, string name) =>
            await _documents.UploadFromStreamAsync(new ObjectId(id), name, source);

        public async void RemoveAsync(string id) =>
            await _documents.DeleteAsync(new ObjectId(id));
    }
}
