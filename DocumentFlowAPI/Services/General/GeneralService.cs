using DocumentFlowAPI.Interfaces.Services;

namespace DocumentFlowAPI.Services.General;

public abstract class GeneralService
{
    Checker checker = new Checker();

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
    public class Checker
    {
        internal static void UniversalCheck<T>(CheckerParam<T> param)
        {
            if (param.Predicate)
            {
                throw new Exception(param.Exception.Message, param.Exception).InnerException;
            }
        }
    }
    public class CheckerParam<T>
    {
        public Exception Exception { get; set; }
        public bool Predicate { get; set; }
        public T[] Target { get; set; }

        public CheckerParam(
            Exception exception, //исключение
            Predicate<T[]> predicate, //условие
            params T[]  target) //объект проверки
        {
            Exception = exception;
            Predicate = predicate(target);
            Target = target;
        }
        public CheckerParam()
        {

        }
    }
    // Пример реализации
    // var checker = new Checker();
    // checker.UniversalCheck<int>(new CheckerParam<int>(
    //     new ArgumentException("Отрицательное"),
    //     param => param > 0,
    //     65);
}
