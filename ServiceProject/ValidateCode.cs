using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace ServiceProject
{
    public class ValidateCode
    {
        /// <summary>
        /// 生成验证码数字
        /// </summary>
        /// <param name="id">验证码位数，默认为4</param>
        /// <returns></returns>
        public string NewValidateCode(int id)
        {
            ValidatorCode obj = new ValidatorCode();
            return obj.CreateValidateCode(id);
        }
        /// <summary>
        /// 验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public byte[] NewValidateCodeGraphic(string code)
        {
            ValidatorCode obj = new ValidatorCode();
            return obj.CreateValidateGraphic(code);
        }
    }
}
