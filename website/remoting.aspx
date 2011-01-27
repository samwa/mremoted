<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="remoting.aspx.cs" Inherits="Samwa.Remoting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel runat="server" ID="pnlRemoteClass" Visible="false">
            Total Clients: <asp:Label runat="server" ID="lblTotalClients"></asp:Label><br />
            Pi: <asp:Label runat="server" ID="lblPi"></asp:Label><br />
            Iteration: <asp:Label runat="server" ID="lblIteration"></asp:Label><br />
            Elapsed Time: <asp:Label runat="server" ID="lblElapsedTime"></asp:Label>
        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlError" Visible="false">
            <asp:Label runat="server" ID="lblErrorMessage"></asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
