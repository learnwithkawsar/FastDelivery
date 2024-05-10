﻿using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Persistence.Mongo;
public class MongoDbContext : IMongoDbContext
{
    public IMongoDatabase Database { get; }
    public IMongoClient MongoClient { get; }

    public MongoDbContext(IOptions<MongoOptions> options)
    {
        RegisterConventions();

        MongoClient = new MongoClient(options.Value.ConnectionString);
        string databaseName = options.Value.DatabaseName;
        Database = MongoClient.GetDatabase(databaseName);
    }

    private static void RegisterConventions()
    {
        ConventionRegistry.Register(
            "conventions",
            new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new IgnoreIfNullConvention(true),
                new EnumRepresentationConvention(BsonType.String),
                new IgnoreIfDefaultConvention(false)
            }, _ => true);
    }

    public IMongoCollection<T> GetCollection<T>(string? name = null)
    {
        return Database.GetCollection<T>(name ?? typeof(T).Name.ToLower());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}