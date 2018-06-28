using System;
using System.Text.RegularExpressions;

namespace Djl.Core.Extension
{
    public static class StringExtension
    {
        public static bool IsNull(this string str)
        {
            return str == null;
        }

        public static bool IsNullOrEmpty(this string inputStr)
        {
            return string.IsNullOrEmpty(inputStr);
        }

        public static bool IsNullOrWhiteSpace(this string inputStr)
        {
            return string.IsNullOrWhiteSpace(inputStr);
        }

        public static string Format(this string inputStr, params object[] obj)
        {
            return string.Format(inputStr, obj);
        }

        private static readonly Regex EmailRegex = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);

        private static readonly Regex MobileRegex = new Regex("^1[0-9]{10}$");

        private static readonly Regex PhoneRegex = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");

        private static readonly Regex IpRegex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");

        private static readonly Regex DateRegex = new Regex(@"(\d{4})-(\d{1,2})-(\d{1,2})");

        private static readonly Regex NumericRegex = new Regex(@"^[-]?[0-9]+(\.[0-9]+)?$");

        private static readonly Regex ZipcodeRegex = new Regex(@"^\d{6}$");

        private static readonly Regex IdRegex = new Regex(@"^[1-9]\d{16}[\dXx]$");

        /// <summary>
        /// 是否中文
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            return Regex.IsMatch(@"^[\u4e00-\u9fa5]+$", str);
        }

        /// <summary>
        /// 是否为邮箱名
        /// </summary>
        public static bool IsEmail(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return EmailRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为手机号
        /// </summary>
        public static bool IsMobile(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return MobileRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为固话号
        /// </summary>
        public static bool IsPhone(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return PhoneRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIp(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return IpRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是身份证号
        /// </summary>
        public static bool IsIdCard(this string idCard)
        {
            if (string.IsNullOrWhiteSpace(idCard))
                return false;
            return IdRegex.IsMatch(idCard);
        }

        /// <summary>
        /// 是否为日期
        /// </summary>
        public static bool IsDate(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return false;
            return DateRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是数值(包括整数和小数)
        /// </summary>
        public static bool IsNumeric(this string numericStr)
        {
            if (string.IsNullOrWhiteSpace(numericStr))
                return false;
            return NumericRegex.IsMatch(numericStr);
        }

        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        public static bool IsZipCode(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return true;
            return ZipcodeRegex.IsMatch(s);
        }

        /// <summary>
        /// 是否是图片文件名
        /// </summary>
        /// <returns> </returns>
        public static bool IsImgFileName(this string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;
            if (fileName.IndexOf(".", StringComparison.Ordinal) == -1)
                return false;

            string tempFileName = fileName.Trim().ToLower();
            string extension = tempFileName.Substring(tempFileName.LastIndexOf(".", StringComparison.Ordinal));
            return extension == ".png" || extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif";
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="inputStr">输入</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string Sub(this string inputStr, int length)
        {
            if (inputStr.IsNullOrEmpty())
                return inputStr;

            return inputStr.Length >= length ? inputStr.Substring(0, length) : inputStr;
        }

        public static string TryReplace(this string inputStr, string oldStr, string newStr)
        {
            return inputStr.IsNullOrEmpty() ? inputStr : inputStr.Replace(oldStr, newStr);
        }

        public static string RegexReplace(this string inputStr, string pattern, string replacement)
        {
            return inputStr.IsNullOrEmpty() ? inputStr : Regex.Replace(inputStr, pattern, replacement);
        }

        /// <summary>
        /// 字符格式化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string Fmt(this string input, params object[] param)
        {
            if (input.IsNullOrWhiteSpace())
                return input;

            var result = string.Format(input, param);
            return result;
        }

        /// <summary>
        /// 格式化电话
        /// </summary>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public static string FmtMobile(this string mobile)
        {
            if (!mobile.IsNullOrEmpty() && mobile.Length > 7)
            {
                var regx = new Regex(@"(?<=\d{3}).+(?=\d{4})", RegexOptions.IgnoreCase);
                mobile = regx.Replace(mobile, "****");
            }

            return mobile;
        }

        /// <summary>
        /// 格式化证件号码
        /// </summary>
        /// <param name="idCard"></param>
        /// <returns></returns>
        public static string FmtIdCard(this string idCard)
        {
            if (!idCard.IsNullOrEmpty() && idCard.Length > 10)
            {
                var regx = new Regex(@"(?<=\w{6}).+(?=\w{4})", RegexOptions.IgnoreCase);
                idCard = regx.Replace(idCard, "********");
            }

            return idCard;
        }

        /// <summary>
        /// 格式化银行卡号
        /// </summary>
        /// <param name="bankCark"></param>
        /// <returns></returns>
        public static string FmtBankCard(this string bankCark)
        {
            if (!bankCark.IsNullOrEmpty() && bankCark.Length > 4)
            {
                var regx = new Regex(@"(?<=\d{4})\d+(?=\d{4})", RegexOptions.IgnoreCase);
                bankCark = regx.Replace(bankCark, " **** **** ");
            }

            return bankCark;
        }
    }
}
