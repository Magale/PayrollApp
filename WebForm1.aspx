<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="payroll.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <p>
            <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Test" Width="343px" />
        </p>
        <p>
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Upload File" Height="22px" Width="341px" />
        </p>
        <p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Get report" Width="340px" />
        </p>
    </form>
</body>
</html>
