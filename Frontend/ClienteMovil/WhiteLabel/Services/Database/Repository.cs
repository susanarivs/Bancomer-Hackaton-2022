using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhiteLabel.Services.Database
{
    sealed class Repository<T> where T : class, new()
    {
        private readonly ISQLitePlatform _platform;

        private static Repository<T> _instance = null;

        internal static Repository<T> Instance()
        {
            if (_instance == null)
            {
                _instance = new Repository<T>();
            }

            return _instance;
        }

        private Repository()
        {
            _platform = DependencyService.Get<ISQLitePlatform>();
            var con = _platform.GetConnection();
            con.CreateTable<T>();
            con.Close();
        }

        public async Task<bool> AddItemAsync(T item)
        {
            return (await _platform.GetConnectionAsync().InsertAsync(item)) > 0;
        }

        public async Task<int> AddItemAsyncWithReturn(T item)
        {
            return await _platform.GetConnectionAsync().InsertAsync(item);
        }

        public async Task<bool> UpdateItemAsync(T item)
        {
            var res = (await _platform.GetConnectionAsync().UpdateAsync(item)) > 0;
            return res;
        }

        public async Task<bool> DeleteItemAsync(T item)
        {
            return (await _platform.GetConnectionAsync().DeleteAsync(item)) > 0;
        }

        public IEnumerable<T> GetItems()
        {
            var connection = _platform.GetConnection();
            var table = connection.Table<T>();
            var list = table.ToList();

            return list;
        }

        public IEnumerable<T> GetItems(Expression<Func<T, bool>> predicate)
        {
            var connection = _platform.GetConnection();
            var table = connection.Table<T>().Where(predicate);
            var list = table.ToList();

            return list;
        }

        public T GetItem(Expression<Func<T, bool>> predicate)
        {
            var connection = _platform.GetConnection();
            var table = connection.Table<T>();
            var data = table.Where(predicate);
            var final = data.FirstOrDefault();

            return final;
        }
    }
}
