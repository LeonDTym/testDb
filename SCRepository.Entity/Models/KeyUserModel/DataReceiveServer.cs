using System;
using System.Collections.Generic;
using System.Text;

namespace SCRepository.Entity.Models.KeyUserModel
{
    public class DataReceiveServer
    {
        #region Переменные
        /// <summary>
        ///  Версия структуры пакета (1 байт)
        /// </summary>
        private byte _infoPaket;

        public byte InfoPaket
        {
            get { return _infoPaket; }
        }

        /// <summary>
        /// 2 - Длина оставшейся части пакета (4 байта)
        /// </summary>
        private byte[] _lengthPacket;

        public int LengthPacket
        {
            get { return BitConverter.ToInt32(_lengthPacket, 0); }
        }
        /// <summary>
        /// 3 - Кодировка (1 байт) 
        /// </summary>
        private byte _coding;

        /// <summary>
        /// 4 - Длина выходных данных (4 байта. при ошибке в криптообработке равна НУЛЮ)
        /// </summary>
        private byte[] _lengthDate;

        public int LengthDate
        {
            get { return BitConverter.ToInt32(_lengthDate, 0); }
        }

        /// <summary>
        /// 5 - Выходные данные (присутствуют только при успешном завершении криптооперации)
        /// </summary>
        private byte[] _Date;

        public byte[] bDate
        {
            get { return _Date; }
        }

        public string Date
        {
            get
            {
                string result = System.Text.Encoding.UTF8.GetString(_Date, 0, LengthDate);
                int code = result.IndexOf("0A73070000");
                if (code > 0)
                {
                    result = result.Substring(0, code);
                    int length = System.Text.Encoding.UTF8.GetBytes(result).Length;
                    byte[] ncode = new byte[LengthDate - length];
                    Array.Copy(_Date, length, ncode, 0, LengthDate - length);
                    StringBuilder info = new StringBuilder();
                    foreach (var item in ncode)
                        info.Append(ConvertToChar(item));
                    result = String.Format("{0}{1}", result, info.ToString());
                }
                return result;
            }
        }

        /// <summary>
        /// 6 - Длина отчёта о криптооперации (4 байта), равная НУЛЮ, если криптооперация завершилась успешно, либо длине п.8 + 4
        /// </summary>
        private byte[] _lengtherror;
        public int LengthError
        {
            get
            {
                return BitConverter.ToInt32(_lengtherror, 0);
            }
        }

        /// <summary>
        /// 7) Код ошибки (4 байта) - присутствует только при ошибочном завершении криптооперации
        /// </summary>
        private byte[] _codeerror;

        public int CodeError
        {
            get { return BitConverter.ToInt32(_codeerror, 0); }
        }

        /// <summary>
        /// 8) Текстовое сообщение об ошибке - присутствует только при ошибочном завершении криптооперации 
        /// </summary>
        private byte[] _MessageError;

        public string MessageError
        {
            get
            {
                if (_flag == 2)
                {
                    //StringBuilder info = new StringBuilder();
                    //foreach (var item in _MessageError)
                    //    info.Append(ConvertToChar(item));
                    if (_MessageError[LengthError - 1] == 0)
                        return System.Text.Encoding.UTF8.GetString(_MessageError, 0, LengthError - 1);// +"\n" + info.ToString();
                                                                                                      //return System.Text.Encoding.GetEncoding(1251).GetString(_MessageError, 0, LengthError - 1);// +"\n" + info.ToString();
                    return System.Text.Encoding.UTF8.GetString(_MessageError, 0, LengthError); //+ "\n" + info.ToString();
                }
                if (_flag == 1)
                {
                    StringBuilder info = new StringBuilder();
                    if (_MessageError != null)
                    {
                        foreach (var item in _MessageError)
                            info.Append(ConvertToChar(item));
                    }
                    return String.Format("{0}", info.ToString());
                }
                return "";
            }
        }
        /// <summary>
        /// Флаг операции
        /// </summary>
        private int _flag;
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        private string MyError;

        public ResultDataVerificationOnServer verifyinfo { get; set; }
        #endregion
        #region Конструкторы
        /// <summary>
        /// 
        /// </summary>
        /// <param name="receive"></param>
        public DataReceiveServer(byte[] receive)
        {
            try
            {
                MyError = "";
                int index = 0;
                this._infoPaket = receive[index];
                index++;
                _lengthPacket = new byte[4];
                Array.Copy(receive, index, _lengthPacket, 0, 4);
                index += 4;
                _coding = receive[index];
                index++;
                _lengthDate = new byte[4];
                Array.Copy(receive, index, _lengthDate, 0, 4);
                index += 4;
                if (LengthDate > 0)
                {
                    _Date = new byte[LengthDate];
                    Array.Copy(receive, index, _Date, 0, LengthDate);
                }
                index += LengthDate;
                _lengtherror = new byte[4];
                Array.Copy(receive, index, _lengtherror, 0, 4);
                index += 4;
                if (LengthDate == 0)
                {
                    _codeerror = new byte[4];
                    Array.Copy(receive, index, _codeerror, 0, 4);
                    index += 4;
                }
                if (LengthError > 0)
                {
                    _MessageError = new byte[LengthError];
                    Array.Copy(receive, index, _MessageError, 0, LengthDate == 0 ? LengthError - 4 : LengthError);
                    InfoReceive(1);
                }
            }
            catch
            {
                MyError = "Нарушена структура данных: длина полученных данных не соответсвует рельным";
            }
            //this._lengtPacket = BitConverter.ToInt32(receive[index], 1);
        }

