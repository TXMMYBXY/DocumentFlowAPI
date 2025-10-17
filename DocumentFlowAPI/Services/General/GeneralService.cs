using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentFlowAPI.Services.General
{
    public abstract class GeneralService
    {
        /// <summary>
        /// Проверка на null
        /// </summary>
        /// <typeparam name="T">Тип данных</typeparam>
        /// <param name="target">объект который нужно проверить</param>
        /// <param name="message">сообщение, в случае null</param>
        /// <exception cref="NullReferenceException"></exception>
        protected static void NullCheck<T>(T target, string message)
        {
            if (target == null)
            {
                throw new NullReferenceException(message);
            }
        }
//TODO: Дописать метод проверки на инвалидную операцию
        protected static void InvalidOperationCheck<T>(T target, string message)
        {
            
        }
    }
}