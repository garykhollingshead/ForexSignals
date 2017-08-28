using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ForexSignals.Data.Persistance;
using Marten;
using Npgsql;

namespace ForexSignals.DataAccess.Adapters
{
    public class MartenAdapter
    {
        private readonly DocumentStore DocumentStore;
        private readonly string _server;
        private readonly string _database;
        private readonly short _port;
        private readonly string _username;
        private readonly string _password;


        public MartenAdapter(string server, string database, short port, string username, string password)
        {
            _server = server;
            _database = database;
            _port = port;
            _username = username;
            _password = password;

            CreateDatabase();

            DocumentStore = DocumentStore.For(Configure);
        }

        private void Configure(StoreOptions storeOptions)
        {
            storeOptions.Connection(GetNpgsqlConnectionString(_database));
        }

        private string GetNpgsqlConnectionString(string database = "postgres")
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = _server,
                Port = _port,
                Username = _username,
                Password = _password,
                Database = database,
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 20,
                ConnectionIdleLifetime = 15
            };

            return builder.ConnectionString;
        }

        public void CreateDatabase()
        {
            using (var conn = new NpgsqlConnection(GetNpgsqlConnectionString()))
            {
                var createDbCmd = new NpgsqlCommand($@"CREATE DATABASE {_database};", conn);
                conn.Open();
                try
                {
                    createDbCmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }

                conn.Close();
            }
            CreateExtension();
        }

        public void CreateExtension()
        {
            using (var conn = new NpgsqlConnection(GetNpgsqlConnectionString(_database)))
            {
                var createExtensionCmd = new NpgsqlCommand($"CREATE EXTENSION PLV8;", conn);
                conn.Open();
                try
                {
                    createExtensionCmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
                conn.Close();
            }
        }

        public void DestroyDatabase()
        {
            DocumentStore.Dispose();
            using (var conn = new NpgsqlConnection(GetNpgsqlConnectionString()))
            {
                var getOpenDbConnsCmd = new NpgsqlCommand($@"SELECT pg_terminate_backend(pg_stat_activity.pid)
                                                            FROM pg_stat_activity
                                                            WHERE pg_stat_activity.datname = '{_database}'
                                                            AND pid <> pg_backend_pid();", conn);
                var deleteDbCmd = new NpgsqlCommand($@"DROP DATABASE {_database};", conn);
                conn.Open();
                try
                {
                    getOpenDbConnsCmd.ExecuteNonQuery();
                    deleteDbCmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                }
                conn.Close();
            }
        }

        public void ClearTable(Type type)
        {
            DocumentStore.Advanced.Clean.DeleteDocumentsFor(type);
        }

        public void Delete<T>(string id) where T : ModelWithIdentity
        {
            using (var session = GetOpenSession())
            {
                session.Delete<T>(id);
                session.SaveChanges();
            }
        }

        public void Delete<T>(List<string> ids) where T : ModelWithIdentity
        {
            using (var session = GetOpenSession())
            {
                ids.ForEach(i => session.Delete<T>(i));
                session.SaveChanges();
            }
        }

        public void Delete<T>(T item) where T : ModelWithIdentity
        {
            using (var session = GetOpenSession())
            {
                session.Delete(item);
                session.SaveChanges();
            }
        }

        public void Delete<T>(List<T> items) where T : ModelWithIdentity
        {
            using (var session = GetOpenSession())
            {
                items.ForEach(i => session.Delete(i));
                session.SaveChanges();
            }
        }

        public void Delete<T>(Expression<Func<T, bool>> query) where T : ModelWithIdentity
        {
            using (var session = GetOpenSession())
            {
                session.DeleteWhere(query);
                session.SaveChanges();
            }
        }

        public T Upsert<T>(T thing) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.OpenSession())
            {
                session.Store(thing);
                session.SaveChanges();
            }

            return thing;
        }

        public List<T> Upsert<T>(List<T> things) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.OpenSession())
            {
                session.StoreObjects(things);
                session.SaveChanges();
            }

            return things;
        }

        public T QueryFirstOrDefault<T>(Expression<Func<T, bool>> query) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.QuerySession())
            {
                return session.Query<T>().Where(query).FirstOrDefault();
            }
        }

        public T QuerySingle<T>(Expression<Func<T, bool>> query) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.QuerySession())
            {
                return session.Query<T>().Where(query).Single();
            }
        }

        public List<T> QueryList<T>(Expression<Func<T, bool>> query) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.QuerySession())
            {
                return session.Query<T>().Where(query).ToList();
            }
        }

        public T GetById<T>(string id) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.QuerySession())
            {
                return session.Load<T>(id);
            }
        }

        public List<T> GetListByIds<T>(List<string> ids) where T : ModelWithIdentity
        {
            using (var session = DocumentStore.QuerySession())
            {
                return session.LoadMany<T>(ids.ToArray()).ToList();
            }
        }

        public R Max<T, R>(Expression<Func<T, R>> query) where T : ModelWithIdentity
        {
            using (var session = GetQuerySession())
            {
                return session.Query<T>().Max(query);
            }
        }

        public R Max<T, R>(Expression<Func<T, bool>> query, Expression<Func<T, R>> value) where T : ModelWithIdentity
        {
            using (var session = GetQuerySession())
            {
                return session.Query<T>().Where(query).Max(value);
            }
        }

        public int Count<T>(Expression<Func<T, bool>> query) where T : ModelWithIdentity
        {
            using (var session = GetQuerySession())
            {
                return session.Query<T>().Count(query);
            }
        }

        public IDocumentSession GetOpenSession()
        {
            return DocumentStore.OpenSession();
        }

        public IQuerySession GetQuerySession()
        {
            return DocumentStore.QuerySession();
        }
    }
}
