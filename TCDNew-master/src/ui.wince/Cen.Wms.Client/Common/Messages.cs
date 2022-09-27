namespace Cen.Wms.Client.Common
{
    public class Messages
    {
        public const string LoggerLocalName = "Cen.Wms";
        public const string LoggerNetName = "Cen.Wms.Client";

        public const string TitleMessage = "Сообщение!";
        public const string TitleError = "Ошибка!";
        public const string TitleConfirm = "Вы уверены?";

        public const string ErrorUnknown = "Произошла неизвестная ошибка!";
        public const string ErrorTransport = "Произошла ошибка при передаче данных!";
        public const string ErrorServer = "Произошла ошибка (сервер):";
        public const string ErrorAuthorisation = "Ошибка авторизации";
        public const string ErrorPasswordIncorrect = "Неверный пароль!";

        public const string ErrorSyncTime = "Произошла ошибка при синхронизации времени!!!";
        public const string ErrorPurchaseTaskLineReadByBarcode = "Произошла ошибка при поиске товара!!!";
        public const string ErrorPurchaseTaskUpdateRead = "Произошла ошибка при получении содержимого задания!!!";
        public const string ErrorPurchaseTaskCreate = "Произошла ошибка при создании задания!!!";
        public const string ErrorPurchaseTaskLineUpdatePost = "Произошла ошибка при отправке изменений!!!";
        public const string ErrorPurchaseTaskFinish = "Произошла ошибка при завершении задания!!!";
        public const string ErrorPurchaseTaskListReadByPerson = "Произошла ошибка при получении заданий!!!";
        public const string ErrorPacHeadReadByBarcode = "Произошла ошибка при поиске закупки!!!";
        public const string ErrorUserLogin = "Произошла ошибка при проверке имени пользователя и пароля!!!";
        public const string ErrorFacilityConfigGet = "Произошла ошибка при получении настроек склада!!!";
        public const string ErrorFacilityListSimpleReadByPerson = "Произошла ошибка при получении списка складов!!!";
    }
}
