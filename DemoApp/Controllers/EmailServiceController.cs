using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using EmailService.Models;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EmailService.Controllers
{

  [Route("api/SendMail")]
  [ApiController]
  public class EmailServiceController : ControllerBase

  {
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    [HttpGet]
    public string GetAppVersion()
    {
      return Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    [HttpPost]
    public string SendEmail([FromBody] EmailModel emailModel)
    {
      string fromEmail = "mounika.aileni24@gmail.com";
      string[] multipleToEmails = emailModel.ToEmails.Split(';');
      string mailBody = GetMailBody(emailModel.Comments);

      MailMessage message = new MailMessage();
      message.From = new MailAddress(fromEmail);

      foreach (string toEmail in multipleToEmails)
      {
        if (!string.IsNullOrEmpty(toEmail))
        {
          message.To.Add(new MailAddress(toEmail));
        }
      }

      message.Body = mailBody;
      message.IsBodyHtml = true;

      SmtpClient smtpClient = new SmtpClient();
      string smtpHost = "smtp.gmail.com";
      string smtpPort = "587";
      smtpClient.EnableSsl = true;
      NetworkCredential nc = new NetworkCredential("mounika.aileni24@gmail.com", "***");
      smtpClient.Credentials = nc;
      smtpClient.Host = smtpHost;

      if (!string.IsNullOrEmpty(smtpPort))
      {
        smtpClient.Port = Convert.ToInt32(smtpPort);
      }
      try
      {
        smtpClient.Send(message);
        Log.Info("E-Mail has been sent to: " + emailModel.ToEmails + " with Comments: '" + emailModel.Comments + "'");
        return "success";
      }
      catch (Exception error)
      {
        Log.Error("E-Mail has been requested by: " + emailModel.ToEmails + ". But request failed to process with error  " + error);
        return error.ToString();
      }
    }

    private static string GetMailBody(string comments)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("<!DOCTYPE html><html><head><title></title><meta charset = " + "utf-8" + " />");
      sb.Append("<style>table td{ padding: 0.5em }body{font - family:Courier New;font - size:15px;}</style></head>");
      sb.Append("<body><table><tr><td colspan = " + "2" + ">" + "******* This is an auto-generated e-mail. Please do not reply********" + "</td>");
      sb.Append("</tr><tr><td colspan = " + "2" + "> Hi,</td></tr><tr>");
      sb.Append("<tr><td colspan = " + "2" + ">Comments : " + comments + "<br></td></tr>");
      sb.Append("<tr><td colspan = " + "2" + ">Regards,<br/>App Admin</td></tr></table></body></html>");

      return sb.ToString();
    }
  }
}