using System.Net;
using System.Net.Mail;
using Order.Application.Dtos.Order;
using Order.Application.Interfaces;

namespace Order.Application.Services.Notifications;

public class EmailNotificationObserver : IOrderObserver
{
    public async Task OnOrderPlaced(CustomerOrderDto customerOrder)
    {
        using var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("gelab2109@gmail.com", "wulyylslqxdqgtvi"),
            EnableSsl = true
        };

        using var mailMessage = new MailMessage
        {
            From = new MailAddress("gelab2109@gmail.com"),
            Subject = "Order Confirmation",
            Body = $@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
              <meta charset=""UTF-8"" />
              <title>Order Confirmation</title>
            </head>
            <body style=""margin:0; padding:0; background:#f5f6fa; font-family:Arial, Helvetica, sans-serif;"">
              <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background:#f5f6fa; padding:24px 0;"">
                <tr>
                  <td align=""center"">
                    <table role=""presentation"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""width:600px; background:#ffffff; border-radius:14px; overflow:hidden; box-shadow:0 6px 18px rgba(0,0,0,0.08);"">
                      
                      <tr>
                        <td style=""padding:22px 28px; background:#111827;"">
                          <div style=""color:#ffffff; font-size:18px; font-weight:700;"">
                            Order Placed Successfully! 🎉
                          </div>
                          <div style=""color:#cbd5e1; font-size:13px; margin-top:6px;"">
                            Thank you for your order
                          </div>
                        </td>
                      </tr>

                      <tr>
                        <td style=""padding:28px;"">
                          <div style=""font-size:15px; color:#111827; line-height:1.6;"">
                            Hi!,<br /><br />
                            Your order has been placed successfully and is being processed.
                          </div>

                          <div style=""margin:18px 0 10px; text-align:center;"">
                            <div style=""display:inline-block; padding:14px 18px; border-radius:12px; background:#f3f4f6; border:1px solid #e5e7eb;"">
                              <span style=""font-size:16px; font-weight:700; color:#111827;"">
                                Order Total: {customerOrder.Total:C}<br/>
                                Order Date: {customerOrder.CreatedAt}<br/>
                                Order ID: #{customerOrder.Id}
                              </span>
                            </div>
                          </div>

                          <div style=""font-size:13px; color:#6b7280; margin-top:12px;"">
                            We will notify you once your order has been shipped.
                          </div>

                          <hr style=""border:none; border-top:1px solid #e5e7eb; margin:22px 0;"" />

                          <div style=""font-size:12px; color:#9ca3af;"">
                            If you did not place this order, please contact support immediately.
                          </div>
                        </td>
                      </tr>

                      <tr>
                        <td style=""padding:18px 28px; background:#f9fafb; font-size:12px; color:#9ca3af;"">
                          © Order Management • This is an automated message, please don't reply.
                        </td>
                      </tr>

                    </table>
                  </td>
                </tr>
              </table>
            </body>
            </html>",
            IsBodyHtml = true
        };

        mailMessage.To.Add("L.Karkarashvili8@gmail.com");

        await smtp.SendMailAsync(mailMessage);
    }
}