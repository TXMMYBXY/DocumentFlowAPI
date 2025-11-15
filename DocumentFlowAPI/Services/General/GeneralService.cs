namespace DocumentFlowAPI.Services.General;

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
    
    class Checker
    {
        internal void UniversalCheck<T>(CheckerParam<T> param)
        {
            if (param.Predicate)
            {
                throw new Exception(param.Message, param.Exception).InnerException;
            }
        }
    }
    class CheckerParam<T>
    {
        public T User { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public bool Predicate { get; set; }

        public CheckerParam(
            T user,
            string message,
            Exception exception,
            Predicate<T> predicate)
        {
            User = user;
            Message = message;
            Exception = exception;
            Predicate = predicate(user);
        }
        public CheckerParam()
        {

        }
    }
    ///Пример реализации
    /// var checker = new Checker();
    // checker.UniversalCheck<int>(new CheckerParam<int>(
    //     65,
    //     $"Отрицательное",
    //     new ArgumentException(),
    //     (param) => param > 0));

}