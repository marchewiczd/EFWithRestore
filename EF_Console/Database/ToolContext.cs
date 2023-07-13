using EF_Console.Database.Tables;
using EF_Console.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EF_Console.Database
{
    public class ToolContext : DbContext
    {
        private static readonly Dictionary<Type, List<object>> _records = new();
        private string _connectionString = "very important connection string";

        protected DbSet<TableTwo> TableTwo { get; set; }
        protected DbSet<TableOne> TableOne { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        }

        public T? Find<T>(object key) where T : TableBase
        {
            var record = GetDbSet<T>()?.Find(key);

            if (_records.TryGetValue(typeof(T), out var list))
                list.Add(record.Clone());
            else
                _records.Add(typeof(T), new List<object>() { record.Clone() });

            return record;
        }
        
        public new int Update<T>(T record) where T : TableBase
        {
            var dbSet = GetDbSet<T>();

            if (dbSet is null)
                return -1;

            dbSet.Update(record);
            return SaveChanges();
        }

        public void Restore<T>() where T : TableBase
        {
            List<T>? oldRecordsForT = _records.First(x => x.Key == typeof(T)).Value.OfType<T>().ToList();
            DbSet<T>? currentRecordsSet = GetDbSet<T>();

            if (!oldRecordsForT.Any() || !currentRecordsSet.Any())
                return;

            foreach (var record in oldRecordsForT)
            {
                var changedRecord = currentRecordsSet.Find(record.GetKey());

                if (changedRecord is null)
                    continue;

                record.CopyValues(changedRecord);
                currentRecordsSet.Update(changedRecord);
            }

            SaveChanges();
        }

        private DbSet<T>? GetDbSet<T>() where T : TableBase =>
            GetType().GetProperty(typeof(T).Name, BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(this, null) as DbSet<T>;
    }
}
