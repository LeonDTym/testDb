using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
    internal class DataForSendServer
    {
        #region Переменные

        private const int cByte = 19;

        /// <summary>
        /// 1 - версия структуры пакета  размер 1-байт для подписи
        /// </summary>
        private byte Versions { get { return 0x055; } }

        /// <summary>
        /// 2 - длина оставщейся части пакета размер 4-байта для подписи
        /// </summary>
        private int LengthSign { get { return 19 + _LengthDate + _LengthKey + _LengthPasswordKey; } }
        private byte[] _bLengthSign { get { return BitConverter.GetBytes(LengthSign); } }

        /// <summary>
        /// 2 - длина оставшейся части пакета размер 4-байта для проверки подписи
        /// </summary>
        private int LengthVerify { get { return 14 + _LengthDate; } }
        private byte[] _bLengthVerify { get { return BitConverter.GetBytes(LengthVerify); } }

        /// <summary>
        /// 3 - кодировка данных размер 1-байт
        /// </summary>
        private byte Coding { get { return 0x000; } }

        /// <summary>
        /// 4 - флаг операции размер 4-байта
        /// </summary>
        public int Flag
        {
            get { return BitConverter.ToInt32(_Flag, 0); }
            set { _Flag = BitConverter.GetBytes(value); }
        }

        private byte[] _Flag;
        /// <summary>
        /// 5 - личный ключ размер 4-байта
        /// </summary>
        private int _LengthKey = 1;
        private byte[] Key { get { return BitConverter.GetBytes(_LengthKey); } }

        /// <summary>
        /// 6 - данные о личном ключе N-байт
        /// </summary>
        private byte[] _InfoKey = { 0 };

        /// <summary>
        /// 7 - длина пароля размер 1-байт
        /// </summary>
        private byte _LengthPasswordKey;

        private byte[] _Password;
        /// <summary>
        /// 8 - пароль размер M-байт
        /// </summary>
        public string Password
        {
            get { return System.Text.Encoding.UTF8.GetString(_Password, 0, _Password.Length); }
            set
            {
                _Password = System.Text.Encoding.UTF8.GetBytes(value);
                _LengthPasswordKey = byte.Parse(_Password.Length.ToString());
            }
        }

        /// <summary>
        /// 9 - длина данных размер 4-байта
        /// </summary>
        private int _LengthDate;
        private byte[] _lDate { get { return BitConverter.GetBytes(_LengthDate); } }

        private byte[] _Date;
        /// <summary>
        /// 10 - данные размер K-байт
        /// </summary>
        public string Date
        {
            get { return System.Text.Encoding.UTF8.GetString(_Date, 0, _Date.Length); }
            set
            {
                _Date = System.Text.Encoding.UTF8.GetBytes(value);
                _LengthDate = _Date.Length;
            }
        }

        #endregion

        #region Конструкторы

        /// <summary>
        /// Конструктор для инициализации данных для подписи
        /// </summary>
        /// <param name="Password">Пароль</param>
        /// <param name="DateInfo">Данные</param>
        //public DataForSendServer(string Password, string DateInfo)
        //{
        //    this.Password = Password;
        //    this.Date = DateInfo;
        //    this.Flag = 1;
        //}
        /// <summary>
        /// Конструктор для инициалазации данных для проверки подписи
        /// </summary>
        /// <param name="DateInfo">Массив данных</param>
        public DataForSendServer(List<byte> DateInfo)
        {
            this._Date = DateInfo.ToArray();
            this._LengthDate = _Date.Length;
            this.Flag = 2;
        }

        #endregion

        #region Методы
        /// <summary>
        /// Формирование массива для отправки сообщения на подпись
        /// </summary>
        /// <returns></returns>
        public byte[] SendMessageSign()
        {
            byte[] result = new byte[LengthSign];
            int index = 0;
            result[index] = Versions;
            index++;
            _bLengthSign.CopyTo(result, index);
            index += 4;
            result[5] = Coding;
            index++;
            _Flag.CopyTo(result, index);
            index += 4;
            Key.CopyTo(result, index);
            index += 4;
            _InfoKey.CopyTo(result, index);
            index += _LengthKey;
            result[index] = _LengthPasswordKey;
            index++;
            _Password.CopyTo(result, index);
            index += _LengthKey;
            _lDate.CopyTo(result, index);
            index += 4;
            _Date.CopyTo(result, index);
            return result;
        }

        /// <summary>
        /// Формирование массива для отправки сообщения на проверку
        /// </summary>
        /// <returns></returns>
        public byte[] SendMessageVerifying()
        {
            byte[] result = new byte[LengthVerify];
            int index = 0;
            result[index] = Versions;
            index++;
            _bLengthVerify.CopyTo(result, index);
            index += 4;
            result[5] = Coding;
            index++;
            _Flag.CopyTo(result, index);
            index += 4;
            _lDate.CopyTo(result, index);
            index += 4;
            _Date.CopyTo(result, index);
            return result;
        }

        #endregion
    }
}
