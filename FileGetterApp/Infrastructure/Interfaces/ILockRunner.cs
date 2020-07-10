using FileGetterApp.Infrastructure.Models;
using System;
using System.Threading.Tasks;

namespace FileGetterApp.Infrastructure.Interfaces
{ 
    public interface ILockRunner
    {
        /// <summary>
        /// Позволяет выполнить последовательно произвольную функцию,
        /// если функция с переданным ключем в данный момент выполняется, то ожидаем её завершения
        /// </summary>     
        /// <param name="key">имя ключа</param>
        /// <param name="func"> функция для выполнения</param>
        /// <param name="setting">Настройки ожидания</param>      
        Task<T> LockRunAsync<T>(string key, Func<Task<T>> func, LockSettings setting = null);
    }
}
