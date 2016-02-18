using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Login : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
        {
            
        }

        protected void Login1_Authenticate1(object sender, AuthenticateEventArgs e)
        {
            BasicHttpBinding basicHttpbinding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            basicHttpbinding.Name = "BasicHttpBinding_YourName";
            basicHttpbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            basicHttpbinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            EndpointAddress endpointAddress = new EndpointAddress("http://localhost:17476/ProductRESTService.svc");
            ServiceReference1.ProductRESTServiceClient proxyClient = new ServiceReference1.ProductRESTServiceClient(basicHttpbinding, endpointAddress);
            int b = proxyClient.CheckUser(Login1.UserName, Login1.Password);
            Response.Redirect("http://localhost:52045/Files/Category.html");
        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {

        }
    }
}