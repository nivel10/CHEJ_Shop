namespace CHEJ_Shop.Common.Helpers
{
    using Common.Models;
    using System;
    using System.Net.Mail;

    public class RegexHelper
    {
        public static Response IsValidEmail(
            string _email)
        {
            try
            {
                var main = new MailAddress(_email);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Method is ok...!!!",
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    //  TODO: Add multi language message
                    Message = ex.Message,
                };
            }
        }
    }
}