        public DataReceiveServer()
        { }

        #endregion
        #region Методы
        /// <summary>
        /// формирует результат работы с буффером от сервиса
        /// </summary>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public string InfoReceive(int Flag)
        {
            _flag = Flag;
            if (MyError.Length == 0)
            {
                StringBuilder result = new StringBuilder();
                switch (_flag)
                {
                    case 1:
                        result.AppendFormat("Версия структуры пакета: {0}\n", _infoPaket.ToString("X"));
                        result.AppendFormat("Длина оставшейся части пакета: {0}\n", LengthPacket);
                        result.AppendFormat("Кодировка: {0}\n", _coding);
                        if (LengthDate > 0)
                        {
                            result.AppendFormat("Длина выходных данных: {0}\n", LengthDate);
                            result.AppendFormat("Выходные данные:\n{0}\n", Date);
                            result.AppendFormat("Журнал: {0}", MessageError);
                        }
                        else if (LengthDate == 0)
                        {
                            result.AppendFormat("Код ошибки подписи документа: {0}\n", CodeError);
                            result.AppendFormat("Ошибка подписи: {0}", MessageError);
                        }
                        break;
                    case 2:
                        if (LengthDate > 0)
                        {
                            verifyinfo = new ResultDataVerificationOnServer(MessageError);
                            result.AppendFormat("{0}", verifyinfo.MyToString());
                        }
                        else if (LengthDate == 0)
                        {
                            result.AppendFormat("Код ошибки проверки ЭЦП документа: {0}\n", CodeError);
                            result.AppendFormat("Ошибка проверки ЭЦП: {0}", MessageError);
                        }
                        break;
                    default:
                        result.AppendFormat("Не известный флаг операции {0}", _flag);
                        break;
                }
                return result.ToString();
            }
            else
                return MyError;

        }

        public ResultDataVerificationOnServer VerifyReceive()
        {
            _flag = 2;
            return new ResultDataVerificationOnServer(MessageError);
        }

        public string InfoIKey(byte[] info)
        {
            if (info.Length > 0)
            {
                //StringBuilder result = new StringBuilder();
                //                Формат ответа:
                //-- версия (1 байт)
                //-- код ошибки (2 байта) . Если 0, то всё норм.
                //-- длина данных (4 байта)
                //-- данные (n байт)
                int index = 1;
                //_codeerror = new byte[2];
                byte[] error = new byte[2];
                Array.Copy(info, index, error, 0, 2);
                index += 2;
                if (BitConverter.ToInt16(error, 0) > 0)
                    return "";
                _lengthDate = new byte[4];
                Array.Copy(info, index, _lengthDate, 0, 4);
                index += 4;
                _Date = new byte[LengthDate];
                Array.Copy(info, index, _Date, 0, LengthDate);
                int[] sn = new int[2];
                sn[0] = BitConverter.ToInt32(_Date, 0);
                sn[1] = BitConverter.ToInt32(_Date, 4);
                return String.Format("{0}{1}", sn[1].ToString("X8"), sn[0].ToString("X8"));
            }
            return "";
        }

        /// <summary>
        /// Конвертация данных в ср-1251
        /// </summary>
        /// <param name="inByte">входной байт с данными</param>
        /// <returns>выходные данные</returns>
        private char ConvertToChar(byte inByte)
        {
            // конверитруемые данные
            char DataChar;
            if (inByte >= 192 && inByte <= 255)           //Символы ('А' -> 'я')
                DataChar = Convert.ToChar(inByte + 848);
            else if (inByte == 168)                  //Символ ('Ё')
                DataChar = Convert.ToChar(inByte + 857);
            else if (inByte == 184)                  //Символ ('ё')
                DataChar = Convert.ToChar(inByte + 921);
            else if (inByte == 161)                  //Символ ('Ў')
                DataChar = Convert.ToChar(inByte + 877);
            else if (inByte == 162)                  //Символ ('ў')
                DataChar = Convert.ToChar(inByte + 956);
            else if (inByte == 185)                  //Символ ('№')
                DataChar = Convert.ToChar(inByte + 8285);
            else DataChar = Convert.ToChar(inByte); //  для остальных символов
            return DataChar;
        }
        #endregion
    }
}
